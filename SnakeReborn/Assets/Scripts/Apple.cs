using UnityEngine;

public class Apple : MonoBehaviour
{
    public AppleEffectData effect;

    private void Start()
    {
        float chance = Random.Range(0f, AppleEffectManager.sumOfWeights);

        Debug.Log(AppleEffectManager.effects == null);

        float cumWieghts = 0f;
        foreach (AppleEffectData eff in AppleEffectManager.effects)
        {
            if (eff == null)
            {
                Debug.LogWarning("Найден null-эффект в списке!");
                continue;
            }

            if (eff.weight + cumWieghts < chance) 
            { 
                effect = eff;
                break;
            }
            cumWieghts += eff.weight;
        }
        print(effect.effectName);
    }
}
