using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// 게임 클리어 시 페이드 인 UI를 표시하고 커서 및 시간을 관리하는 매니저입니다.
/// </summary>
[RequireComponent(typeof(CanvasGroup))] // [추가] 이 스크립트는 CanvasGroup이 필수
public class ClearUIManager : MonoBehaviour
{
    [Header("🎉 클리어 UI")]
    [Tooltip("클리어 패널 게임 오브젝트")]
    public GameObject clearPanel;
    [Tooltip("페이드 효과를 위한 캔버스 그룹 (자동 할당 시도)")]
    public CanvasGroup canvasGroup;
    [Tooltip("페이드 인이 완료되기까지 걸리는 시간 (초)")]
    public float fadeDuration = 1f;

    void Start()
    {
        // [추가] CanvasGroup이 할당되지 않았다면 자동으로 찾아 할당
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        // [수정] 처음엔 클리어 패널 숨기기 및 알파값 0
        if (clearPanel != null)
            clearPanel.SetActive(false);

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }

    /// <summary>
    /// 외부에서 호출하면 클리어 UI가 페이드인으로 나타납니다.
    /// </summary>
    public void ShowClear()
    {
        if (canvasGroup == null)
        {
            Debug.LogWarning("❗ CanvasGroup이 설정되지 않았어요. 페이드 효과 없이 패널만 켭니다.");
            if (clearPanel != null)
                clearPanel.SetActive(true);
            return;
        }

        // [추가] 패널 활성화 (코루틴 시작 전)
        if (clearPanel != null)
            clearPanel.SetActive(true);

        Debug.Log("🎊 클리어 UI 페이드인 시작!");
        StartCoroutine(FadeInCanvasGroup());

        // [추가] 게임 시간 정지 (UI 애니메이션은 Time.unscaledDeltaTime으로 계속됨)
        Time.timeScale = 0f;

        // [추가] 클리어 시 마우스 커서 보이기
        CursorManager.Instance?.SetCursorVisible(true);
    }

    /// <summary>
    /// 메인 메뉴로 돌아가는 버튼 함수입니다.
    /// </summary>
    public void GoToMainMenu()
    {
        // [추가] 씬 이동 전 TimeScale 원상복구
        Time.timeScale = 1f;

        // [추가] 메인 메뉴로 갈 때 마우스 커서 보이게
        CursorManager.Instance?.SetCursorVisible(true);

        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// CanvasGroup의 알파를 서서히 올려서 페이드 인 시키는 코루틴
    /// </summary>
    IEnumerator FadeInCanvasGroup()
    {
        float elapsed = 0f;
        canvasGroup.alpha = 0f; // 시작 알파값 확실히 0으로

        while (elapsed < fadeDuration)
        {
            // [수정] Time.timeScale에 영향을 받지 않는 unscaledDeltaTime 사용
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration); // 0과 1 사이 값으로 고정
            yield return null; // 다음 프레임까지 대기
        }

        canvasGroup.alpha = 1f; // 페이드 인 완료
    }
}