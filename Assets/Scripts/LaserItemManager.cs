using UnityEngine;

public class LaserItemManager : MonoBehaviour
{
    [Header("레이저 아이템 설정")]
    public GameObject laserItemPrefab;     // 번개(레이저) 아이템 프리팹
    public int killsRequired = 5;          // 필요한 적 처치 수

    [Header("생성 위치 범위")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4f;
    public float maxY = 4f;

    private int currentKills = 0;

    void Start()
    {
        // GameManager에 이벤트 등록
        var gm = GameManager.instance;
        if (gm != null)
        {
            gm.onEnemyKilled += OnEnemyKilled;
        }
        else
        {
            Debug.LogWarning("GameManager.instance를 찾을 수 없어 레이저 아이템 생성이 작동하지 않을 수 있습니다.");
        }
    }

    void OnDestroy()
    {
        // 이벤트 해제
        var gm = GameManager.instance;
        if (gm != null)
        {
            gm.onEnemyKilled -= OnEnemyKilled;
        }
    }

    void OnEnemyKilled()
    {
        currentKills++;
        if (currentKills >= killsRequired)
        {
            SpawnLaserItem();
            currentKills = 0; // 카운트 리셋
        }
    }

    void SpawnLaserItem()
    {
        if (laserItemPrefab == null)
        {
            Debug.LogWarning("레이저 아이템 프리팹이 설정되지 않았습니다.");
            return;
        }

        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        Vector2 spawnPos = new Vector2(x, y);

        Instantiate(laserItemPrefab, spawnPos, Quaternion.identity);
        Debug.Log("⚡ 레이저(번개) 아이템 생성!");
    }
}