using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// 게임 오버 시 페이드 인 UI를 표시하고 커서 상태를 관리하는 매니저야!
/// </summary>
public class GameOverManager : MonoBehaviour
{
    [Header("🚩 게임 오버 UI")]
    public GameObject gameOverPanel;        // 게임 오버 패널 오브젝트
    public CanvasGroup canvasGroup;         // 페이드 효과용 캔버스 그룹
    public float fadeDuration = 1.0f;       // 페이드 인 지속 시간

    void Start()
    {
        // 시작 시 UI를 비활성화하고 알파값을 0으로 설정
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }

    /// <summary>
    /// 외부에서 호출해서 게임 오버 UI를 표시할 수 있어
    /// </summary>
    public void ShowGameOver()
    {
        Debug.Log("🚩 게임 오버 발생! UI 페이드 인 시작");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (canvasGroup != null)
            StartCoroutine(FadeIn());

        Time.timeScale = 0f;

        // 🔹 마우스 커서 보이기
        CursorManager.Instance.SetCursorVisible(true);
    }

    /// <summary>
    /// 게임 오버 UI를 천천히 나타나게 하는 코루틴
    /// </summary>
    private IEnumerator FadeIn()
    {
        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    /// <summary>
    /// 다시 시작 버튼 눌렀을 때 호출됨
    /// </summary>
    public void Retry()
    {
        Time.timeScale = 1f;

        // [추가] 재시작 모드 활성화 (스토리 스킵용)
        GameManager.isRetry = true;

        // ❌ 커서 다시 숨김
        CursorManager.Instance.SetCursorVisible(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// 메인 메뉴로 이동할 때 호출됨
    /// </summary>
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        // [추가] 메인 메뉴로 갈 때는 재시작 모드 해제
        GameManager.isRetry = false;

        // ✅ 마우스 커서 보이게
        CursorManager.Instance.SetCursorVisible(true);

        SceneManager.LoadScene("MainMenu");
    }
}
