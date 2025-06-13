using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 3f;               // 적의 이동 속도
    public float minY = -4f;                   // 이동 가능한 최소 Y 위치
    public float maxY = 4f;                    // 이동 가능한 최대 Y 위치

    [Header("전투 설정")]
    public int maxHealth = 3;                  // 적의 최대 체력
    public GameObject explosionEffect;         // 파괴 시 이펙트
    public GameObject bulletPrefab;            // 적의 공격용 총알
    public float fireRate = 2f;                // 발사 간격 (초)

    private int currentHealth;                 // 현재 체력
    private float fireTimer;                   // 발사 타이머
    private bool movingUp = true;              // 현재 위로 이동 중인지 여부

    void Start()
    {
        currentHealth = maxHealth;             // 체력 초기화
        fireTimer = fireRate;                  // 타이머 초기화
    }

    void Update()
    {
        Move();                                // 이동 처리
        HandleShooting();                      // 공격 처리
    }

    void Move()
    {
        float direction = movingUp ? 1f : -1f;
        Vector3 movement = Vector3.up * direction * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        if (movingUp && transform.position.y >= maxY)
        {
            movingUp = false;
        }
        else if (!movingUp && transform.position.y <= minY)
        {
            movingUp = true;
        }
    }

    void HandleShooting()
    {
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate;
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 폭발 이펙트 처리
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // ✅ 추가된 부분: GameManager에 적 처치 알림
        if (GameManager.instance != null)
        {
            GameManager.instance.OnEnemyKilled();
        }

        // 적 제거
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }
}


