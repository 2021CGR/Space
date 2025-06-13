using UnityEngine;

/// <summary>
/// 플레이어 체력 관리 + 죽을 때 폭발 이펙트 + 게임오버 호출
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [Header("체력 설정")]
    public int maxHealth = 5;
    private int currentHealth;

    [Header("폭발 이펙트")]
    public GameObject explosionEffectPrefab; // 플레이어 사망 시 보여줄 폭발 프리팹
    public float explosionLifetime = 1.5f;   // 폭발 이펙트 지속 시간

    [Header("게임 오버 매니저")]
    public GameOverManager gameOverManager;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("플레이어가 데미지를 입음. 현재 체력: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("☠️ 플레이어 사망 처리 시작");

        // 폭발 이펙트 생성
        if (explosionEffectPrefab != null)
        {
            GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, explosionLifetime); // 일정 시간 후 자동 제거
        }

        // 플레이어 비활성화
        gameObject.SetActive(false);

        // 게임 오버 처리
        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOver();
        }
        else
        {
            Debug.LogWarning("⚠️ GameOverManager가 연결되지 않았습니다.");
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}

