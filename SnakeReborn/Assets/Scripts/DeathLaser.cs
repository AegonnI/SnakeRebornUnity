using UnityEngine;

public class DeathLaser : Laser
{
    //private void OnTriggerEnter2D(Collider2D collider)
    //{
    //    if (collider.gameObject.GetComponent<SnakePart>())
    //    {
    //        Vector2 p = collider.ClosestPoint(transform.position);
    //        var sr = GetComponent<SpriteRenderer>();
    //        ImpactSparks.BurstAt(p, sr != null ? sr.color : new Color(0.2f, 0.9f, 0.5f), transform.parent);
    //        transform.parent.GetComponentInChildren<Snake>().Death();
    //        Destroy(gameObject);
    //    }
    //}

    public bool TryHit(SnakePart part, Snake snake)
    {
        if (part == null || snake == null)
            return false;

        Vector2 p = part.transform.position;
        var sr = GetComponent<SpriteRenderer>();
        Color sparkColor = sr != null ? sr.color : new Color(0.2f, 0.9f, 0.5f);

        snake.Death();
        Destroy(gameObject);

        ImpactSparks.BurstAt(p, sparkColor, transform.parent);

        return true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(
            $"[KILL LASER] TRIGGER! collider={collider.name}, " +
            $"layer={LayerMask.LayerToName(collider.gameObject.layer)}"
        );

        SnakePart part = collider.GetComponent<SnakePart>();

        Debug.Log(
            $"[KILL LASER] SnakePart = {(part != null ? part.name : "NULL")}"
        );

        if (part != null)
        {
            Snake snake = part.GetComponentInParent<Snake>();

            Debug.Log(
                $"[KILL LASER] Snake = {(snake != null ? snake.name : "NULL")}"
            );

            if (snake != null)
            {
                Debug.Log("[KILL LASER] Calling TryHit()");
                TryHit(part, snake);
            }
        }
    }
}
