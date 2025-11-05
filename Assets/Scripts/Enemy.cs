using UnityEngine;

/// <summary>
/// 👾 일반 적(잡몹)의 이동, 공격, 체력 및 사망 처리를 담당합니다.
/// [수정] 현재 씬이 보스 씬인지 일반 씬인지 확인하여 사망 로직을 다르게 처리합니다.
/// </summary>
public class Enemy : MonoBehaviour
{
    [Header("이동 관련 설정")]
    [Tooltip("적의 이동 속도입니다.")]
    public float moveSpeed = 3f;
    [Tooltip("이동 가능한 Y 최소값 (화면 아래쪽 경계)")]
    public float minY = -4f;
    [Tooltip("이동 가능한 Y 최대값 (화면 위쪽 경계)")]
    public float maxY = 4f;
    [Tooltip("이동 가능한 X 최소값 (화면 왼쪽 경계)")]
    public float minX = -8f;
    [Tooltip("이동 가능한 X 최대값 (화면 오른쪽 경계)")]
    public float maxX = 8f;

    [Header("전투 및 체력 설정")]
    [Tooltip("적의 최대 체력입니다.")]
    public int maxHealth = 3;
    [Tooltip("사망 시 생성될 폭발 이펙트 프리팹입니다.")]
    public GameObject explosionEffect;
    [Tooltip("적이 발사할 총알 프리팹입니다.")]
    public GameObject bulletPrefab;
    [Tooltip("총알 발사 간격(주기)입니다. (초)")]
    public float fireRate = 2f;

    [Header("총알 발사 위치 설정")]
    [Tooltip("총알이 발사될 위치(Transform) 배열입니다.")]
    public Transform[] firePoints;

    [Header("사운드 설정")]
    [Tooltip("사망 시 재생할 오디오 클립입니다.")]
    public AudioClip deathSound;

    // [private] 내부 변수
    private int currentHealth; // 현재 체력
    private float fireTimer;   // 다음 발사까지 남은 시간 (타이머)
    private bool movingUp = true;   // true: 위로, false: 아래로
    private bool movingRight = true;// true: 오른쪽으로, false: 왼쪽으로

    void Start()
    {
        currentHealth = maxHealth;
        fireTimer = fireRate;
    }

    void Update()
    {
        Move();
        HandleShooting();
    }

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
        if (bulletPrefab == null) return;

        if (firePoints != null && firePoints.Length > 0)
        {
            foreach (Transform point in firePoints)
            {
                if (point != null)
                    Instantiate(bulletPrefab, point.position, point.rotation);
            }
        }
        else
        {
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 사망 처리 (Die)
    /// </summary>
    void Die()
    {
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        if (deathSound != null)
            BGMManager.Instance?.PlaySFX(deathSound);

        // ────── ✨ 여기가 수정된 부분입니다! ──────
        //
        // [조건] 지금 이 씬에 'BossGameManager'가 '없다면' (== 일반 스테이지라면)
        // (BossGameManager.instance == null)
        //
        if (BossGameManager.instance == null)
        {
            // [실행] 일반 'GameManager'를 호출하여 킬 카운트 등을 처리합니다.
            // (이전에 주석 처리했던 부분을 다시 살려 이 안으로 넣습니다.)
            if (GameManager.instance != null)
            {
                GameManager.instance.OnEnemyKilled();
                Debug.Log("일반 스테이지 적 처치: GameManager 호출");
            }
        }
        else
        {
            // (BossGameManager.instance가 '있다면' == 보스 씬이라면)
            // 아무것도 호출하지 않습니다. (보스전 씬 재시작 버그 방지)
            Debug.Log("보스 스테이지 잡몹 처치: GameManager 호출 안 함");
        }
        // ──────────────────────────────────────────

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