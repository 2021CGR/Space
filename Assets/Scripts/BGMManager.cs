using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// 🎵 BGM과 SFX를 중앙에서 재생하고 관리하는 사운드 매니저
/// </summary>
public enum BGMType { MainMenu, InGame }

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    [Header("🎵 BGM 오디오 클립들")]
    public AudioClip mainMenuClip;
    public AudioClip inGameClip;

    [Header("🔊 Audio 연결")]
    public AudioSource bgmSource;  // BGM 재생용 AudioSource (Output: BGM)
    public AudioSource sfxSource;  // SFX 재생용 AudioSource (Output: SFX)
    public AudioMixer audioMixer; // Mixer 연결용 (선택사항)

    private void Awake()
    {
        // Singleton 패턴 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Debug.Log("✅ BGMManager 초기화 완료");
    }

    /// <summary>
    /// 🎵 지정된 타입의 BGM을 재생한다
    /// </summary>
    public void PlayBGM(BGMType type)
    {
        if (bgmSource == null)
        {
            Debug.LogWarning("⚠️ BGM Source가 null입니다!");
            return;
        }

        AudioClip selectedClip = type == BGMType.MainMenu ? mainMenuClip : inGameClip;

        if (selectedClip == null)
        {
            Debug.LogWarning("⚠️ 선택된 BGM 오디오 클립이 null입니다!");
            return;
        }

        bgmSource.clip = selectedClip;
        bgmSource.loop = true;
        bgmSource.Play();

        Debug.Log($"▶️ BGM 재생 시작됨: {selectedClip.name}");
    }

    /// <summary>
    /// 🔊 효과음을 재생한다 (SFX Source를 통해)
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null)
        {
            Debug.LogWarning("⚠️ SFX 재생 실패 (클립 또는 AudioSource가 null)");
            return;
        }

        sfxSource.PlayOneShot(clip);
        Debug.Log($"🔊 재생되는 효과음: {clip.name}");
    }
}

