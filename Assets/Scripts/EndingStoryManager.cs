using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndingStoryManager : MonoBehaviour
{
    public CanvasGroup panel;
    public Image image;
    public Sprite[] frames;
    public float fadeIn = 0.5f;
    public float fadeOut = 0.4f;
    public float hold = 1.5f;
    public bool clickToAdvance = true;
    public KeyCode advanceKey = KeyCode.Space;
    public KeyCode skipKey = KeyCode.Escape;

    [Header("🎵 엔딩 BGM")]
    public AudioClip endingBGM;
    private AudioSource audioSource;

    public static bool IsPlaying { get; private set; }

    void Start()
    {
        // 시작 시에는 꺼져 있어야 함 (BossGameManager가 켜줄 것임)
        // 하지만 실수로 켜놨을 경우를 대비해, 패널만 숨김
        if (panel != null) panel.gameObject.SetActive(false);
        
        // 오디오 소스 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    /// <summary>
    /// 외부(BossGameManager)에서 호출하여 엔딩 시작
    /// </summary>
    public void StartEnding()
    {
        if (panel == null || image == null || frames == null || frames.Length == 0)
        {
            Debug.LogWarning("❌ EndingStoryManager: 설정이 부족하여 엔딩을 생략하고 바로 클리어 화면으로 갑니다.");
            FinishEnding();
            return;
        }

        gameObject.SetActive(true);
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        IsPlaying = true;
        Debug.Log("🎬 엔딩 스토리 시작!");

        // 게임 정지 (연출 동안)
        Time.timeScale = 0f;
        
        // 커서 숨김 (엔딩 감상)
        if (CursorManager.Instance != null) CursorManager.Instance.SetCursorVisible(false);

        // 🎵 엔딩 BGM 재생
        if (endingBGM != null)
        {
            // 기존 BGM 끄기
            if (BGMManager.Instance != null && BGMManager.Instance.bgmSource != null)
                BGMManager.Instance.bgmSource.Stop();

            audioSource.clip = endingBGM;
            audioSource.Play();
        }

        panel.gameObject.SetActive(true);

        for (int i = 0; i < frames.Length; i++)
        {
            image.sprite = frames[i];
            panel.alpha = 0f;
            
            // 페이드 인 (Realtime 사용)
            float t = 0f;
            while (t < fadeIn)
            {
                t += Time.unscaledDeltaTime;
                panel.alpha = Mathf.Lerp(0f, 1f, t / fadeIn);
                yield return null;
            }

            // 대기 (클릭/키 입력으로 넘기기)
            float elapsed = 0f;
            while (elapsed < hold)
            {
                if (clickToAdvance && (Input.GetKeyDown(advanceKey) || Input.GetMouseButtonDown(0))) break;
                if (Input.GetKeyDown(skipKey)) { i = frames.Length - 1; break; } // 스킵 시 마지막으로
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            // 페이드 아웃
            t = 0f;
            while (t < fadeOut)
            {
                t += Time.unscaledDeltaTime;
                panel.alpha = Mathf.Lerp(1f, 0f, t / fadeOut);
                yield return null;
            }
        }

        panel.gameObject.SetActive(false);
        
        // 엔딩 종료 처리
        FinishEnding();
    }

    void FinishEnding()
    {
        IsPlaying = false;
        
        // BGM 정리
        if (endingBGM != null) audioSource.Stop();

        // 게임 클리어 화면 호출
        if (BossGameManager.instance != null)
        {
            BossGameManager.instance.ShowClearUI();
        }
        else
        {
            // 혹시 BossGameManager가 없으면 직접 찾아서 호출
            ClearUIManager clearUI = FindObjectOfType<ClearUIManager>();
            if (clearUI != null) clearUI.ShowClear();
        }

        Debug.Log("🎬 엔딩 스토리 종료 -> 클리어 화면 전환");
    }
}
