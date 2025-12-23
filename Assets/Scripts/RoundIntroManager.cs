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
        // [수정] 코루틴으로 시작하여 스토리 매니저 대기 로직 추가
        StartCoroutine(StartSequence());
    }

    private IEnumerator StartSequence()
    {
        // 1. 스토리 인트로가 실행 중이라면 끝날 때까지 대기
        // (StoryIntroManager 클래스가 존재하고, IsPlaying이 true인 동안 대기)
        if (StoryIntroManager.IsPlaying)
        {
            // 스토리가 진행되는 동안 게임 정지 유지 (이미 StoryIntroManager가 정지시켰겠지만 확실하게)
            Time.timeScale = 0f;
            
            // 스토리가 끝날 때까지 대기
            yield return new WaitWhile(() => StoryIntroManager.IsPlaying);
        }

        // 2. 라운드 인트로 시작
        if (freezeGameAtStart)
        {
            Time.timeScale = 0f; // 게임 정지 (페이드 동안)
        }

        // [추가] 라운드 시작 시 게임 BGM 재생 (스토리가 끝나고 여기서 재생됨)
        if (BGMManager.Instance != null)
        {
            BGMManager.Instance.PlayBGM(BGMType.InGame);
            Debug.Log("🔊 Round 시작: 게임 BGM 재생");
        }

        if (roundPanel != null)
        {
            roundPanel.alpha = 0f;
            roundPanel.gameObject.SetActive(true);
            yield return StartCoroutine(ShowRoundIntro());
        }
        else
        {
            StartGameNow();
        }
    }

    // [기존 StartCoroutine(ShowRoundIntro()) 호출 방식 변경에 따라 수정]
    // Start() 함수가 IEnumerator StartSequence()로 대체되었으므로 
    // 기존의 void Start() 내용은 위 코드로 통합되었습니다.

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