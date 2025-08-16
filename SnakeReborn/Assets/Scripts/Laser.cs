using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed;
    public Vector2 dir;
    public bool isPause;

    void Start()
    {
        transform.right = dir;
        isPause = false;
    }

    void Update()
    {
        transform.position = (Vector2)transform.position + dir * Time.deltaTime * ((isPause ? 0f : 1f) * speed);
    }

    private void OnBecameInvisible()
    {
        transform.parent.gameObject.GetComponent<GameLogic>().DeleteObjectAbroad(gameObject);
    }
}
