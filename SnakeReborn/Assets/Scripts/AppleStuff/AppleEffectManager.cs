using System.Collections.Generic;
using UnityEngine;

public class AppleEffectManager : MonoBehaviour
{
    public static List<AppleEffectData> effects;
    public static float sumOfWeights;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Debug.Log("AppleEffectManager started");

        effects = new List<AppleEffectData>();

        effects.Add(new AppleEffectData());

        effects.Add(new AppleEffectData());
        effects[^1].effectName = "Effect of speed";
        effects[^1].speedMultiplier = 2.5f;
        effects[^1].durationInSec = 4.0f;
        effects[^1].weight = 0.3f;
        effects[^1].color = Color.purple;

        effects.Add(new AppleEffectData());
        effects[^1].effectName = "Invincibility";
        effects[^1].givesInvincibility = true;
        effects[^1].durationInSec = 4.0f;
        effects[^1].weight = 0.15f;
        effects[^1].color = Color.gold;

        foreach (AppleEffectData data in effects)
        {
            sumOfWeights += data.weight;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
