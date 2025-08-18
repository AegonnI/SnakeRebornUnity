using UnityEngine;

public class SlicerLaser : Laser
{
    public static bool hasProcessed = true;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<SnakePart>() && hasProcessed)
        {
            hasProcessed = false;
            transform.parent.GetComponentInChildren<Snake>().Slice(gameObject, collider.gameObject);
            //Destroy(gameObject);
        }

        //Debug.Log("Slicer");
    }
}
