using UnityEngine;

/// <summary>
/// 게임 시작 시 플레이어를 리스폰 위치로 이동시키는 스크립트.
/// 씬에 미리 존재하는 플레이어에게 적용됨.
/// </summary>
public class PlayerSpawner : MonoBehaviour
{
    [Header("리스폰 위치 연결")]
    public Transform spawnPoint; // 리스폰 위치 오브젝트

    void Start()
    {
        // "Player" 태그가 붙은 오브젝트를 찾아서
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && spawnPoint != null)
        {
            // 해당 위치로 이동시킴
            player.transform.position = spawnPoint.position;
            Debug.Log("✅ 플레이어가 리스폰 위치로 이동함");
        }
        else
        {
            Debug.LogWarning("❌ Player 오브젝트 또는 리스폰 위치가 연결되지 않았습니다.");
        }
    }
}
