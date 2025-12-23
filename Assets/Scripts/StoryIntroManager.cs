using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class StoryIntroManager : MonoBehaviour
{
    public CanvasGroup panel;
    public Image image;
    public Sprite[] frames;
    public float fadeIn = 0.5f;
    public float fadeOut = 0.4f;
    public float hold = 1.5f;
    public bool freezeGameplay = true;
    public bool clickToAdvance = true;
    public KeyCode advanceKey = KeyCode.Space;
    public KeyCode skipKey = KeyCode.Escape;

    [Header("🎵 스토리 BGM")]
    public AudioClip storyBGM;     // 스토리 진행 중 재생할 BGM
    private AudioSource audioSource;

    public static bool IsPlaying { get; private set; } 

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log($"🎬 StoryIntroManager 시작됨. 현재 씬: {sceneName}");

        // [안전장치] 메인 메뉴에서는 실행 금지
        if (sceneName == "MainMenu")
        {
            Debug.Log("🚫 현재 씬이 MainMenu이므로 스토리를 비활성화합니다.");
            gameObject.SetActive(false); 
            return;
        }

        // [추가] 재시작(Retry) 모드라면 스토리를 건너뜀
        if (GameManager.isRetry)
        {
            Debug.Log("🔁 재시작(Retry) 상태이므로 스토리를 건너뜁니다.");
            GameManager.isRetry = false; // 플래그 초기화
            gameObject.SetActive(false); // 스토리 오브젝트 끄기
            IsPlaying = false; // 확실하게 false 처리
            return;
        }

        if (panel == null) { Debug.LogError("❌ StoryIntroManager: Panel이 연결되지 않음!"); return; }
        if (image == null) { Debug.LogError("❌ StoryIntroManager: Image가 연결되지 않음!"); return; }
        if (frames == null || frames.Length == 0) { Debug.LogError("❌ StoryIntroManager: Frames(이미지)가 비어있음!"); return; }

        // 오디오 소스 설정
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true; // 반복 재생
        audioSource.playOnAwake = false;

        Debug.Log("✅ 스토리 코루틴 시작!");
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        IsPlaying = true; 

        // 🎵 1. 기존 게임 BGM 확실하게 끄기
        if (BGMManager.Instance != null && BGMManager.Instance.bgmSource != null)
        {
            BGMManager.Instance.bgmSource.Stop();
            Debug.Log("🔇 스토리 시작: 기존 게임 BGM 중지됨");
        }

        // 🎵 2. 스토리 전용 BGM 재생
        if (storyBGM != null)
        {
            audioSource.clip = storyBGM;
            audioSource.Play();
            Debug.Log("🎵 스토리 BGM 재생 시작");
        }

        if (freezeGameplay) Time.timeScale = 0f;
        if (CursorManager.Instance != null) CursorManager.Instance.SetCursorVisible(true);
        panel.gameObject.SetActive(true);
        
        for (int i = 0; i < frames.Length; i++)
        {
            image.sprite = frames[i];
            panel.alpha = 0f;
            float t = 0f;
            while (t < fadeIn)
            {
                t += Time.unscaledDeltaTime;
                panel.alpha = Mathf.Lerp(0f, 1f, t / fadeIn);
                yield return null;
            }
            float elapsed = 0f;
            while (elapsed < hold)
            {
                if (clickToAdvance && (Input.GetKeyDown(advanceKey) || Input.GetMouseButtonDown(0))) break;
                if (Input.GetKeyDown(skipKey)) { i = frames.Length - 1; break; }
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
            t = 0f;
            while (t < fadeOut)
            {
                t += Time.unscaledDeltaTime;
                panel.alpha = Mathf.Lerp(1f, 0f, t / fadeOut);
                yield return null;
            }
        }
        panel.gameObject.SetActive(false);
        
        // 🎵 3. 스토리 BGM 끄고 게임 BGM 다시 재생
        if (storyBGM != null)
        {
            audioSource.Stop();
        }

        // [수정] 스토리가 끝나면 BGM을 바로 켜지 않음 (RoundIntroManager가 켤 것임)
        // 만약 RoundIntroManager가 없다면 여기서 켜야 하지만, 
        // 사용자 요청에 따라 "Round가 뜨기 시작하면 다시 사운드가 나오게" 하기 위해 여기서는 켭니다.
        // 아니, 사용자 요청은 "Round가 뜨기 시작하면"이므로 여기서는 켜지 않고 넘깁니다.
        Debug.Log("� 스토리 종료: 게임 BGM 대기 중 (Round 시작 시 재생됨)");

        IsPlaying = false;
        Debug.Log("🎬 스토리 종료됨.");

        if (freezeGameplay) Time.timeScale = 1f;
        if (CursorManager.Instance != null) CursorManager.Instance.SetCursorVisible(false);
    }
}
