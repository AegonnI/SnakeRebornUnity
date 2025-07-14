using System;
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
        GenDeathLaser();
        for (int i = 0; i < numOfSlicers; i++)
        {
            GenSlicerLaser();
        }
    }

    public void DeleteObjectAbroad(GameObject gameObject)
    {
        if (gameObject.GetComponent<DeathLaser>())
        {
            //Debug.Log("I DeleteObjectAbroad");
            Destroy(gameObject);
            GenDeathLaser();
        }
        if (gameObject.GetComponent<SlicerLaser>())
        {
            Destroy(gameObject);
            GenSlicerLaser();
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

    void GenDeathLaser()
    {
        //Debug.Log("I gen a new deathlaser");
        Instantiate(deathLaser, GenDir(deathLaser), Quaternion.identity, transform);
    }

    void GenSlicerLaser()
    {
        Instantiate(slicerLaser, GenDir(slicerLaser), Quaternion.identity, transform);
    }
}
