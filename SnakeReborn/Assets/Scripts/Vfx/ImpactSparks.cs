using UnityEngine;

/// <summary>
/// One-shot burst of small particles at a point (no prefab required).
/// </summary>
public static class ImpactSparks
{
    public static void BurstAt(Vector2 worldPosition, Color color, Transform parent = null, int count = 18)
    {
        var go = new GameObject("ImpactSparks");
        go.transform.position = worldPosition;
        if (parent != null)
            go.transform.SetParent(parent, true);

        var ps = go.AddComponent<ParticleSystem>();

        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = ps.main;
        main.playOnAwake = false;
        main.loop = false;
        main.duration = 0.12f;

        float glowIntensity = 2f;
        Color hdrColor = new Color(color.r * glowIntensity, color.g * glowIntensity, color.b * glowIntensity, color.a);

        main.startLifetime = new ParticleSystem.MinMaxCurve(0.25f, 0.55f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(1.2f, 3.8f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.04f, 0.11f);
        main.startColor = color;
        main.maxParticles = 64;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.gravityModifier = 0.35f;

        main.startColor = hdrColor;

        var emission = ps.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, (short)Mathf.Clamp(count, 4, 48)) });

        var shape = ps.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.08f;

        var col = ps.colorOverLifetime;
        col.enabled = true;
        var grad = new Gradient();
        grad.SetKeys(
            new[] { new GradientColorKey(hdrColor, 0f), new GradientColorKey(hdrColor, 1f) }, // Тоже HDR
            new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) });
        col.color = grad;

        var sz = ps.sizeOverLifetime;
        sz.enabled = true;
        sz.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, 0f));

        var rend = ps.GetComponent<ParticleSystemRenderer>();
        rend.sortingOrder = 40;

        // ВЫБОР МАТЕРИАЛА ДЛЯ СВЕЧЕНИЯ
        // Для стандартного пайплайна (Built-in) используем аддитивный шейдер частиц:
        Material particleMat = new Material(Shader.Find("Particles/Standard Unlit"));

        // Переключаем режим смешивания на Additive (сложение цветов дает сильный эффект свечения)
        particleMat.SetFloat("_Mode", 4f);
        particleMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        particleMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);
        particleMat.SetInt("_ZWrite", 0);
        particleMat.DisableKeyword("_ALPHATEST_ON");
        particleMat.EnableKeyword("_ALPHABLEND_ON");
        particleMat.EnableKeyword("_ALPHAPREMULTIPLY_ON");

        rend.material = particleMat;

        ps.Play();

        float kill = main.duration + main.startLifetime.constantMax + 0.15f;
        Object.Destroy(go, kill);
    }
}
