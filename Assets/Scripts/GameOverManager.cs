using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// 게임 오버 UI를 페이드 인으로 보여주고,
/// 버튼을 통해 다시하기/메인 메뉴 기능을 제공.
/// </summary>
public class GameOverManager : MonoBehaviour
{
    [Header("게임 오버 UI")]
    public GameObject gameOverPanel;       // 게임 오버 UI 패널
    public CanvasGroup canvasGroup;        // 페이드 인/아웃용 컴포넌트
    public float fadeDuration = 1.0f;      // 페이드 인 시간 (초)

    void Start()
    {
        // 시작 시 꺼진 상태로 설정
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
    }

    /// <summary>
    /// 게임 오버 처리: 패널을 페이드 인으로 보여줌
    /// </summary>
    public void ShowGameOver()
    {
        Debug.Log("🛑 게임 오버: 페이드 인 시작");

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (canvasGroup != null)
        {
            StartCoroutine(FadeIn());
        }

        Time.timeScale = 0f;
    }

    private IEnumerator FadeIn()
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime; // 일시정지 상태에서도 페이드 작동
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f; // 확실하게 완성
    }

    /// <summary>
    /// 다시 시작 - 현재 씬 리로드
    /// </summary>
    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// 메인 메뉴로 이동
    /// </summary>
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
