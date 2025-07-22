using UnityEngine;

public class AppleEffectData
{
    public string effectName;       // для отладки/UI
    public bool givesInvincibility;
    public float speedMultiplier;
    public float durationInSec;
    //public Vector3 color;
    public Color color;

    [Min(0)]
    public float weight;

    public AppleEffectData() 
    {
        effectName = "Without Effects";
        givesInvincibility = false;
        speedMultiplier = 1f;
        durationInSec = -1f;
        weight = 1f;
        //color = new Vector3(0.8962264f, 0.1986917f, 0.1986917f);
        color = new Color(0.8962264f, 0.1986917f, 0.1986917f);
    }
}
