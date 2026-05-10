using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed;
    public Vector2 dir;
    public bool isPause;
    public bool isTimeStop;
    private bool isDying;

    void Start()
    {
        transform.right = dir;
        isPause = false;
        isDying = false;
    }

    void Update()
    {
        transform.position = (Vector2)transform.position + dir * Time.deltaTime * ((isPause || isTimeStop ? 0f : 1f) * speed);
    }

    private void OnBecameInvisible()
    {
        if (!isDying)
        {
            isDying = true;
            transform.parent.gameObject.GetComponent<GameLogic>().DeleteObjectAbroad(gameObject);
        }
    }
}
