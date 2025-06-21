using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// 게임 클리어 시 페이드 인 UI를 표시하는 매니저야!
/// </summary>
public class ClearUIManager : MonoBehaviour
{
    [Header("🎉 클리어 UI")]
    public GameObject clearPanel;            // 클리어 패널 오브젝트
    public CanvasGroup canvasGroup;          // 페이드 효과용 캔버스 그룹
    public float fadeDuration = 1f;          // 페이드 인 지속 시간 (초)

    void Start()
    {
        // 처음엔 클리어 패널 숨기기
        if (clearPanel != null)
            clearPanel.SetActive(false);

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }

    /// <summary>
    /// 외부에서 호출하면 클리어 UI가 페이드인으로 나타나
    /// </summary>
    public void ShowClear()
    {
        if (clearPanel != null)
            clearPanel.SetActive(true);

        if (canvasGroup != null)
        {
            StartCoroutine(FadeInCanvasGroup());
            Time.timeScale = 0f;
            Debug.Log("🎊 클리어 UI 페이드인 시작!");
        }
        else
        {
            Debug.LogWarning("❗ CanvasGroup이 설정되지 않았어요.");
        }

        // 🟢 클리어 시 마우스 커서 보이기
        CursorManager.Instance.SetCursorVisible(true);
    }

    /// <summary>
    /// 메인 메뉴로 돌아가는 버튼 함수야
    /// </summary>
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        // 🟢 메인 메뉴로 갈 때 마우스 커서 보이게
        CursorManager.Instance.SetCursorVisible(true);

        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// CanvasGroup의 알파를 서서히 올려서 페이드 인 시키는 코루틴
    /// </summary>
    IEnumerator FadeInCanvasGroup()
    {
        float elapsed = 0f;
        canvasGroup.alpha = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}
