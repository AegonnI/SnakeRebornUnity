using UnityEngine;

/// <summary>
/// Sparse field of small dots that twinkle (or a soft grid). Place on an empty object behind gameplay; set sorting layer to Background if you use one.
/// </summary>
public class TwinklingBackground : MonoBehaviour
{
    [SerializeField] int starCount = 90;
    [SerializeField] Vector2 extraMargin = new Vector2(1.2f, 1.2f);
    [SerializeField] Color tint = new Color(0.35f, 0.55f, 0.95f, 0.55f);
    [SerializeField] float twinkleSpeed = 1.35f;
    [SerializeField] float minAlpha = 0.08f;
    [SerializeField] float maxAlpha = 0.55f;
    [SerializeField] int sortingOrder = -50;

    [Header("Optional grid")]
    [SerializeField] bool drawSoftGrid = true;
    [SerializeField] Color gridColor = new Color(1f, 1f, 1f, 0.04f);
    [SerializeField] float gridStep = 0.65f;
    [SerializeField] float gridTwinkleAmp = 0.03f;

    Sprite _pixel;
    Transform[] _stars;
    float[] _phases;
    float[] _speeds;
    LineRenderer _gridLines;

    void Awake()
    {
        _pixel = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 4, 4), new Vector2(0.5f, 0.5f), 16f);

        var cam = Camera.main;
        if (cam == null)
            return;

        float h = cam.orthographicSize * 2f + extraMargin.y;
        float w = h * cam.aspect + extraMargin.x;
        Vector3 c = cam.transform.position;
        var min = new Vector2(c.x - w * 0.5f, c.y - h * 0.5f);
        var max = new Vector2(c.x + w * 0.5f, c.y + h * 0.5f);

        _stars = new Transform[starCount];
        _phases = new float[starCount];
        _speeds = new float[starCount];

        for (int i = 0; i < starCount; i++)
        {
            var go = new GameObject("Star");
            go.transform.SetParent(transform, false);
            float x = Random.Range(min.x, max.x);
            float y = Random.Range(min.y, max.y);
            go.transform.position = new Vector3(x, y, 0f);
            float sc = Random.Range(0.04f, 0.12f);
            go.transform.localScale = new Vector3(sc, sc, 1f);

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = _pixel;
            sr.color = tint;
            sr.sortingOrder = sortingOrder;

            _stars[i] = go.transform;
            _phases[i] = Random.Range(0f, Mathf.PI * 2f);
            _speeds[i] = Random.Range(0.7f, 1.6f);
        }

        if (drawSoftGrid)
            BuildGrid(min, max);
    }

    void BuildGrid(Vector2 min, Vector2 max)
    {
        var go = new GameObject("TwinkleGrid");
        go.transform.SetParent(transform, false);
        go.transform.position = new Vector3((min.x + max.x) * 0.5f, (min.y + max.y) * 0.5f, 0f);

        _gridLines = go.AddComponent<LineRenderer>();
        _gridLines.loop = false;
        _gridLines.useWorldSpace = true;
        _gridLines.sortingOrder = sortingOrder - 1;
        _gridLines.material = new Material(Shader.Find("Sprites/Default"));
        _gridLines.startColor = gridColor;
        _gridLines.endColor = gridColor;
        _gridLines.startWidth = 0.02f;
        _gridLines.endWidth = 0.02f;
        _gridLines.textureMode = LineTextureMode.Tile;

        var pts = new System.Collections.Generic.List<Vector3>();
        for (float x = min.x; x <= max.x; x += gridStep)
        {
            pts.Add(new Vector3(x, min.y, 0f));
            pts.Add(new Vector3(x, max.y, 0f));
        }
        for (float y = min.y; y <= max.y; y += gridStep)
        {
            pts.Add(new Vector3(min.x, y, 0f));
            pts.Add(new Vector3(max.x, y, 0f));
        }

        _gridLines.positionCount = pts.Count;
        for (int i = 0; i < pts.Count; i++)
            _gridLines.SetPosition(i, pts[i]);
    }

    void Update()
    {
        float t = Time.time * twinkleSpeed;
        if (_stars == null)
            return;

        for (int i = 0; i < _stars.Length; i++)
        {
            var sr = _stars[i].GetComponent<SpriteRenderer>();
            if (sr == null)
                continue;
            float w = (Mathf.Sin(t * _speeds[i] + _phases[i]) + 1f) * 0.5f;
            float a = Mathf.Lerp(minAlpha, maxAlpha, w);
            Color c = tint;
            c.a = a * tint.a;
            sr.color = c;
        }

        if (_gridLines != null && drawSoftGrid)
        {
            float g = gridColor.a + Mathf.Sin(t * 0.8f) * gridTwinkleAmp;
            g = Mathf.Clamp01(g);
            var c = gridColor;
            c.a = g;
            _gridLines.startColor = c;
            _gridLines.endColor = c;
        }
    }
}
