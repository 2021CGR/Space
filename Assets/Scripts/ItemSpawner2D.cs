using UnityEngine;

public class ItemSpawner2D : MonoBehaviour
{
    [Header("아이템 설정")]
    public GameObject energyItemPrefab;    // 에너지(번개) 아이템 프리팹
    public GameObject dualShotItemPrefab; // 듀얼샷 아이템 프리팹

    [Header("생성 위치 범위")]
    public float minX = -8f;             // X 최소값
    public float maxX = 8f;              // X 최대값
    public float minY = -4f;             // Y 최소값
    public float maxY = 4f;              // Y 최대값

    [Header("스폰 시간 설정")]
    public float minSpawnTime = 5f;      // 최소 스폰 시간
    public float maxSpawnTime = 10f;     // 최대 스폰 시간

    private float spawnTimer;

    void Start()
    {
        // 첫 스폰 타이머 설정
        SetNextSpawnTime();
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnRandomItem();
            SetNextSpawnTime();
        }
    }

    void SetNextSpawnTime()
    {
        spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void SpawnRandomItem()
    {
        // 위치 설정
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        Vector2 spawnPos = new Vector2(x, y);

        // 아이템 결정 (50% 확률로 랜덤)
        GameObject itemToSpawn;

        // 0 또는 1이 랜덤으로 나옴 (0: 듀얼샷, 1: 에너지)
        int randomSelect = Random.Range(0, 2);
        
        if (randomSelect == 0)
        {
            itemToSpawn = dualShotItemPrefab;
            // 만약 듀얼샷 프리팹이 비어있다면 에너지 아이템으로 대체
            if (itemToSpawn == null) itemToSpawn = energyItemPrefab;
            else Debug.Log("🔫 듀얼샷 아이템 생성!");
        }
        else
        {
            itemToSpawn = energyItemPrefab;
            Debug.Log("⚡ 에너지(번개) 아이템 생성!");
        }

        if (itemToSpawn != null)
        {
            Instantiate(itemToSpawn, spawnPos, Quaternion.identity);
        }
    }

    // 듀얼샷/에너지 주기 스폰 제거됨
}
