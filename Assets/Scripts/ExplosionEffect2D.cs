using UnityEngine;

public class ExplosionEffect2D : MonoBehaviour
{
    [Header("이펙트 유지 시간 (초)")]
    public float duration = 0.5f;

    void Start()
    {
        Destroy(gameObject, duration);
    }
}
