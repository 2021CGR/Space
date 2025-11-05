using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 게임 시작 시 Round 텍스트를 페이드 인/아웃으로 보여준 후 실제 게임을 시작하는 매니저
/// </summary>
public class RoundIntroManager : MonoBehaviour
{
    [Header("페이드 설정")]
    public CanvasGroup roundPanel;      // Round 텍스트가 있는 패널
    public float fadeDuration = 1f;     // 페이드 인/아웃 시간
    public float showDuration = 1f;     // 가운데에 유지되는 시간

    [Header("게임 시작 타이밍")]
    public bool freezeGameAtStart = true; // true면 시작 전 게임 정지

    void Start()
    {
        if (freezeGameAtStart)
        {
            Time.timeScale = 0f; // 게임 정지 (페이드 동안)
        }

        if (roundPanel != null)
        {
            roundPanel.alpha = 0f;
            roundPanel.gameObject.SetActive(true);
            StartCoroutine(ShowRoundIntro());
        }
        else
        {
            // [추가됨] 만약 라운드 패널이 없다면, 바로 게임 시작 및 커서 숨김 처리
            StartGameNow();
        }
    }

    private IEnumerator ShowRoundIntro()
    {
        // 페이드 인
        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            roundPanel.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            yield return null;
        }

        // 잠시 유지
        roundPanel.alpha = 1f;
        yield return new WaitForSecondsRealtime(showDuration);

        // 페이드 아웃
        time = 0f;
        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            roundPanel.alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            yield return null;
        }

        // 게임 시작
        roundPanel.gameObject.SetActive(false);

        if (freezeGameAtStart)
        {
            Time.timeScale = 1f;
        }

        // ────── ✨ 여기가 추가된 부분입니다! ──────
        // 게임 플레이가 시작되는 시점이므로 커서를 숨기고 잠급니다.
        // 이것이 빌드에서 커서가 중앙에 고정되는 문제를 해결합니다.
        if (CursorManager.Instance != null)
        {
            CursorManager.Instance.SetCursorVisible(false);
        }
        else
        {
            Debug.LogWarning("CursorManager가 없습니다! 커서를 직접 제어합니다.");
            // CursorManager가 로드되지 않은 경우를 대비한 예외 처리
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        // ──────────────────────────────────────────

        Debug.Log("🎮 게임 시작!");
    }

    /// <summary>
    /// [추가됨] 라운드 인트로 없이 바로 게임을 시작해야 할 때 호출되는 함수
    /// </summary>
    private void StartGameNow()
    {
        Debug.Log("🎮 라운드 인트로 없이 바로 게임 시작!");

        if (freezeGameAtStart)
        {
            Time.timeScale = 1f; // 혹시 모르니 타임스케일 복구
        }

        // 게임 플레이가 시작되는 시점이므로 커서를 숨기고 잠급니다.
        if (CursorManager.Instance != null)
        {
            CursorManager.Instance.SetCursorVisible(false);
        }
        else
        {
            Debug.LogWarning("CursorManager가 없습니다! 커서를 직접 제어합니다.");
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}