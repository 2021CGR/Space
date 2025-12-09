using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public float duration = 0.5f;
    public float warmup = 0.12f;
    public float cooldown = 0.18f;
    public int damage = 999;
    public bool scaleEase = true;
    public float scaleInMin = 0.85f;
    public float scaleOutMax = 1.05f;
    public AnimationCurve alphaIn = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public AnimationCurve alphaOut = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

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
        float alpha = 1f;

        if (time <= warmup)
        {
            float tIn = Mathf.Clamp01(time / warmup);
            alpha = alphaIn.Evaluate(tIn);
            if (scaleEase)
            {
                float s = Mathf.SmoothStep(0f, 1f, tIn);
                transform.localScale = Vector3.Lerp(initialScale * scaleInMin, initialScale, s);
            }
        }
        else if (time >= duration - cooldown)
        {
            float tOut = Mathf.Clamp01((time - (duration - cooldown)) / cooldown);
            alpha = alphaOut.Evaluate(tOut);
            if (scaleEase)
            {
                float s = Mathf.SmoothStep(0f, 1f, tOut);
                transform.localScale = Vector3.Lerp(initialScale, initialScale * scaleOutMax, s);
            }
        }

        if (renderers != null)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                var c = renderers[i].color;
                c.a = alpha;
                renderers[i].color = c;
            }
        }

        if (time >= duration)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
