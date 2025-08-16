using UnityEngine;

public class DeathLaser : Laser
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Death");
    }
}
