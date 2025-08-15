using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class GameLogic : MonoBehaviour
{
    public GameObject snakePlayer;
    public GameObject apple;
    public GameObject deathLaser;
    public GameObject slicerLaser;
    public float border;
    public int numOfSlicers;

    private Camera cam;
    private Vector2 topLeft;
    private Vector2 bottomRight;
 
    void Start()
    {
        cam = Camera.main;
        float camHeight = cam.orthographicSize * 2;
        float camWidth = camHeight * cam.aspect;
        topLeft = (Vector2)cam.transform.position + new Vector2(-camWidth / 2, camHeight / 2);
        bottomRight = (Vector2)cam.transform.position + new Vector2(camWidth / 2, -camHeight / 2);

        snakePlayer.GetComponent<Snake>().appleEated += GenApple;

        GenApple();
        GenLaser(deathLaser);
        for (int i = 0; i < numOfSlicers; i++)
        {
            GenLaser(slicerLaser);
        }
    }

    public void DeleteObjectAbroad(GameObject gameObject)
    {
        Destroy(gameObject);
        if (gameObject.GetComponent<DeathLaser>())
        {
            GenLaser(deathLaser);
            return;
        }
        if (gameObject.GetComponent<SlicerLaser>())
        {
            GenLaser(slicerLaser);
            return;
        }
    }

    void GenApple()
    {
        float x, y;

        x = UnityEngine.Random.Range(topLeft.x + border, bottomRight.x - border);
        y = UnityEngine.Random.Range(bottomRight.y + border, topLeft.y - border);

        Instantiate(apple, new Vector2(x, y), Quaternion.identity);
    }

    Vector2 GenDir(GameObject obj)
    {
        Vector2 startPos = new Vector2();

        int randdir = new System.Random().Next(0, 3);
        if (randdir == 0) // слева направо
        {
            obj.GetComponent<Laser>().dir = new Vector2(1f, 0f);
            startPos = new Vector2(topLeft.x, UnityEngine.Random.Range(bottomRight.y + border, topLeft.y - border));
        }
        else if (randdir == 1) // справа налево
        {
            obj.GetComponent<Laser>().dir = new Vector2(-1f, 0f);
            startPos = new Vector2(bottomRight.x, UnityEngine.Random.Range(bottomRight.y + border, topLeft.y - border));
        }
        else if (randdir == 2) // снизу вверх
        {
            obj.GetComponent<Laser>().dir = new Vector2(0f, 1f);
            startPos = new Vector2(UnityEngine.Random.Range(topLeft.x + border, bottomRight.x - border), bottomRight.y);
        }
        else // сверху вниз
        {
            obj.GetComponent<Laser>().dir = new Vector2(0f, -1f);
            startPos = new Vector2(UnityEngine.Random.Range(topLeft.x + border, bottomRight.x - border), topLeft.y);
        }

        return startPos;
    }

    void GenLaser(GameObject laser)
    {
        Instantiate(laser, GenDir(laser), Quaternion.identity, transform);
    }
}
