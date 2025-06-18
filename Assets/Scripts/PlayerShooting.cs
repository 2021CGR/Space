using UnityEngine;

/// <summary>
/// 플레이어의 일반 총알 및 스페셜 공격 기능.
/// 각 발사마다 사운드를 재생함.
/// </summary>
public class PlayerShooting : MonoBehaviour
{
    [Header("총알 설정")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.25f;

    private float nextFireTime;

    [Header("스페셜 레이저 설정")]
    public GameObject laserPrefab;
    public Transform laserFirePoint;
    private bool isSpecialReady = false;

    [Header("사운드 클립")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip specialClip;

    void Update()
    {
        HandleNormalFire();
        HandleSpecialFire();
    }

    /// <summary>
    /// 일반 총알 자동 발사 + 사운드
    /// </summary>
    void HandleNormalFire()
    {
        if (Time.time >= nextFireTime)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // 🔊 총알 사운드 재생
            if (shootClip != null)
            {
                AudioSource.PlayClipAtPoint(shootClip, firePoint.position);
            }

            nextFireTime = Time.time + fireRate;
        }
    }

    /// <summary>
    /// 스페셜 공격 키 입력 시 발사 + 사운드
    /// </summary>
    void HandleSpecialFire()
    {
        if (isSpecialReady && Input.GetKeyDown(KeyCode.Space))
        {
            FireLaser();
            isSpecialReady = false;

            // 에너지 소비
            PlayerSpecialEnergy energy = GetComponent<PlayerSpecialEnergy>();
            if (energy != null)
            {
                energy.ConsumeEnergy();
            }

            // 🔊 스페셜 사운드 재생
            if (specialClip != null)
            {
                AudioSource.PlayClipAtPoint(specialClip, laserFirePoint.position);
            }
        }
    }

    /// <summary>
    /// 외부에서 스페셜 공격 활성화
    /// </summary>
    public void EnableSpecialAttack()
    {
        isSpecialReady = true;
    }

    /// <summary>
    /// 레이저 발사
    /// </summary>
    void FireLaser()
    {
        Instantiate(laserPrefab, laserFirePoint.position, laserFirePoint.rotation);
    }
}

