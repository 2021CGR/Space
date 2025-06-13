using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("적 정보")]
    public int totalEnemies = 10;       // 스테이지에 등장하는 총 적 수
    private int killedEnemies = 0;      // 현재까지 죽은 적 수

    [Header("다음 씬 설정")]
    public string nextSceneName = "Stage2"; // 다음으로 이동할 씬 이름

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void OnEnemyKilled()
    {
        killedEnemies++;
        Debug.Log($"적 처치 수: {killedEnemies}/{totalEnemies}");

        if (killedEnemies >= totalEnemies)
        {
            Debug.Log("✅ 적 전멸! 다음 씬으로 이동합니다.");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
