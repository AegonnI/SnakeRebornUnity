using UnityEngine;

public class Score : MonoBehaviour
{
    public static int record;
    public static int score;

    void Start()
    {
        record = LoadRecord();
    }

    void OnApplicationQuit()
    {
        SaveRecord(record);
    }

    public static void AddScore(int count)
    {
        score += count;
    }

    public int LoadRecord()
    {
        return PlayerPrefs.GetInt("HighScore", 0); // 0 по умолчанию
    }

    public void SaveRecord(int score)
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
