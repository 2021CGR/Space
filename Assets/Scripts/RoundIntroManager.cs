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

        Debug.Log("🎮 게임 시작!");
    }
}
