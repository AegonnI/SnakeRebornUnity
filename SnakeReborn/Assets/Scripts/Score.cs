using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static int record = 0;
    public static int score = 0;

    public static event Action<int> OnScoreChanged;

    public TextMeshProUGUI highScoreText;

    void Start()
    {
        Debug.Log(record);
        if (record == 0)
        {
            record = LoadRecord();
        }      
        highScoreText.text = "High Score: " + record.ToString();
    }

    void OnApplicationQuit()
    {
        SaveRecord(record);
    }

    public static void AddScore(int delta)
    {
        score += delta;
        //OnScoreChanged(score);
        Debug.Log(score);
    }

    public static void SetScore(int count)
    {
        score = count;
        //OnScoreChanged(score);
        Debug.Log(score);
    }

    public int LoadRecord()
    {
        return PlayerPrefs.GetInt("HighScore", 0); // 0 по умолчанию
    }

    public static void SaveRecord(int score)
    {
        PlayerPrefs.SetInt("HighScore", score);
        PlayerPrefs.Save(); // обязательно для записи на диск
    }

    public void ChangeRecord(int score)
    {
        record = score;
        SaveRecord(record);
    }
}
