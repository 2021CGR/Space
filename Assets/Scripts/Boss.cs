using UnityEngine;

/// <summary>
/// 보스의 체력, 공격 패턴, 등장 애니메이션, 사망 처리까지 담당합니다.
/// 보스를 처치하면 ClearUIManager를 호출해서 클리어 패널을 띄웁니다.
/// </summary>
public class Boss : MonoBehaviour
{
    [Header("🩸 보스 체력 설정")]
    [Tooltip("보스의 최대 체력")]
    public int maxHealth = 100;
    private int currentHealth; // [수정] 현재 체력 (private로 변경)

    [Header("🎬 등장 애니메이션 설정")]
    [Tooltip("보스가 등장할 목표 위치")]
    public Vector3 targetPosition;
    [Tooltip("보스가 등장할 때의 이동 속도")]
    public float entrySpeed = 2f;
    private bool hasEntered = false; // 등장 완료 여부

    [Header("🌀 공격 패턴 관련 설정")]
    [Tooltip("다음 패턴까지의 대기 시간 (초)")]
    public float patternInterval = 1f;
    private float patternTimer = 0f; // 패턴 주기 타이머
    private int currentPattern = 0; // 현재 패턴 인덱스

    [Header("💥 이펙트 및 탄환 프리팹")]
    [Tooltip("사망 시 생성될 폭발 이펙트")]
    public GameObject explosionEffect;
    [Tooltip("보스가 발사할 총알 프리팹 배열 (패턴 순서대로)")]
    public GameObject[] bulletPrefabs;
    [Tooltip("총알이 발사되는 위치 배열")]
    public Transform[] firePoints;

    [Header("🔊 사운드 설정")]
    [Tooltip("사망 시 재생할 소리")]
    public AudioClip deathSound;

    void Start()
    {
        // [추가] 보스 시작 시 체력 초기화
        currentHealth = maxHealth;
        // [추가] 등장 완료 플래그 초기화
        hasEntered = false;
    }

    void Update()
    {
        // [수정] 아직 등장 중이면 이동만 처리
        if (!hasEntered)
        {
            HandleEntry();
            return; // 등장 중일 때는 아래 공격 로직을 실행하지 않음
        }

        // [수정] 등장이 완료되었으면 공격 패턴 처리
        HandleAttackPatterns();
    }

    /// <summary>
    /// [추가] 보스 등장 연출을 처리하는 함수
    /// </summary>
    void HandleEntry()
    {
        // 지정된 위치까지 부드럽게 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, entrySpeed * Time.deltaTime);

        // 목표 위치에 거의 도착했다면 등장 완료 처리
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            hasEntered = true;
            Debug.Log("🎬 보스 등장 완료!");
        }
    }

    /// <summary>
    /// [추가] 보스의 공격 패턴을 처리하는 함수
    /// </summary>
    void HandleAttackPatterns()
    {
        patternTimer += Time.deltaTime;

        // 설정된 간격(patternInterval)마다 패턴 실행
        if (patternTimer >= patternInterval)
        {
            patternTimer = 0f; // 타이머 초기화

            // [추가] 발사할 총알이 있는지 확인
            if (bulletPrefabs.Length == 0 || firePoints.Length == 0)
            {
                Debug.LogWarning("총알 프리팹 또는 발사 위치가 설정되지 않았습니다.");
                return;
            }

            // 현재 패턴에 맞는 총알 발사
            FirePattern(currentPattern);

            // [수정] 다음 패턴으로 순환 ( % 연산자: 배열 길이를 넘어가면 0으로 돌아옴)
            currentPattern = (currentPattern + 1) % bulletPrefabs.Length;
        }
    }

    /// <summary>
    /// 지정된 총알 프리팹을 모든 발사 위치에서 생성합니다.
    /// </summary>
    /// <param name="index">bulletPrefabs 배열의 인덱스</param>
    void FirePattern(int index)
    {
        foreach (Transform point in firePoints)
        {
            // [추가] 발사 위치(point)가 null이 아닌지 확인
            if (point != null)
            {
                Instantiate(bulletPrefabs[index], point.position, Quaternion.identity);
            }
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
    /// 총알 등에 의해 데미지를 입었을 때 호출됩니다.
    /// </summary>
    public void TakeDamage(int damage)
    {
        // [추가] 등장 중이거나 이미 죽었다면 데미지를 받지 않음
        if (!hasEntered || currentHealth <= 0)
        {
            if (!hasEntered) Debug.Log("🛡️ 보스는 아직 등장 중이라 데미지를 받지 않음.");
            return;
        }

        currentHealth -= damage;
        Debug.Log($"🩸 보스 체력: {currentHealth} / {maxHealth}");

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
        Debug.Log("🎉 보스 처치 완료!");

        // 폭발 이펙트 생성
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // 사망 사운드 재생 (BGMManager가 있다면 BGMManager를 통하는 것이 좋음)
        // [수정] BGMManager 싱글톤을 사용하여 SFX 재생 시도
        if (deathSound != null)
        {
            if (BGMManager.Instance != null)
            {
                BGMManager.Instance.PlaySFX(deathSound);
            }
            else
            {
                // BGMManager가 없을 경우를 대비한 예전 방식
                AudioSource.PlayClipAtPoint(deathSound, transform.position);
            }
        }

        // [수정] 클리어 UI 호출 (FindObjectOfType은 씬에 ClearUIManager가 1개만 있다는 가정 하에 작동)
        // 참고: FindObjectOfType은 성능에 부하를 줄 수 있으므로,
        // BossGameManager 같은 싱글톤 매니저를 통해 호출하는 것이 더 좋습니다.
        ClearUIManager clearUI = FindObjectOfType<ClearUIManager>();
        if (clearUI != null)
        {
            Debug.Log("🎯 클리어 UI 호출됨!");
            clearUI.ShowClear();
        }
        else
        {
            // [수정] BossGameManager를 통한 호출 시도 (대안)
            if (BossGameManager.instance != null)
            {
                Debug.Log("🎯 BossGameManager의 클리어 UI 호출!");
                BossGameManager.instance.OnBossDefeated();
            }
            else
            {
                Debug.LogWarning("⚠️ ClearUIManager와 BossGameManager를 찾을 수 없음.");
            }
        }

        // 보스 오브젝트 제거
        Destroy(gameObject);
    }



    /// <summary>
    /// 현재 체력을 0과 1 사이의 값(백분율)으로 반환 (UI에 활용)
    /// </summary>
    public float GetHealthPercent()
    {
        // [추가] maxHealth가 0이 되어 나누기 오류가 나는 것을 방지
        if (maxHealth <= 0) return 0f;

        // [수정] (float) 형변환을 통해 소수점까지 정확히 계산
        return (float)currentHealth / maxHealth;
    }
}