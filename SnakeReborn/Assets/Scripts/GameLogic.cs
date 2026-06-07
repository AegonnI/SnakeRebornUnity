using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public GameObject snakePlayer;
    public GameObject apple;
    public GameObject deathLaser;
    public GameObject slicerLaser;
    public float border;
    public int numOfSlicers;
    public Button continueButton;
    public Button exitToMenuButton;
    public Button exitButton;
    public GameObject Menu;

    public TextMeshProUGUI score;
    public TextMeshProUGUI timerText;

    [Header("Laser telegraph")]
    public float laserTelegraphDuration = 0.55f;
    public AnimationCurve telegraphWidthMultiplier = AnimationCurve.Linear(0f, 0.35f, 1f, 1.15f);
    public Material telegraphLineMaterial;
    public Color deathTelegraphEdge = new Color(0.35f, 1f, 0.7f, 0.95f);
    public Color deathTelegraphCore = new Color(0.1f, 0.35f, 0.25f, 0f);
    public Color slicerTelegraphEdge = new Color(1f, 0.4f, 0.55f, 0.95f);
    public Color slicerTelegraphCore = new Color(0.4f, 0.1f, 0.18f, 0f);

    private Camera cam;
    private Vector2 topLeft;
    private Vector2 bottomRight;

    /// <summary>Global time-stop for lasers; new lasers read this when spawned.</summary>
    bool _laserTimeStopGlobal;

    public bool LaserTimeStopActive => _laserTimeStopGlobal;

    LaserSpawnCoroutineHost _laserSpawnHost;

    // Свойство для отслеживания уничтожения логики
    private bool _isShuttingDown = false;
    public bool IsShuttingDown => _isShuttingDown;

    void Awake()
    {
        var hostGo = new GameObject("~LaserSpawnCoroutineHost");
        hostGo.transform.SetParent(null);
        _laserSpawnHost = hostGo.AddComponent<LaserSpawnCoroutineHost>();
    }

    void OnDestroy()
    {
        _isShuttingDown = true; // Фиксируем уничтожение объекта
        if (_laserSpawnHost != null)
            Destroy(_laserSpawnHost.gameObject);
    }

    void Start()
    {
        cam = Camera.main;
        float camHeight = cam.orthographicSize * 2;
        float camWidth = camHeight * cam.aspect;
        topLeft = (Vector2)cam.transform.position + new Vector2(-camWidth / 2, camHeight / 2);
        bottomRight = (Vector2)cam.transform.position + new Vector2(camWidth / 2, -camHeight / 2);

        snakePlayer = Instantiate(snakePlayer, new Vector2(0f, 0f), Quaternion.identity, transform);

        snakePlayer.GetComponent<Snake>().appleEated += GenApple;
        snakePlayer.GetComponent<Snake>().timeStoped += InTimeStop;

        GenApple();
        GenLaser(deathLaser);
        for (int i = 0; i < numOfSlicers; i++)
            GenLaser(slicerLaser);

        continueButton.onClick.AddListener(OnContinueClick);
        exitToMenuButton.onClick.AddListener(OnExitToMenuClick);
        exitButton.onClick.AddListener(OnExitClick);
    }

    void InTimeStop()
    {
        _laserTimeStopGlobal = !_laserTimeStopGlobal;
        foreach (var laser in GetComponentsInChildren<Laser>(true))
        {
            laser.isTimeStop = _laserTimeStopGlobal;
            laser.stopTimeStart = Time.time;      
        }
    }

    void OnPause()
    {
        Laser[] lasers = GetComponentsInChildren<Laser>();
        foreach (var laser in lasers)
            laser.isPause = !laser.isPause;

        GetComponentInChildren<Snake>().isPause = !GetComponentInChildren<Snake>().isPause;

        Menu.SetActive(!Menu.activeSelf);
    }

    void OnContinueClick()
    {
        OnPause();
    }

    void OnExitToMenuClick()
    {
        Debug.Log(int.Parse(score.text));
        Debug.Log(Score.record);
        if (int.Parse(score.text) > Score.record)
        {
            Score.record = int.Parse(score.text);
        }
        SceneManager.LoadScene("MainMenu");
    }

    void OnExitClick()
    {
        if (int.Parse(score.text) > Score.record)
        {
            Score.SaveRecord(int.Parse(score.text));
        }
        StartGame.OnExitClick();
    }

    public void DeleteObjectAbroad(GameObject gameObject)
    {
        bool isDeath = gameObject.GetComponent<DeathLaser>() != null;
        bool isSlicer = gameObject.GetComponent<SlicerLaser>() != null;

        Destroy(gameObject);

        if (isDeath)
        {
            GenLaser(deathLaser);
            return;
        }
        if (isSlicer)
        {
            GenLaser(slicerLaser);
        }
    }

    void GenApple()
    {
        float x = Random.Range(topLeft.x + border, bottomRight.x - border);
        float y = Random.Range(bottomRight.y + border, topLeft.y - border);

        NNDecisionMaker.AppleCoords = new Vector2(x, y);

        Instantiate(apple, new Vector2(x, y), Quaternion.identity, transform);
    }

    void GetLaserSpawn(out Vector2 startPos, out Vector2 dir)
    {
        int randdir = Random.Range(0, 4);
        if (randdir == 0)
        {
            dir = new Vector2(1f, 0f);
            startPos = new Vector2(topLeft.x, Random.Range(bottomRight.y + border, topLeft.y - border));
        }
        else if (randdir == 1)
        {
            dir = new Vector2(-1f, 0f);
            startPos = new Vector2(bottomRight.x, Random.Range(bottomRight.y + border, topLeft.y - border));
        }
        else if (randdir == 2)
        {
            dir = new Vector2(0f, 1f);
            startPos = new Vector2(Random.Range(topLeft.x + border, bottomRight.x - border), bottomRight.y);
        }
        else
        {
            dir = new Vector2(0f, -1f);
            startPos = new Vector2(Random.Range(topLeft.x + border, bottomRight.x - border), topLeft.y);
        }
    }

    static Vector2 ComputeLaserLineEnd(Vector2 start, Vector2 dir, Vector2 tl, Vector2 br, float pad = 4f)
    {
        if (dir.x > 0.5f)
            return new Vector2(br.x + pad, start.y);
        if (dir.x < -0.5f)
            return new Vector2(tl.x - pad, start.y);
        if (dir.y > 0.5f)
            return new Vector2(start.x, tl.y + pad);
        return new Vector2(start.x, br.y - pad);
    }

    void GenLaser(GameObject laserPrefab)
    {
        GetLaserSpawn(out Vector2 startPos, out Vector2 dir);
        bool isDeath = laserPrefab.GetComponent<DeathLaser>() != null;
        Color edge = isDeath ? deathTelegraphEdge : slicerTelegraphEdge;
        Color core = isDeath ? deathTelegraphCore : slicerTelegraphCore;
        if (_laserSpawnHost != null && _laserSpawnHost.gameObject.activeInHierarchy)
            _laserSpawnHost.StartCoroutine(SpawnLaserAfterTelegraph(laserPrefab, startPos, dir, edge, core));
        else
            SpawnLaserImmediate(laserPrefab, startPos, dir);
    }

    void SpawnLaserImmediate(GameObject laserPrefab, Vector2 startPos, Vector2 dir)
    {
        var instance = Instantiate(laserPrefab, startPos, Quaternion.identity, transform);
        var las = instance.GetComponent<Laser>();
        if (las != null)
        {
            las.dir = dir;
            las.isTimeStop = _laserTimeStopGlobal;
        }
    }

    IEnumerator SpawnLaserAfterTelegraph(GameObject laserPrefab, Vector2 startPos, Vector2 dir, Color edge, Color core)
    {
        Vector2 endPos = ComputeLaserLineEnd(startPos, dir, topLeft, bottomRight);
        var teleGo = new GameObject("LaserTelegraph");
        teleGo.transform.SetParent(transform, false);
        var lr = teleGo.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.useWorldSpace = true;
        lr.sortingOrder = 8;
        lr.numCapVertices = 4;
        lr.numCornerVertices = 2;

        Material mat = telegraphLineMaterial;
        if (mat == null)
        {
            Shader sh = Shader.Find("Sprites/Default");
            if (sh == null)
                sh = Shader.Find("Unlit/Color");
            if (sh != null)
                mat = new Material(sh);
        }
        if (mat != null)
            lr.material = mat;

        var grad = new Gradient();
        grad.SetKeys(
            new[]
            {
                new GradientColorKey(edge, 0f),
                new GradientColorKey(Color.Lerp(edge, core, 0.5f), 0.5f),
                new GradientColorKey(core, 1f)
            },
            new[]
            {
                new GradientAlphaKey(edge.a, 0f),
                new GradientAlphaKey(Mathf.Lerp(edge.a, core.a, 0.5f), 0.5f),
                new GradientAlphaKey(core.a, 1f)
            });
        lr.colorGradient = grad;

        Snake snake = snakePlayer != null ? snakePlayer.GetComponent<Snake>() : null;

        float acc = 0f;
        float duration = Mathf.Max(0.05f, laserTelegraphDuration);
        while (acc < duration)
        {
            if (snake == null || !snake.isPause)
                acc += Time.deltaTime;
            float u = Mathf.Clamp01(acc / duration);
            lr.SetPosition(0, startPos);
            lr.SetPosition(1, endPos);
            float wMul = telegraphWidthMultiplier != null ? telegraphWidthMultiplier.Evaluate(u) : 1f;
            lr.startWidth = 0.07f * wMul;
            lr.endWidth = 0.2f * wMul;
            yield return null;
        }

        SpawnLaserImmediate(laserPrefab, startPos, dir);
        Destroy(teleGo);
    }

    public void ChangeScore(string text)
    {
        if (int.Parse(score.text) > Score.record)
        {
            Score.record = int.Parse(score.text);
        }
        score.text = text;
    }

    public void ChangeTimer(string text)
    {
        timerText.text = text;
    }

    public void ChangeTimer(Color color)
    {
        timerText.color = color;
    }

    public void GameOver()
    {
        OnPause();
    }
}

/// <summary>Coroutine host at scene root so spawn coroutines work when Main Camera / GameLogic is inactive.</summary>
public sealed class LaserSpawnCoroutineHost : MonoBehaviour { }
