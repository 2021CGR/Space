using UnityEngine;

public class ExplosionEffect2D : MonoBehaviour
{
    public float duration = 0.6f;
    public AnimationCurve alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
    public bool scaleEase = true;
    public float scaleMultiplier = 1.1f;

    private float time;
    private SpriteRenderer[] renderers;
    private Vector3 initialScale;

    void Awake()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        initialScale = transform.localScale;
    }

    void Update()
    {
        time += Time.deltaTime;
        float t = Mathf.Clamp01(time / duration);
        float a = alphaCurve.Evaluate(t);

        if (renderers != null)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                var c = renderers[i].color;
                c.a = a;
                renderers[i].color = c;
            }
        }

        if (scaleEase)
        {
            float s = Mathf.SmoothStep(0f, 1f, t);
            transform.localScale = Vector3.Lerp(initialScale, initialScale * scaleMultiplier, s);
        }

        if (time >= duration)
        {
            Destroy(gameObject);
        }
    }
}
