using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("총알 설정")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.25f;

    private float nextFireTime;

    [Header("듀얼샷 설정")]
    public Transform leftFirePoint;     // 왼쪽 총알 발사 위치
    public Transform rightFirePoint;    // 오른쪽 총알 발사 위치
    public float dualShotDuration = 10f; // 듀얼샷 지속 시간 (인스펙터 수정 가능)
    
    private bool isDualShotActive = false;
    private float dualShotTimer = 0f;

    [Header("스페셜 레이저 설정")]
    public GameObject laserPrefab;
    public Transform laserFirePoint;
    private bool isSpecialReady = false;

    [Header("사운드 클립")]
    [SerializeField] private AudioClip shootClip;

    void Update()
    {
        // [수정] 스토리가 진행 중이거나 게임이 멈춰있을 때는 발사 금지
        // (오프닝 스토리 OR 엔딩 스토리 중일 때)
        if (StoryIntroManager.IsPlaying || EndingStoryManager.IsPlaying || Time.timeScale == 0f)
            return;

        HandleNormalFire();
        HandleSpecialFire();
        HandleDualShotTimer();
    }

    void HandleNormalFire()
    {
        if (Time.time >= nextFireTime)
        {
            if (isDualShotActive)
            {
                // 듀얼샷 활성화 시 두 발 발사
                if (leftFirePoint != null)
                    Instantiate(bulletPrefab, leftFirePoint.position, leftFirePoint.rotation);
                if (rightFirePoint != null)
                    Instantiate(bulletPrefab, rightFirePoint.position, rightFirePoint.rotation);
            }
            else
            {
                // 기본 총알 한 발
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            }

            // 🔊 총알 사운드 재생 → BGMManager 통해 Mixer 반영
            if (shootClip != null)
            {
                BGMManager.Instance?.PlaySFX(shootClip);
            }

            nextFireTime = Time.time + fireRate;
        }
    }

    void HandleSpecialFire()
    {
        if (isSpecialReady && Input.GetKeyDown(KeyCode.Space))
        {
            FireLaser();
            isSpecialReady = false;

            PlayerSpecialEnergy energy = GetComponent<PlayerSpecialEnergy>();
            if (energy != null)
            {
                energy.ConsumeEnergy();
            }
        }
    }

    void HandleDualShotTimer()
    {
        if (isDualShotActive)
        {
            dualShotTimer -= Time.deltaTime;
            if (dualShotTimer <= 0f)
            {
                isDualShotActive = false;
                Debug.Log("🔫 듀얼샷 종료: 원래대로 한 발만 발사");
            }
        }
    }

    public void EnableSpecialAttack()
    {
        isSpecialReady = true;
    }

    public void EnableDualShot()
    {
        isDualShotActive = true;
        dualShotTimer = dualShotDuration;
        Debug.Log("🔫 듀얼샷 활성화: 10초간 2발 발사");
    }

    void FireLaser()
    {
        Instantiate(laserPrefab, laserFirePoint.position, laserFirePoint.rotation);
    }
}