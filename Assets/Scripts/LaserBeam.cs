using UnityEngine;

/// <summary>
/// 플레이어의 강한 레이저: 고정된 위치에서 발사되어
/// 적을 감지하여 데미지를 입히고, 일정 시간 후 사라짐.
/// </summary>
public class LaserBeam : MonoBehaviour
{
    [Header("지속 시간")]
    public float duration = 0.4f;         // 레이저 유지 시간 (초)

    [Header("데미지 설정")]
    public int damage = 999;              // 강한 데미지 (한 방에 죽는 수준)

    void Start()
    {
        // 일정 시간이 지나면 자동으로 레이저 오브젝트 제거
        Destroy(gameObject, duration);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 적과 충돌했을 때만 처리
        if (other.CompareTag("Enemy"))
        {
            // Enemy 스크립트를 찾아 데미지를 적용
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // 강한 데미지 적용
            }

            // (선택) 관통형이 아니라면 아래 줄을 주석 해제해서 즉시 제거
            // Destroy(gameObject);
        }
    }

    // Update() 제거됨 — 더 이상 이동하지 않음
}
