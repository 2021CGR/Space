using UnityEngine;

public class ItemSpawner2D : MonoBehaviour
{
    [Header("아이템 설정")]
    public GameObject energyItemPrefab;    // 에너지(번개) 아이템 프리팹
    public GameObject dualShotItemPrefab; // 듀얼샷 아이템 프리팹

    [Header("스폰 확률")]
    [Range(0f, 1f)]
    public float dualShotChance = 0.3f;  // 듀얼샷 아이템 생성 확률

    [Header("생성 위치 범위")]
    public float minX = -8f;             // X 최소값
    public float maxX = 8f;              // X 최대값
    public float minY = -4f;             // Y 최소값
    public float maxY = 4f;              // Y 최대값

    [Header("스폰 설정")]
    public int killsRequired = 5;        // 적 처치 수에 따라 에너지 아이템 생성

    private int currentKillCount = 0;

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

    // 주기 스폰 제거됨

    void OnEnemyKilled()
    {
        if (energyItemPrefab == null) return;
        currentKillCount++;
        if (currentKillCount >= killsRequired)
        {
            SpawnEnergyItem();
            currentKillCount = 0;
        }
    }

    void SpawnEnergyItem()
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        Vector2 spawnPos = new Vector2(x, y);
        Instantiate(energyItemPrefab, spawnPos, Quaternion.identity);
        Debug.Log("⚡ 에너지(번개) 아이템 생성!");
    }

    // 듀얼샷/에너지 주기 스폰 제거됨
}
