using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("🎵 UI 사운드 설정")]
    public AudioClip hoverClip;         // 마우스 오버 사운드
    public AudioClip clickClip;         // 클릭 사운드
    public AudioSource uiAudioSource;   // UI 전용 오디오 소스

    public static UIManager Instance { get; private set; }

    void Awake()
    {
        // 싱글톤 중복 방지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // 🎤 Hover 사운드 재생
    public void PlayHoverSound()
    {
        if (hoverClip != null && uiAudioSource != null)
        {
            uiAudioSource.PlayOneShot(hoverClip);
        }
    }

    // 🖱️ 클릭 사운드 재생
    public void PlayClickSound()
    {
        if (clickClip != null && uiAudioSource != null)
        {
            uiAudioSource.PlayOneShot(clickClip);
        }
    }
}
