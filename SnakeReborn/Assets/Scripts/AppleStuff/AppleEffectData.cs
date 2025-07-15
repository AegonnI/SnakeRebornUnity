using UnityEngine;

public class AppleEffectData
{
    public string effectName;       // для отладки/UI
    public bool givesInvincibility;
    public float speedMultiplier;
    public float durationInSec;

    [Min(0)]
    public float weight;

    public AppleEffectData() 
    {
        effectName = "Without Effects";
        givesInvincibility = false;
        speedMultiplier = 1f;
        durationInSec = -1f;
        weight = 1f;
    }
}
