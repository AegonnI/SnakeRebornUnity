using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class GameLogic : MonoBehaviour
{
    public GameObject snakePlayer;
    public GameObject apple;
    public float border;

    private Camera cam;
    private Vector2 topLeft;
    private Vector2 bottomRight;
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        float camHeight = cam.orthographicSize * 2;
        float camWidth = camHeight * cam.aspect;
        topLeft = (Vector2)cam.transform.position + new Vector2(-camWidth / 2, camHeight / 2);
        bottomRight = (Vector2)cam.transform.position + new Vector2(camWidth / 2, -camHeight / 2);

        snakePlayer.GetComponent<Snake>().appleEated += GenApple;

        GenApple();
    }

    void GenApple()
    {
        float x, y;

        x = UnityEngine.Random.Range(topLeft.x + border, bottomRight.x - border);
        y = UnityEngine.Random.Range(bottomRight.y + border, topLeft.y - border);

        Instantiate(apple, new Vector2(x, y), Quaternion.identity);
    }
}
