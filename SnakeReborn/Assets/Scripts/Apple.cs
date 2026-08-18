using UnityEngine;

public class Apple : MonoBehaviour
{
    public AppleEffectData effect;

    private void Start()
    {
        effect = GetRandomEffect();
        NNDecisionMaker.appleEffect = effect;

        gameObject.GetComponent<SpriteRenderer>().color = effect.appleColor;
    }

    //private void OnTriggerEnter2D(Collider2D collider)
    //{
    //    if (collider.gameObject.GetComponent<SnakePart>() && collider.gameObject.GetComponent<SnakePart>().isHead)
    //    {
    //        var sr = GetComponent<SpriteRenderer>();
    //        Vector2 p = collider.ClosestPoint(transform.position);
    //        Color sparkColor = effect != null ? effect.appleColor : (sr != null ? sr.color : Color.red);
    //        ImpactSparks.BurstAt(p, sparkColor, transform.parent);
    //        transform.parent.GetComponentInChildren<Snake>().Eat(this);
    //        Destroy(gameObject);
    //    }
    //}

    public bool TryCollect(SnakePart part, Snake snake)
    {
        if (part == null || snake == null || !part.isHead)
            return false;

        var sr = GetComponent<SpriteRenderer>();
        Vector2 p = part.transform.position;
        Color sparkColor = effect != null ? effect.appleColor : (sr != null ? sr.color : Color.red);

        snake.Eat(this);
        Destroy(gameObject);

        ImpactSparks.BurstAt(p, sparkColor, transform.parent);

        return true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(
            $"[APPLE] TRIGGER! collider={collider.name}, " +
            $"layer={LayerMask.LayerToName(collider.gameObject.layer)}"
        );

        SnakePart part = collider.GetComponent<SnakePart>();

        Debug.Log(
            $"[APPLE] SnakePart = {(part != null ? part.name : "NULL")}"
        );

        if (part != null)
        {
            Snake snake = part.GetComponentInParent<Snake>();

            Debug.Log(
                $"[APPLE] Snake = {(snake != null ? snake.name : "NULL")}"
            );

            if (snake != null)
            {
                Debug.Log("[APPLE] Calling TryCollect()");
                TryCollect(part, snake);
            }
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
