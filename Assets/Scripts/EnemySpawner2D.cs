using UnityEngine;

/// <summary>
/// 2D 환경에서 화면 오른쪽에 적을 주기적으로 소환합니다.
/// </summary>
public class EnemySpawner2D : MonoBehaviour
{
    [Header("소환 설정")]
    [Tooltip("소환할 적 프리팹 배열 (이 중 하나가 랜덤 선택됨)")]
    public GameObject[] enemyPrefabs;
    [Tooltip("소환 간격 (초)")]
    public float spawnInterval = 2f;
    [Tooltip("이 스포너가 소환할 총 적의 수")]
    public int maxSpawnCount = 10;

    [Header("위치 범위")]
    [Tooltip("소환 가능한 Y 최소값")]
    public float minY = -4f;
    [Tooltip("소환 가능한 Y 최대값")]
    public float maxY = 4f;
    [Tooltip("적이 소환될 X 좌표 (보통 화면 밖 오른쪽)")]
    public float spawnX = 10f;

    private float timer;                // 다음 소환까지 남은 시간
    private int spawnedCount = 0;       // 지금까지 소환된 적 수

    void Start()
    {
        // [추가] 타이머 초기화 (시작하자마자 소환될 수 있도록 0으로 설정 가능)
        timer = spawnInterval;
    }

    void Update()
    {
        // [수정] 최대 소환 수에 도달했거나, 소환할 프리팹이 없으면 중지
        if (spawnedCount >= maxSpawnCount || enemyPrefabs.Length == 0)
            return;

        timer -= Time.deltaTime;

        // 타이머가 0 이하가 되면 적 소환
        if (timer <= 0f)
        {
            SpawnEnemy();
            timer = spawnInterval; // 타이머 초기화
        }
    }

    void SpawnEnemy()
    {
        // 1. Y 위치 랜덤 설정
        float randomY = Random.Range(minY, maxY);
        Vector2 spawnPos = new Vector2(spawnX, randomY);

        // 2. enemyPrefabs 배열 중에서 랜덤으로 하나 선택
        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject selectedEnemy = enemyPrefabs[index];

        // 3. 적 소환
        if (selectedEnemy != null) // [추가] 프리팹이 null이 아닌지 확인
        {
            Instantiate(selectedEnemy, spawnPos, Quaternion.identity);
            spawnedCount++; // 소환 수 증가
            Debug.Log($"적 소환됨: {selectedEnemy.name} (총 {spawnedCount}/{maxSpawnCount} 마리)");
        }
    }
}