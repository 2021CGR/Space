using UnityEngine;

public class EnemySpawner2D : MonoBehaviour
{
    [Header("소환 설정")]
    public GameObject[] enemyPrefabs;     // 여러 적 프리팹을 담을 배열
    public float spawnInterval = 2f;      // 소환 간격 (초)
    public int maxSpawnCount = 10;        // 총 소환할 적 수

    [Header("위치 범위")]
    public float minY = -4f;              // 소환 가능한 Y 최소값
    public float maxY = 4f;               // 소환 가능한 Y 최대값
    public float spawnX = 10f;            // 소환 위치 X (화면 오른쪽)

    private float timer;
    private int spawnedCount = 0;         // 지금까지 소환된 적 수

    void Update()
    {
        // 최대 소환 수 도달 시 소환 중지
        if (spawnedCount >= maxSpawnCount)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnEnemy();
            timer = spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        // Y 위치 랜덤 설정
        float randomY = Random.Range(minY, maxY);
        Vector2 spawnPos = new Vector2(spawnX, randomY);

        // enemyPrefabs 배열 중에서 랜덤으로 하나 선택
        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject selectedEnemy = enemyPrefabs[index];

        // 적 소환
        Instantiate(selectedEnemy, spawnPos, Quaternion.identity);
        spawnedCount++; // 소환 수 증가
    }
}
