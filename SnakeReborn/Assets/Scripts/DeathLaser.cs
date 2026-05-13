using UnityEngine;

public class DeathLaser : Laser
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<SnakePart>())
        {
            Vector2 p = collider.ClosestPoint(transform.position);
            var sr = GetComponent<SpriteRenderer>();
            ImpactSparks.BurstAt(p, sr != null ? sr.color : new Color(0.2f, 0.9f, 0.5f), transform.parent);
            transform.parent.GetComponentInChildren<Snake>().Death();
            Destroy(gameObject);
        }
    }
}
