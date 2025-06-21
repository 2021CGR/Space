using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("이동 관련 설정")]
    public float moveSpeed = 3f;
    public float minY = -4f;
    public float maxY = 4f;
    public float minX = -8f;
    public float maxX = 8f;

    [Header("전투 및 체력 설정")]
    public int maxHealth = 3;
    public GameObject explosionEffect;
    public GameObject bulletPrefab;
    public float fireRate = 2f;

    [Header("총알 발사 위치 설정")]
    public Transform[] firePoints;

    [Header("사운드 설정")]
    public AudioClip deathSound;

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
        if (firePoints != null && firePoints.Length > 0)
        {
            foreach (Transform point in firePoints)
            {
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

        // 🔊 사망 사운드 → Mixer 반영
        if (deathSound != null)
            BGMManager.Instance?.PlaySFX(deathSound);

        if (GameManager.instance != null)
            GameManager.instance.OnEnemyKilled();

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
