using UnityEngine;

public class DeathLaser : Laser
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<SnakePart>())
        {
            transform.parent.GetComponentInChildren<Snake>().Death();
            Destroy(gameObject);
        }
    }
}
