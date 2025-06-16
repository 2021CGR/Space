using UnityEngine;

/// <summary>
/// 보스의 등장, 공격 패턴, 데미지 처리, 사망을 포함한 전체 제어 스크립트.
/// 이번 수정에서는 '등장 중 무적 상태' 로직과 사운드 재생이 추가됨.
/// </summary>
public class Boss : MonoBehaviour
{
    [Header("보스 체력")]
    public int maxHealth = 100;         // 최대 체력
    private int currentHealth;          // 현재 체력

    [Header("이펙트 및 탄환 설정")]
    public GameObject explosionEffect;  // 사망 시 폭발 이펙트
    public GameObject[] bulletPrefabs;  // 공격 패턴별 총알 프리팹
    public Transform[] firePoints;      // 총알 발사 위치들

    [Header("공격 패턴 설정")]
    private float patternTimer = 0f;          // 공격 타이머
    public float patternInterval = 1f;        // 공격 간격 (초)
    private int currentPattern = 0;           // 현재 공격 패턴 인덱스

    [Header("등장 애니메이션 설정")]
    public Vector3 targetPosition;     // 보스가 멈출 위치
    public float entrySpeed = 2f;      // 등장 속도
    private bool hasEntered = false;   // 등장 완료 여부 (공격/피격 조건에 사용)

    [Header("사운드 설정")]
    public AudioClip deathSound;       // 사망 시 재생할 사운드

    void Start()
    {
        // 체력 초기화
        currentHealth = maxHealth;
    }

    void Update()
    {
        // 아직 등장 중이면 타겟 위치까지 이동
        if (!hasEntered)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, entrySpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                hasEntered = true; // 도착 완료
            }

            return; // 아직 공격하지 않음
        }

        // 도착 이후에는 공격 실행
        patternTimer += Time.deltaTime;

        if (patternTimer >= patternInterval)
        {
            patternTimer = 0f;
            FirePattern(currentPattern);
            currentPattern = (currentPattern + 1) % bulletPrefabs.Length;
        }
    }

    /// <summary>
    /// 각 발사 위치에서 총알 생성
    /// </summary>
    void FirePattern(int index)
    {
        foreach (Transform point in firePoints)
        {
            Instantiate(bulletPrefabs[index], point.position, Quaternion.identity);
        }
    }

    /// <summary>
    /// 보스가 등장을 완료했는지 외부에서 확인할 수 있도록 반환하는 함수
    /// </summary>
    public bool HasEntered()
    {
        return hasEntered;
    }

    /// <summary>
    /// 외부에서 보스가 데미지를 입을 때 호출됨
    /// 단, 등장이 완료되지 않은 경우 무시됨 (무적 상태)
    /// </summary>
    /// <param name="damage">입히는 데미지 양</param>
    public void TakeDamage(int damage)
    {
        // 보스가 아직 등장 중이면 데미지를 무시함 (무적)
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
    /// 보스가 사망했을 때 폭발 이펙트를 생성하고 사운드 재생 후 제거
    /// </summary>
    void Die()
    {
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // 사운드가 설정되어 있다면 해당 위치에서 재생
        if (deathSound != null)
            AudioSource.PlayClipAtPoint(deathSound, transform.position);

        Debug.Log("🎉 보스 처치 완료!");
        Destroy(gameObject);
    }

    /// <summary>
    /// 플레이어 총알과 충돌 시 데미지 적용
    /// 단, 등장 중에는 무적이므로 TakeDamage 내부에서 판단
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    /// <summary>
    /// UI 체력바에 사용되는 현재 체력 비율 반환 (0~1)
    /// </summary>
    public float GetHealthPercent()
    {
        return (float)currentHealth / maxHealth;
    }
}

