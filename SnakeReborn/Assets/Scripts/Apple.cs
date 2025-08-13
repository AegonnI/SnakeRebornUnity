using UnityEngine;

public class Apple : MonoBehaviour
{
    public AppleEffectData effect;

    private void Start()
    {
        effect = GetRandomEffect();

        gameObject.GetComponent<SpriteRenderer>().color = effect.appleColor;
        //Debug.Log(effect.effectName);
        //print(effect.effectName);
    }

    public AppleEffectData GetRandomEffect()
    {
        float randomValue = Random.Range(0f, AppleEffectManager.sumOfWeights);
        float cumulative = 0f;
        foreach (var effect in AppleEffectManager.effects)
        {
            cumulative += effect.weight;
            if (randomValue <= cumulative)
                return effect;
        }
        return new AppleEffectData();
    }
}
