using UnityEngine;

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

    void HandleNormalFire()
    {
        if (Time.time >= nextFireTime)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

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

            // 🔊 스페셜 사운드 재생 → BGMManager 통해 Mixer 반영
            if (specialClip != null)
            {
                BGMManager.Instance?.PlaySFX(specialClip);
            }
        }
    }

    public void EnableSpecialAttack()
    {
        isSpecialReady = true;
    }

    void FireLaser()
    {
        Instantiate(laserPrefab, laserFirePoint.position, laserFirePoint.rotation);
    }
}


