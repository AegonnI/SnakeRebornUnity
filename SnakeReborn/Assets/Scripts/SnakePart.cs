using UnityEngine;

public class SnakePart : MonoBehaviour
{
    public bool isHead;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (isHead && !collider.gameObject.GetComponent<SnakePart>())
        {
            //Destroy(collider.gameObject);
            transform.parent.gameObject.GetComponent<Snake>().OnChildTriggerEnter2D(collider);           
        }
    }
}
