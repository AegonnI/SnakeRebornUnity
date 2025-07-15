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


    void Start()
    {
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
        for (int i = snakeParts.Count-1; i > 0; i--)
        {
            snakeParts[i].transform.position = snakeParts[i - 1].transform.position;
        }
        snakeParts[0].transform.position = (_mousePosition + (speedFactor - 1f) * (Vector2)snakeParts[0].transform.position) / speedFactor;
    }

    public void OnChildTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Apple>() != null)
        {
            appleEated();
            Destroy(collider.gameObject);
        }
        //if (collider.gameObject.GetComponent<DeathLaser>() != null)
        //{
        //    //Debug.Log("I took deathlaser");
        //    for (int i = snakeParts.Count - 1; i > 0; i--)
        //    {
        //        Destroy(snakeParts[i]);
        //        snakeParts.RemoveAt(i);
        //    }
        //    snakeParts[0].transform.position = new Vector2();

        //    Destroy(collider.gameObject);
        //}
    }
    public void OnChildTriggerEnter2D(Collider2D collider, GameObject snakePartObj)
    {
        //if (collider.gameObject.GetComponent<Apple>())
        //{
        //    appleEated();
        //    Destroy(collider.gameObject);
        //    SnakePart.hasProcessed = true;
        //}
        if (collider.gameObject.GetComponent<DeathLaser>())
        {
            //Debug.Log("I took deathlaser");
            for (int i = snakeParts.Count - 1; i > 0; i--)
            {
                Destroy(snakeParts[i]);
                snakeParts.RemoveAt(i);
            }
            snakeParts[0].transform.position = new Vector2();
            score.text = snakeParts.Count.ToString();

            Destroy(collider.gameObject);
            //SnakePart.hasProcessed = true;
        }
        if (collider.gameObject.GetComponent<SlicerLaser>())
        {
            //Debug.Log("chik");

            int indexForSlice = snakeParts.IndexOf(snakePartObj);
            Debug.Log(indexForSlice);

            if (indexForSlice > 0)
            {
                for (int i = snakeParts.Count - 1; i > indexForSlice; i--)
                {
                    Destroy(snakeParts[i]);
                    snakeParts.RemoveAt(i);
                }
                score.text = snakeParts.Count.ToString();
            }
            //Destroy(collider.gameObject);
            //SnakePart.hasProcessed = true;
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
