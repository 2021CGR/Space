using UnityEngine;

/// <summary>
/// 플레이어가 스페이스바를 누르면 총알을 발사하는 스크립트
/// 총알은 지정된 위치에서 생성되며 위 방향으로 날아간다.
/// </summary>
public class PlayerShooting : MonoBehaviour
{
    [Header("총알 설정")]
    public GameObject bulletPrefab;              // 발사할 총알 프리팹
    public Transform firePoint;                  // 총알이 생성될 위치

    [Header("발사 쿨다운")]
    public float fireDelay = 0.2f;               // 발사 간격 (초)
    private float lastFireTime;                  // 마지막 발사 시간 저장용

    void Update()
    {
        // 1. 스페이스바를 누르고, 발사 간격이 지나면
        if (Input.GetKey(KeyCode.Space) && Time.time >= lastFireTime + fireDelay)
        {
            Fire(); // 총알 발사
            lastFireTime = Time.time; // 마지막 발사 시간 갱신
        }
    }

    void Fire()
    {
        // 2. 총알을 firePoint 위치에서 생성 (회전 없음)
        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    }
}
