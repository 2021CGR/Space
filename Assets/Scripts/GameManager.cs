using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro 사용을 위해 추가

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // [추가] 재시작(Retry) 상태인지 확인하는 정적 변수
    public static bool isRetry = false;

    [Header("적 정보")]
    public int totalEnemies = 10;       // 스테이지에 등장하는 총 적 수
    private int killedEnemies = 0;      // 현재까지 죽은 적 수

    [Header("다음 씬 설정")]
    public string nextSceneName = "Stage2"; // 다음으로 이동할 씬 이름

    [Header("UI 설정")]
    [Tooltip("남은 적 수를 표시할 TextMeshPro UI 오브젝트를 연결하세요.")]
    public TextMeshProUGUI enemyCountText; 

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
        // [수정] 스토리가 진행 중이 아닐 때만 재생 (스토리 중에는 StoryIntroManager가 음악을 관리함)
        if (!StoryIntroManager.IsPlaying)
        {
            BGMManager.Instance?.PlayBGM(BGMType.InGame);
        }
        else
        {
            Debug.Log("🤫 스토리가 진행 중이므로 GameManager에서 BGM을 재생하지 않습니다.");
        }

        // 🖱️ 게임 시작 시 마우스 커서 숨기기 (처음 진입 시에도 확실히)
        CursorManager.Instance.SetCursorVisible(false);

        // 시작 시 UI 초기화 (남은 적 수 표시)
        UpdateEnemyCountUI();
    }

    /// <summary>
    /// 적이 죽었을 때 호출됨
    /// </summary>
    public void OnEnemyKilled()
    {
        killedEnemies++;
        Debug.Log($"적 처치 수: {killedEnemies}/{totalEnemies}");

        // 적 처치 시 UI 갱신
        UpdateEnemyCountUI();

        // 이벤트 호출
        onEnemyKilled?.Invoke();

        if (killedEnemies >= totalEnemies)
        {
            Debug.Log("✅ 적 전멸! 다음 씬으로 이동합니다.");
            SceneManager.LoadScene(nextSceneName);
        }
    }

    /// <summary>
    /// 남은 적 수를 계산하여 UI에 표시하는 함수
    /// </summary>
    private void UpdateEnemyCountUI()
    {
        if (enemyCountText != null)
        {
            // 남은 적 계산 (전체 - 처치한 수)
            int remaining = totalEnemies - killedEnemies;
            
            // 0보다 작아지지 않게 방어 코드
            if (remaining < 0) remaining = 0;

            // 텍스트 갱신 (예: 10, 9, 8...)
            enemyCountText.text = remaining.ToString();
        }
    }
}
