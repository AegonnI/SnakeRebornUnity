using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed;
    public Vector2 dir;
    public bool isPause;
    public bool isTimeStop;
    private bool isDying;

    public float stopTimeStart;
    private float stoppingDurationInSec = 0.5f;
    // Статическое поле для отслеживания закрытия приложения
    private static bool isAppQuitting = false;

    void Start()
    {
        transform.right = dir;
        isPause = false;
        isDying = false;
        var gl = transform.parent != null ? transform.parent.GetComponent<GameLogic>() : null;
        if (gl != null)
            isTimeStop = gl.LaserTimeStopActive;
    }

    void Update()
    {
        transform.position = (Vector2)transform.position + dir * Time.deltaTime * ((isPause ? 0f : 1f) * (isTimeStop ? Mathf.Clamp01(1 - (Time.time - stopTimeStart) / stoppingDurationInSec) : 1f) * speed);
        Physics2D.SyncTransforms();
    }

    void OnApplicationQuit()
    {
        isAppQuitting = true;
    }

    private void OnBecameInvisible()
    {
        // 1. Не выполняем логику, если приложение закрывается
        if (isAppQuitting)
            return;

        // 2. Не выполняем логику, если сцена уже выгружается
        if (gameObject.scene.isLoaded == false)
            return;

        if (!isDying)
        {
            isDying = true;

            var gl = transform.parent.GetComponent<GameLogic>();

            // 3. Проверяем, что GameLogic существует и не уничтожается в данный момент
            if (gl != null && !gl.IsShuttingDown)
            {
                gl.DeleteObjectAbroad(gameObject);
            }
        }
    }
}
