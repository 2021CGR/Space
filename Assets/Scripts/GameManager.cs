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

    // 적 처치 이벤트
    public System.Action onEnemyKilled;

    void Awake()
    {
        // 싱글톤 설정
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // 🎵 인게임 배경음 재생
        BGMManager.Instance?.PlayBGM(BGMType.InGame);

        // 🖱️ 게임 시작 시 마우스 커서 숨기기 (처음 진입 시에도 확실히)
        CursorManager.Instance.SetCursorVisible(false);
    }

    /// <summary>
    /// 적이 죽었을 때 호출됨
    /// </summary>
    public void OnEnemyKilled()
    {
        killedEnemies++;
        Debug.Log($"적 처치 수: {killedEnemies}/{totalEnemies}");

        // 이벤트 호출
        onEnemyKilled?.Invoke();

        if (killedEnemies >= totalEnemies)
        {
            Debug.Log("✅ 적 전멸! 다음 씬으로 이동합니다.");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}