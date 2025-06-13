using UnityEngine;

public class ItemSpawner2D : MonoBehaviour
{
    [Header("아이템 설정")]
    public GameObject itemPrefab;        // 생성할 아이템 프리팹

    [Header("스폰 주기")]
    public float spawnInterval = 5f;     // 아이템 생성 간격 (초)

    [Header("생성 위치 범위")]
    public float minX = -8f;             // X 최소값
    public float maxX = 8f;              // X 최대값
    public float minY = -4f;             // Y 최소값
    public float maxY = 4f;              // Y 최대값

    private float timer;

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnItem();
            timer = spawnInterval;
        }
    }

    void SpawnItem()
    {
        // X, Y 좌표를 범위 내에서 랜덤하게 지정
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        Vector2 spawnPos = new Vector2(randomX, randomY);

        // 아이템 생성
        Instantiate(itemPrefab, spawnPos, Quaternion.identity);
    }
}
