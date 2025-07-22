using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Linq;
using TMPro;

public class Snake : MonoBehaviour
{
    public GameObject snakePart;
    public float speed;
    public event Action appleEated;

    public TextMeshProUGUI score;

    private float speedFactor;
    private Vector2 _mousePosition;
    private List<GameObject> snakeParts;
    private AppleEffectData effectOnSnake;


    void Start()
    {
        effectOnSnake = new AppleEffectData();
        
        appleEated += GrowUp;
        
        speedFactor = 70f / speed;
        
        _mousePosition = new Vector2();
        snakeParts = new List<GameObject>();
        snakePart.GetComponent<SnakePart>().isHead = true;
        snakeParts.Add(Instantiate(snakePart, _mousePosition, Quaternion.identity, transform));
        snakePart.GetComponent<SnakePart>().isHead = false;
    }

    void Update()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (snakeParts.Count > 1)
        {
            snakeParts.Insert(1, snakeParts[^1]);
            snakeParts.RemoveAt(snakeParts.Count - 1); 
            snakeParts[1].transform.position = snakeParts[0].transform.position;
        }

        float newSpeedFactor = speedFactor / effectOnSnake.speedMultiplier;
        snakeParts[0].transform.position = (_mousePosition + (newSpeedFactor - 1f) * (Vector2)snakeParts[0].transform.position) / newSpeedFactor;
    }

    public void OnChildTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Apple>() != null)
        {
            appleEated();
            effectOnSnake = collider.gameObject.GetComponent<Apple>().effect;
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
        for (int i = 0; i < 5; i++) 
        {
            snakeParts.Add(Instantiate(snakePart, snakeParts[^1].transform.position, Quaternion.identity, transform));
        }
        score.text = snakeParts.Count.ToString();
}
}
