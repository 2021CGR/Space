using UnityEngine;

/// <summary>
/// 플레이어가 발사한 총알의 이동, 충돌, 데미지 처리 로직을 담당하는 스크립트
/// </summary>
public class PlayerBulletController2D : MonoBehaviour
{
    public float speed = 10f;      // 총알의 이동 속도
    public float lifetime = 5f;    // 총알이 몇 초 후 사라질지 (초 단위)
    public int damage = 1;         // 총알이 입히는 데미지

    void Start()
    {
        // 일정 시간이 지나면 총알 자동 제거 (최대 수명)
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // 오른쪽 방향으로 총알 이동 (Vector2.right = (1, 0))
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    /// <summary>
    /// 총알이 충돌했을 때 호출되는 함수 (Trigger 충돌 전용)
    /// </summary>
    /// <param name="other">충돌한 콜라이더</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // 적(Enemy) 또는 보스(Boss) 태그를 가진 오브젝트에 충돌한 경우
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            Debug.Log("플레이어 총알이 적 또는 보스에 충돌!");

            // Enemy 타입인지 검사
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // 적에게 데미지 적용
            }

            // Boss 타입인지 검사
            Boss boss = other.GetComponent<Boss>();
            if (boss != null)
            {
                boss.TakeDamage(damage); // 보스에게 데미지 적용
            }

            // 총알 제거
            Destroy(gameObject);
        }
    }
}

