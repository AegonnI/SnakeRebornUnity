using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed;
    public Vector2 dir;

    void Start()
    {
        transform.right = dir;
    }

    void Update()
    {
        transform.position = (Vector2)transform.position + dir * Time.deltaTime * speed;
    }

    private void OnBecameInvisible()
    {
        transform.parent.gameObject.GetComponent<GameLogic>().DeleteObjectAbroad(gameObject);
    }
}
