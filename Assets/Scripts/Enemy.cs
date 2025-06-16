using UnityEngine;

/// <summary>
/// 적 캐릭터의 전체 동작을 담당하는 스크립트.
/// - 상하 + 좌우 이동 (범위 반사형)
/// - 일정 시간마다 총알 발사
/// - 플레이어 총알 충돌 시 데미지 및 사망 처리
/// </summary>
public class Enemy : MonoBehaviour
{
    [Header("이동 관련 설정")]
    public float moveSpeed = 3f;       // 이동 속도

    public float minY = -4f;           // Y축 이동 최소값
    public float maxY = 4f;            // Y축 이동 최대값
    public float minX = -8f;           // X축 이동 최소값
    public float maxX = 8f;            // X축 이동 최대값

    [Header("전투 및 체력 설정")]
    public int maxHealth = 3;                  // 최대 체력
    public GameObject explosionEffect;         // 사망 시 폭발 이펙트
    public GameObject bulletPrefab;            // 총알 프리팹
    public float fireRate = 2f;                // 발사 간격

    [Header("총알 발사 위치 설정")]
    public Transform[] firePoints;             // 총알 발사 위치들

    [Header("사운드 설정")]
    public AudioClip deathSound;               // 사망 시 효과음

    // 내부 상태
    private int currentHealth;
    private float fireTimer;
    private bool movingUp = true;
    private bool movingRight = true;

    void Start()
    {
        currentHealth = maxHealth;
        fireTimer = fireRate;
    }

    void Update()
    {
        Move();               // 좌우 + 상하 이동
        HandleShooting();    // 총알 발사
    }

    /// <summary>
    /// 적이 상하 + 좌우로 지정된 범위 안에서 자동으로 이동
    /// 범위를 벗어나면 방향 반전
    /// </summary>
    void Move()
    {
        float yDirection = movingUp ? 1f : -1f;
        float xDirection = movingRight ? 1f : -1f;

        Vector3 movement = new Vector3(xDirection, yDirection, 0f) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        Vector3 pos = transform.position;

        if (movingUp && pos.y >= maxY) movingUp = false;
        else if (!movingUp && pos.y <= minY) movingUp = true;

        if (movingRight && pos.x >= maxX) movingRight = false;
        else if (!movingRight && pos.x <= minX) movingRight = true;
    }

    /// <summary>
    /// 총알 발사 처리 (지정된 위치에서 동시에 발사)
    /// </summary>
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
        if (firePoints != null && firePoints.Length > 0)
        {
            foreach (Transform point in firePoints)
            {
                Instantiate(bulletPrefab, point.position, Quaternion.identity);
            }
        }
        else
        {
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        }
    }

    /// <summary>
    /// 데미지를 입고 체력이 0 이하가 되면 사망
    /// </summary>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 폭발 이펙트 생성 후 제거 + GameManager에 처치 통보 + 사망 사운드 재생
    /// </summary>
    void Die()
    {
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        if (deathSound != null)
            AudioSource.PlayClipAtPoint(deathSound, transform.position);

        if (GameManager.instance != null)
            GameManager.instance.OnEnemyKilled();

        Destroy(gameObject);
    }

    /// <summary>
    /// 플레이어 총알과 충돌 시 데미지 처리
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }
}
