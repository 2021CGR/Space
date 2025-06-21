using UnityEngine;

/// <summary>
/// 이 스크립트는 보스의 체력, 공격 패턴, 등장 애니메이션, 사망 처리까지 담당해.
/// 보스를 처치하면 ClearUIManager를 호출해서 클리어 패널을 띄워줄 수 있어!
/// </summary>
public class Boss : MonoBehaviour
{
    [Header("🩸 보스 체력 설정")]
    public int maxHealth = 100;         // 최대 체력
    private int currentHealth;          // 현재 체력

    [Header("💥 이펙트 및 탄환 프리팹")]
    public GameObject explosionEffect;  // 사망 시 폭발 이펙트
    public GameObject[] bulletPrefabs;  // 다양한 패턴의 총알 프리팹
    public Transform[] firePoints;      // 총알이 발사되는 위치들

    [Header("🌀 공격 패턴 관련 설정")]
    private float patternTimer = 0f;     // 패턴 주기 타이머
    public float patternInterval = 1f;   // 패턴 주기 간격 (초)
    private int currentPattern = 0;      // 현재 패턴 인덱스

    [Header("🎬 등장 애니메이션 설정")]
    public Vector3 targetPosition;       // 등장 애니메이션 타겟 위치
    public float entrySpeed = 2f;        // 등장 속도
    private bool hasEntered = false;     // 등장 완료 여부

    [Header("🔊 사운드 설정")]
    public AudioClip deathSound;         // 사망 시 재생할 소리

    void Start()
    {
        // 보스 시작 시 체력 초기화
        currentHealth = maxHealth;
    }

    void Update()
    {
        // 아직 등장 중이면 이동만 처리하고 리턴
        if (!hasEntered)
        {
            // 지정된 위치까지 부드럽게 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, entrySpeed * Time.deltaTime);

            // 거의 도착했다면 등장 완료
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                hasEntered = true;
            }

            return;
        }

        // 등장 완료 후에는 공격 패턴 시작
        patternTimer += Time.deltaTime;

        if (patternTimer >= patternInterval)
        {
            patternTimer = 0f;

            // 현재 패턴에 맞는 총알 발사
            FirePattern(currentPattern);

            // 다음 패턴으로 순환
            currentPattern = (currentPattern + 1) % bulletPrefabs.Length;
        }
    }

    /// <summary>
    /// 지정된 총알 프리팹을 모든 발사 위치에서 생성해
    /// </summary>
    void FirePattern(int index)
    {
        foreach (Transform point in firePoints)
        {
            Instantiate(bulletPrefabs[index], point.position, Quaternion.identity);
        }
    }

    /// <summary>
    /// 외부에서 보스가 등장 완료했는지 확인할 수 있는 함수
    /// </summary>
    public bool HasEntered()
    {
        return hasEntered;
    }

    /// <summary>
    /// 총알 등에 의해 데미지를 입었을 때 호출됨
    /// </summary>
    public void TakeDamage(int damage)
    {
        if (!hasEntered)
        {
            Debug.Log("🛡️ 보스는 아직 등장 중이라 데미지를 받지 않음.");
            return;
        }

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 보스가 사망할 때 호출됨 – 폭발 이펙트와 클리어 UI 표시
    /// </summary>
    void Die()
    {
        // 폭발 이펙트 생성
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // 사망 사운드 재생
        if (deathSound != null)
            AudioSource.PlayClipAtPoint(deathSound, transform.position);

        Debug.Log("🎉 보스 처치 완료!");

        // 클리어 UI 호출 시도
        var clearUI = FindObjectOfType<ClearUIManager>();
        if (clearUI != null)
        {
            Debug.Log("🎯 클리어 UI 호출됨!");
            clearUI.ShowClear();
        }
        else
        {
            Debug.LogWarning("⚠️ ClearUIManager를 찾을 수 없음.");
        }

        // 보스 오브젝트 제거
        Destroy(gameObject);
    }

    /// <summary>
    /// 총알과 충돌 시 데미지를 입도록 처리
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            TakeDamage(1); // 충돌 시 데미지 1
            Destroy(other.gameObject); // 총알 제거
        }
    }

    /// <summary>
    /// 현재 체력을 백분율로 반환 (UI에 활용할 수 있어)
    /// </summary>
    public float GetHealthPercent()
    {
        return (float)currentHealth / maxHealth;
    }
}
