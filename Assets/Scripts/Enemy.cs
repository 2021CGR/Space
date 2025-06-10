using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;              // 적의 이동 속도 (위아래 이동 속도)
    public float verticalRange = 3f;          // 위아래 이동 범위
    public int maxHealth = 3;                 // 적의 최대 체력
    public GameObject explosionEffect;        // 파괴 시 이펙트
    public GameObject bulletPrefab;           // 적의 공격용 총알
    public float fireRate = 2f;               // 발사 간격 (초)
    public int currentHealth ;

    private float fireTimer;
    private Vector3 startPosition;            // 최초 위치 저장용
    private bool movingUp = true;             // 위로 올라가고 있는지 여부

    void Start()
    {
        currentHealth = maxHealth;            // 체력 초기화
        fireTimer = fireRate;                 // 발사 타이머 초기화
        startPosition = transform.position;   // 시작 위치 저장
    }

    void Update()
    {
        Move();               // 이동 처리
        HandleShooting();    // 발사 처리
    }

    void Move()
    {
        // 현재 이동 방향에 따라 이동
        float direction = movingUp ? 1f : -1f;

        // 이동 벡터 계산
        Vector3 movement = Vector3.up * direction * moveSpeed * Time.deltaTime;

        // 이동 적용
        transform.Translate(movement);

        // 이동 범위 체크 (시작 위치 기준)
        float distanceFromStart = transform.position.y - startPosition.y;

        // 설정한 범위를 벗어나면 방향 반전
        if (movingUp && distanceFromStart >= verticalRange)
        {
            movingUp = false;
        }
        else if (!movingUp && distanceFromStart <= -verticalRange)
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
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

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


