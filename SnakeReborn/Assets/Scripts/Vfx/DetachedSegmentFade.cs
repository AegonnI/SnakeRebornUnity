using UnityEngine;

/// <summary>
/// Detached snake segment: drifts slightly and fades out, then self-destructs.
/// </summary>
public class DetachedSegmentFade : MonoBehaviour
{
    [SerializeField] float lifetime = 0.9f;

    Vector2 _velocity;
    float _angularSpeed;
    SpriteRenderer _sr;
    float _t;
    Vector3 _baseScale;

    public void Begin(Vector2 extraImpulse)
    {
        _sr = GetComponent<SpriteRenderer>();
        _baseScale = transform.localScale;
        _velocity = extraImpulse + Random.insideUnitCircle * 1.1f;
        _angularSpeed = Random.Range(-720f, 720f);
    }

    void Update()
    {
        float dt = Time.deltaTime;
        _t += dt;
        float k = Mathf.Clamp01(_t / lifetime);

        transform.position += (Vector3)(_velocity * dt);
        _velocity *= 0.94f;
        transform.Rotate(0f, 0f, _angularSpeed * dt);

        if (_sr != null)
        {
            Color c = _sr.color;
            c.a = Mathf.Lerp(1f, 0f, k * k);
            _sr.color = c;
        }

        float s = Mathf.Lerp(1f, 0.35f, k);
        transform.localScale = _baseScale * s;

        if (_t >= lifetime)
            Destroy(gameObject);
    }
}
