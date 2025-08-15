using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class Snake : MonoBehaviour
{
    public GameObject snakePart;
    public float speed;
    public float fractionOfDistancePerSecond;
    public event Action appleEated;

    public TextMeshProUGUI score;
    public TextMeshProUGUI timerText;

    private float speedFactor;
    private List<GameObject> snakeParts;
    private AppleEffectData effectOnSnake;

    private float timer;


    void Start()
    {
        effectOnSnake = new AppleEffectData();
        
        appleEated += GrowUp;
        
        speedFactor = 70f / speed;
        
        snakeParts = new List<GameObject>();
        snakePart.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        snakePart.GetComponent<SnakePart>().isHead = true;
        snakeParts.Add(Instantiate(snakePart, new Vector2(0f, 0f), Quaternion.identity, transform));
        snakePart.GetComponent<SnakePart>().isHead = false;
        
    }

    void Update()
    {
        SnakeMove();

        CheckAndExpireEffect();
    }

    public void OnChildTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Apple>() != null)
        {
            appleEated();
            //if (effectOnSnake != collider.gameObject.GetComponent<Apple>().effect)
            if (collider.gameObject.GetComponent<Apple>().effect.effectName != new AppleEffectData().effectName)
            {
                GetEffect(collider.gameObject.GetComponent<Apple>().effect);
            }         
            Destroy(collider.gameObject);
        }
    }
    public void OnChildTriggerEnter2D(Collider2D collider, GameObject snakePartObj)
    {
        if (collider.gameObject.GetComponent<DeathLaser>())
        {
            if (!effectOnSnake.givesInvincibility)
            {
                for (int i = snakeParts.Count - 1; i > 0; i--)
                {
                    Destroy(snakeParts[i]);
                    snakeParts.RemoveAt(i);
                }
                snakeParts[0].transform.position = new Vector2();
                score.text = snakeParts.Count.ToString();
            }
            else
            {
                GrowUp();
            }
            Destroy(collider.gameObject);
        }
        if (collider.gameObject.GetComponent<SlicerLaser>())
        {
            if (!effectOnSnake.givesInvincibility)
            {
                int indexForSlice = snakeParts.IndexOf(snakePartObj);

                if (indexForSlice > 0)
                {
                    for (int i = snakeParts.Count - 1; i > indexForSlice; i--)
                    {
                        Destroy(snakeParts[i]);
                        snakeParts.RemoveAt(i);
                    }
                    score.text = snakeParts.Count.ToString();
                }
            }
            else
            {
                Destroy(collider.gameObject);
                GrowUp();
            }
        }
        SnakePart.hasProcessed = true;
    }

    public void GrowUp()
    {
        snakePart.gameObject.GetComponent<SpriteRenderer>().color = effectOnSnake.snakeColor;
        for (int i = 0; i < 5; i++) 
        {
            snakeParts.Add(Instantiate(snakePart, snakeParts[^1].transform.position, Quaternion.identity, transform));
        }
        score.text = snakeParts.Count.ToString();
    }

    private void GetEffect(AppleEffectData effect)
    {
        effectOnSnake = effect;

        timer = Time.time;

        Color color = effectOnSnake.snakeColor;
        for (int i = 0; i < snakeParts.Count; i++)
        {
            snakeParts[i].gameObject.GetComponent<SpriteRenderer>().color = color;
        }

        timerText.color = color;
    }

    private void SnakeMove()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (snakeParts.Count > 1)
        {
            snakeParts.Insert(1, snakeParts[^1]);
            snakeParts.RemoveAt(snakeParts.Count - 1);
            snakeParts[1].transform.position = snakeParts[0].transform.position;
        }

        float F = fractionOfDistancePerSecond + (1f - fractionOfDistancePerSecond) * (effectOnSnake.speedMultiplier - 1) / effectOnSnake.speedMultiplier;

        if (F <= 0f) return;

        if (F >= 1f)
        {
            snakeParts[0].transform.position = mousePosition;
            return;
        }

        float t = 1f - Mathf.Pow(1f - F, Time.deltaTime);
        snakeParts[0].transform.position = (Vector2)((1 - t) * snakeParts[0].transform.position) + t * mousePosition;

        //float newSpeedFactor = speedFactor / effectOnSnake.speedMultiplier;
        //snakeParts[0].transform.position = (mousePosition + (newSpeedFactor - 1f) * (Vector2)snakeParts[0].transform.position) / newSpeedFactor;

        //float fractionPerSecond = newSpeedFactor;
        //float perFrame = 1f - Mathf.Pow(1f - fractionPerSecond, Time.deltaTime);
        //snakeParts[0].transform.position = Vector2.Lerp(snakeParts[0].transform.position, mousePosition, perFrame);

        //snakeParts[0].transform.position = (1-Time.deltaTime*)
    }

    private void CheckAndExpireEffect()
    {
        if (effectOnSnake.effectName != new AppleEffectData().effectName)
        {
            if (Time.time >= timer + effectOnSnake.durationInSec)
            {
                GetEffect(new AppleEffectData());
                timerText.text = "";
                return;
            }

            timerText.text = (timer + effectOnSnake.durationInSec - Time.time).ToString();
        }
    }
}
