using UnityEngine;

public class Apple : MonoBehaviour
{
    public AppleEffectData effect;

    private void Start()
    {
        effect = GetRandomEffect();

        gameObject.GetComponent<SpriteRenderer>().color = effect.appleColor;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<SnakePart>() && collider.gameObject.GetComponent<SnakePart>().isHead)
        {
            transform.parent.GetComponentInChildren<Snake>().Eat(this);
            Destroy(gameObject);
        }
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
