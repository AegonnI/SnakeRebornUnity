using UnityEngine;

public class SlicerLaser : Laser
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Slicer");
    }
}
