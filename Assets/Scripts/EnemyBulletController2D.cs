using UnityEngine;

/// <summary>
/// 2D 적 총알의 이동과 충돌을 처리합니다.
/// </summary>
[RequireComponent(typeof(Collider2D))] // [추가] 충돌 감지를 위해 Collider2D 필수
public class EnemyBulletController2D : MonoBehaviour
{
    [Header("총알 속도 및 데미지")]
    [Tooltip("총알이 날아가는 속도")]
    public float speed = 10f;
    [Tooltip("총알이 파괴되기까지의 생존 시간 (초)")]
    public float lifetime = 5f;
    [Tooltip("총알이 플레이어에게 입히는 데미지")]
    public int damage = 1;

    void Start()
    {
        // [추가] lifetime 초 후에 이 총알 오브젝트를 자동으로 파괴
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // [수정] 2D 환경이므로 Vector2.left (왼쪽)으로 이동
        // 3D 환경이라면 Vector3.left 또는 -transform.right 등을 사용
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    /// <summary>
    /// [수정] 2D 물리 충돌 감지 (Collider2D가 IsTrigger=true여야 함)
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        // "Player" 태그가 붙은 오브젝트와 충돌했는지 확인
        if (other.CompareTag("Player"))
        {
            Debug.Log("적 총알이 플레이어에 충돌!");

            // [수정] 충돌한 오브젝트에서 PlayerHealth 컴포넌트(가상)를 가져옴
            // (제공된 파일에 PlayerHealth가 없으므로, 실제 사용하는 스크립트 이름으로 변경 필요)
            PlayerHealth player = other.GetComponent<PlayerHealth>(); // PlayerHealth는 예시 이름
            if (player != null)
            {
                player.TakeDamage(damage); // 플레이어의 TakeDamage 함수 호출
            }
            else
            {
                Debug.LogWarning("플레이어에서 PlayerHealth 스크립트를 찾지 못했습니다.");
            }

            // [추가] 플레이어와 충돌 시 총알 즉시 제거
            Destroy(gameObject);
        }
    }
}