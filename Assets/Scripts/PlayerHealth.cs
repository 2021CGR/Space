using UnityEngine;

/// <summary>
/// 플레이어 체력 관리, 폭발 이펙트, 사망 시 사운드 재생 및 게임오버 처리
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [Header("체력 설정")]
    public int maxHealth = 5;
    private int currentHealth;

    [Header("폭발 이펙트")]
    public GameObject explosionEffectPrefab;
    public float explosionLifetime = 1.5f;

    [Header("게임 오버 매니저")]
    public GameOverManager gameOverManager;

    [Header("폭발 사운드 클립")]
    [SerializeField] private AudioClip explosionClip;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log($"🩸 플레이어 체력 초기화: {currentHealth}");
    }

    void Update()
    {
        // 테스트용: K키로 강제 사망
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("🧪 K키 입력 → 강제 Die() 실행");
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"💥 데미지 입음: -{damage} → 현재 체력: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("☠️ 플레이어 사망 처리 시작");

        // 💥 폭발 이펙트
        if (explosionEffectPrefab != null)
        {
            GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, explosionLifetime);
        }

        // 🔊 폭발 사운드 (PlayClipAtPoint 사용)
        if (explosionClip != null)
        {
            AudioSource.PlayClipAtPoint(explosionClip, transform.position);
            Debug.Log("💥 폭발 사운드 재생됨");
        }
        else
        {
            Debug.LogWarning("❌ 폭발 사운드 클립이 연결되어 있지 않음");
        }

        // 🛑 플레이어 비활성화
        gameObject.SetActive(false);

        // 🕹️ 게임 오버 처리
        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOver();
        }
        else
        {
            Debug.LogWarning("⚠️ GameOverManager가 연결되어 있지 않음");
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
