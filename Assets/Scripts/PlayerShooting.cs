using UnityEngine;

/// <summary>
/// 플레이어의 일반 공격 및 스페셜 레이저 발사를 제어하는 스크립트.
/// 에너지 아이템을 먹으면 스페셜 공격이 가능해지고, 사용 후 에너지 상태를 초기화.
/// </summary>
public class PlayerShooting : MonoBehaviour
{
    [Header("일반 공격 설정")]
    public GameObject bulletPrefab;           // 일반 총알 프리팹
    public Transform firePoint;               // 일반 총알 발사 위치
    public float fireRate = 0.25f;            // 일반 공격 속도
    private float nextFireTime = 0f;          // 다음 공격 가능한 시간

    [Header("스페셜 레이저 설정")]
    public GameObject laserPrefab;            // 스페셜 레이저 프리팹
    public Transform laserFirePoint;          // 레이저 발사 위치
    private bool isSpecialReady = false;      // 스페셜 발사 가능 여부

    [Header("사운드 설정")]
    public AudioClip shootSound;              // 총알 발사 효과음
    private AudioSource audioSource;          // AudioSource 컴포넌트 참조

    void Start()
    {
        // AudioSource 가져오기
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleNormalFire();
        HandleSpecialFire();
    }

    /// <summary>
    /// 일반 총알 발사 처리 (자동)
    /// </summary>
    void HandleNormalFire()
    {
        if (Time.time >= nextFireTime)
        {
            // 총알 생성
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // 발사 사운드 재생
            if (shootSound != null && audioSource != null)
                audioSource.PlayOneShot(shootSound);

            // 다음 발사 시간 갱신
            nextFireTime = Time.time + fireRate;
        }
    }

    /// <summary>
    /// 스페셜 레이저 발사 처리
    /// </summary>
    void HandleSpecialFire()
    {
        // 스페셜 준비 상태 + 스페이스바 입력일 때만 발사
        if (isSpecialReady && Input.GetKeyDown(KeyCode.Space))
        {
            FireLaser();              // 레이저 생성
            isSpecialReady = false;  // 상태 초기화

            // 에너지 상태 초기화 → 아이콘도 사라짐
            PlayerSpecialEnergy energy = GetComponent<PlayerSpecialEnergy>();
            if (energy != null)
            {
                energy.ConsumeEnergy();
            }
        }
    }

    /// <summary>
    /// 레이저를 생성하는 함수
    /// </summary>
    void FireLaser()
    {
        Instantiate(laserPrefab, laserFirePoint.position, laserFirePoint.rotation);
    }

    /// <summary>
    /// 외부에서 에너지를 획득했을 때 호출 → 스페셜 사용 가능 상태로 설정
    /// </summary>
    public void EnableSpecialAttack()
    {
        isSpecialReady = true;
    }
}


