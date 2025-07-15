using UnityEngine;

public class SnakePart : MonoBehaviour
{
    public static bool hasProcessed = true;
    
    public bool isHead;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //if (!collider.gameObject.GetComponent<SnakePart>())
        //{
        //    if (isHead && collider.gameObject.GetComponent<Apple>())
        //    {
        //        transform.parent.gameObject.GetComponent<Snake>().OnChildTriggerEnter2D(collider);
        //    }
        //}


        //if (isHead && !collider.gameObject.GetComponent<SnakePart>() && !collider.gameObject.GetComponent<SlicerLaser>())
        //{
        //    //Destroy(collider.gameObject);
        //    transform.parent.gameObject.GetComponent<Snake>().OnChildTriggerEnter2D(collider);           
        //}
        //else if (collider.gameObject.GetComponent<SlicerLaser>())
        //{
        //    //Destroy(collider.gameObject);
        //    if (hasProcessed)
        //    {
        //        hasProcessed = false;
        //        transform.parent.gameObject.GetComponent<Snake>().OnChildTriggerEnter2D(collider, gameObject);
        //    }

        //}
        //else if (collider.gameObject.GetComponent<SlicerLaser>())
        //{
        //    //Destroy(collider.gameObject);
        //    if (hasProcessed)
        //    {
        //        hasProcessed = false;
        //        transform.parent.gameObject.GetComponent<Snake>().OnChildTriggerEnter2D(collider, gameObject);
        //    }

        //}
        if (!collider.gameObject.GetComponent<SnakePart>())
        {
            //Debug.Log(isHead);
            if (isHead && collider.gameObject.GetComponent<Apple>())
            {
                transform.parent.gameObject.GetComponent<Snake>().OnChildTriggerEnter2D(collider);
            }

            else if (hasProcessed)
            {
                hasProcessed = false;
                transform.parent.gameObject.GetComponent<Snake>().OnChildTriggerEnter2D(collider, gameObject);
            }
        }
    }
}
