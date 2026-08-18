using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour
{
    public GameObject snakePart;
    public float speed;
    public float fractionOfDistancePerSecond;
    public event Action appleEated;
    public event Action timeStoped;
    public bool isPause;

    private float speedFactor;
    private List<GameObject> snakeParts;
    private AppleEffectData effectOnSnake;

    private float timer;

    public int partsPerApple = 5;

    public AudioClip redAppleSound;
    public AudioClip purpleAppleSound;
    public AudioClip goldenAppleSound;
    public AudioClip cyanAppleSound;
    public AudioClip deathLaserSound;
    public AudioClip sliceLaserSound;

    private GameLogic _gameLogic;
    private Camera _cam;
    private AudioSource audioSource;

    void Start()
    {
        effectOnSnake = new AppleEffectData();
        audioSource = GetComponent<AudioSource>();

        appleEated += GrowUp;

        speedFactor = 70f / speed;

        _gameLogic = transform.parent != null ? transform.parent.GetComponent<GameLogic>() : null;
        _cam = Camera.main;

        snakeParts = new List<GameObject>();
        var head = Instantiate(snakePart, new Vector2(0f, 0f), Quaternion.identity, transform);
        head.GetComponent<SnakePart>().isHead = true;
        head.GetComponent<SpriteRenderer>().color = Color.white;
        snakeParts.Add(head);

        isPause = false;
    }

    void Update()
    {
        if (!isPause)
        {
            SnakeMove();
            CheckCollisions();
            CheckAndExpireEffect();
        }
    }

    public void Eat(Apple apple)
    {
        appleEated();
        if (apple.effect.effectName != new AppleEffectData().effectName)
            GetEffect(apple.effect);
        switch (apple.effect.effectName)
        {
            case "Effect of speed":
                audioSource.PlayOneShot(purpleAppleSound);
                break;
            case "Invincibility":
                audioSource.PlayOneShot(goldenAppleSound);
                break;
            case "TimeStop":
                audioSource.PlayOneShot(cyanAppleSound);
                break;
            default:
                audioSource.PlayOneShot(redAppleSound);
                break;
        }
    }

    public void Death()
    {
        audioSource.PlayOneShot(deathLaserSound);

        if (!effectOnSnake.givesInvincibility)
        {
            if (GameSattings.returnToMenuAfterDeath)
            {
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                for (int i = snakeParts.Count - 1; i > 0; i--)
                {
                    Destroy(snakeParts[i]);
                    snakeParts.RemoveAt(i);
                }
                snakeParts[0].transform.position = new Vector2();
                if (_gameLogic != null)
                    _gameLogic.ChangeScore(snakeParts.Count.ToString());
                Score.SetScore(0);
            }
        }
        else
            GrowUp();
    }

    public void Slice(GameObject slicer, GameObject snakePartObj)
    {
        audioSource.PlayOneShot(sliceLaserSound);

        var slicerSr = slicer != null ? slicer.GetComponent<SpriteRenderer>() : null;
        Color sparkCol = slicerSr != null ? slicerSr.color : new Color(1f, 0.4f, 0.55f);
        Vector2 sparkPos = snakePartObj.transform.position;
        if (slicer != null && slicer.TryGetComponent(out Collider2D slab))
            sparkPos = slab.ClosestPoint(snakePartObj.transform.position);
        ImpactSparks.BurstAt(sparkPos, sparkCol, _gameLogic != null ? _gameLogic.transform : null);

        if (!effectOnSnake.givesInvincibility)
        {
            int indexForSlice = snakeParts.IndexOf(snakePartObj);

            if (indexForSlice > 0)
            {
                var detached = new List<GameObject>();
                for (int i = snakeParts.Count - 1; i > indexForSlice; i--)
                {
                    detached.Add(snakeParts[i]);
                    snakeParts.RemoveAt(i);
                }

                Transform vfxRoot = _gameLogic != null ? _gameLogic.transform : transform.parent;
                Vector2 headPos = snakeParts[0].transform.position;
                Vector2 outward = ((Vector2)snakePartObj.transform.position - headPos);
                if (outward.sqrMagnitude < 0.01f)
                    outward = UnityEngine.Random.insideUnitCircle.normalized;
                outward.Normalize();

                for (int i = 0; i < detached.Count; i++)
                {
                    var go = detached[i];
                    go.transform.SetParent(vfxRoot, true);
                    foreach (var col in go.GetComponents<Collider2D>())
                        col.enabled = false;
                    var fade = go.AddComponent<DetachedSegmentFade>();
                    fade.Begin(outward * (1.4f + i * 0.12f) + Vector2.Perpendicular(outward) * UnityEngine.Random.Range(-0.6f, 0.6f));
                }

                if (_gameLogic != null)
                    _gameLogic.ChangeScore(snakeParts.Count.ToString());
                Score.SetScore(snakeParts.Count);
            }
        }
        else
        {
            Destroy(slicer);
            GrowUp();
        }
        SlicerLaser.hasProcessed = true;
    }

    public void GrowUp()
    {
        for (int i = 0; i < partsPerApple; i++)
        {
            var part = Instantiate(snakePart, snakeParts[^1].transform.position, Quaternion.identity, transform);
            part.GetComponent<SnakePart>().isHead = false;
            part.GetComponent<SpriteRenderer>().color = effectOnSnake.snakeColor;
            snakeParts.Add(part);
        }
        if (_gameLogic != null)
            _gameLogic.ChangeScore(snakeParts.Count.ToString());
        Score.AddScore(partsPerApple);
    }

    private void GetEffect(AppleEffectData effect)
    {
        if (effectOnSnake.effectName == "TimeStop" ^ effect.effectName == "TimeStop")
            timeStoped();

        effectOnSnake = effect; 

        timer = Time.time;

        Color color = effectOnSnake.snakeColor;
        for (int i = 0; i < snakeParts.Count; i++)
            snakeParts[i].gameObject.GetComponent<SpriteRenderer>().color = color;
        if (_gameLogic != null)
            _gameLogic.ChangeTimer(color);
    }

    private void SnakeMove()
    {
        Vector2 mousePosition;

        if (GameSattings.useNNPlayer)
            mousePosition = NNDecisionMaker.MakeDecision();
        else
        {
            if (_cam == null)
                _cam = Camera.main;
            mousePosition = _cam != null ? _cam.ScreenToWorldPoint(Input.mousePosition) : Vector2.zero;
        }

        if (snakeParts.Count > 1)
        {
            snakeParts.Insert(1, snakeParts[^1]);
            snakeParts.RemoveAt(snakeParts.Count - 1);
            snakeParts[1].transform.position = snakeParts[0].transform.position;
        }

        float F = fractionOfDistancePerSecond + (1f - fractionOfDistancePerSecond) * (effectOnSnake.speedMultiplier - 1) / effectOnSnake.speedMultiplier;

        if (F <= 0f) return;

        if (F >= 1f)
        {
            snakeParts[0].transform.position = mousePosition;
            Physics2D.SyncTransforms();
            return;
        }

        float t = 1f - Mathf.Pow(1f - F, Time.deltaTime);
        snakeParts[0].transform.position = (Vector2)((1 - t) * snakeParts[0].transform.position) + t * mousePosition;

        Physics2D.SyncTransforms();
    }

    const float PartHitRadius = 0.1f;

    void CheckCollisions()
    {
        foreach (var partGo in snakeParts)
        {
            if (partGo == null || !partGo.TryGetComponent(out SnakePart part))
                continue;

            Vector2 pos = partGo.transform.position;

            if (part.isHead)
            {
                foreach (var apple in FindObjectsByType<Apple>(FindObjectsSortMode.None))
                {
                    if (apple == null) continue;
                    if (Vector2.Distance(pos, apple.transform.position) <= PartHitRadius * 2f)
                    {
                        apple.TryCollect(part, this);
                        return;
                    }
                }
            }

            foreach (var laser in FindObjectsByType<DeathLaser>(FindObjectsSortMode.None))
            {
                if (laser != null && OverlapsTransform(laser.transform, pos, PartHitRadius))
                {
                    laser.TryHit(part, this);
                    return;
                }
            }

            foreach (var laser in FindObjectsByType<SlicerLaser>(FindObjectsSortMode.None))
            {
                if (laser != null && OverlapsTransform(laser.transform, pos, PartHitRadius))
                {
                    laser.TryHit(part, this);
                    return;
                }
            }
        }
    }

    static bool OverlapsTransform(Transform target, Vector2 worldPoint, float padding)
    {
        Vector2 local = target.InverseTransformPoint(worldPoint);
        Vector2 half = (Vector2)target.lossyScale * 0.5f + Vector2.one * padding;
        return Mathf.Abs(local.x) <= half.x && Mathf.Abs(local.y) <= half.y;
    }

    private void CheckAndExpireEffect()
    {
        if (effectOnSnake.effectName != new AppleEffectData().effectName)
        {
            if (Time.time >= timer + effectOnSnake.durationInSec)
            {
                GetEffect(new AppleEffectData());
                if (_gameLogic != null)
                    _gameLogic.ChangeTimer("");
                return;
            }
            if (_gameLogic != null)
                _gameLogic.ChangeTimer((timer + effectOnSnake.durationInSec - Time.time).ToString("F2"));

            float a = (float)((Time.time - timer) / effectOnSnake.durationInSec);

            Color color = new Color(1, 1, 1) * a + effectOnSnake.snakeColor * (1 - a);
            for (int i = 0; i < snakeParts.Count; i++)
                snakeParts[i].gameObject.GetComponent<SpriteRenderer>().color = color;
            if (_gameLogic != null)
                _gameLogic.ChangeTimer(color);
        }
    }
}
