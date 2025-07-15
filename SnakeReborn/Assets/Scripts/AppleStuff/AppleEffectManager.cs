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
        effects[^1].speedMultiplier = 2.0f;
        effects[^1].durationInSec = 4.0f;
        effects[^1].weight = 0.05f;

        effects.Add(new AppleEffectData());
        effects[^1].effectName = "Invincibility";
        effects[^1].givesInvincibility = true;
        effects[^1].durationInSec = 4.0f;
        effects[^1].weight = 0.025f;

        foreach(AppleEffectData data in effects)
        {
            sumOfWeights += data.weight;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
