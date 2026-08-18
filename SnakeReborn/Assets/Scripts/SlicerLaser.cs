using UnityEngine;

public class SlicerLaser : Laser
{
    public static bool hasProcessed = true;

    //private void OnTriggerEnter2D(Collider2D collider)
    //{
    //    if (collider.gameObject.GetComponent<SnakePart>() && hasProcessed)
    //    {
    //        hasProcessed = false;
    //        transform.parent.GetComponentInChildren<Snake>().Slice(gameObject, collider.gameObject);
    //        //Destroy(gameObject);
    //    }

    //    //Debug.Log("Slicer");
    //}

    public bool TryHit(SnakePart part, Snake snake)
    {
        if (part == null || snake == null || !hasProcessed)
            return false;

        snake.Slice(gameObject, part.gameObject);
        return true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(
            $"[CUT LASER] TRIGGER! collider={collider.name}, " +
            $"layer={LayerMask.LayerToName(collider.gameObject.layer)}"
        );

        SnakePart part = collider.GetComponent<SnakePart>();

        Debug.Log(
            $"[CUT LASER] SnakePart = {(part != null ? part.name : "NULL")}"
        );

        if (part != null)
        {
            Snake snake = part.GetComponentInParent<Snake>();

            Debug.Log(
                $"[CUT LASER] Snake = {(snake != null ? snake.name : "NULL")}"
            );

            if (snake != null)
            {
                Debug.Log("[CUT LASER] Calling TryHit()");
                TryHit(part, snake);
            }
        }
    }
}
