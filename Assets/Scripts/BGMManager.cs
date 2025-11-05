using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// 🎵 BGM과 SFX를 중앙에서 재생하고 관리하는 사운드 매니저 (싱글톤, 씬 전환 시 유지)
/// </summary>
public enum BGMType { MainMenu, InGame } // [추가] BGM 종류를 쉽게 구분하기 위한 열거형

public class BGMManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static BGMManager Instance;

    [Header("🎵 BGM 오디오 클립들")]
    [Tooltip("메인 메뉴 BGM")]
    public AudioClip mainMenuClip;
    [Tooltip("인게임(플레이) BGM")]
    public AudioClip inGameClip;

    [Header("🔊 Audio 연결")]
    [Tooltip("BGM 재생용 AudioSource (출력: BGM Mixer Group)")]
    public AudioSource bgmSource;
    [Tooltip("SFX 재생용 AudioSource (출력: SFX Mixer Group)")]
    public AudioSource sfxSource;
    [Tooltip("볼륨 조절에 사용할 Audio Mixer")]
    public AudioMixer audioMixer; // (현재 코드에서는 직접 사용 X, SettingsManager가 사용)

    private void Awake()
    {
        // [수정] 싱글톤 설정 (중복 방지 및 씬 전환 시 유지)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 이 오브젝트를 씬 전환 시 파괴하지 않음
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 중복 생성 방지를 위해 자신을 파괴
            return;
        }

        // [추가] AudioSource가 할당되지 않았다면 경고
        if (bgmSource == null) Debug.LogWarning("BGM Source가 연결되지 않았습니다.");
        if (sfxSource == null) Debug.LogWarning("SFX Source가 연결되지 않았습니다.");
    }

    /// <summary>
    /// 🎵 지정된 타입의 BGM을 재생합니다.
    /// </summary>
    public void PlayBGM(BGMType type)
    {
        if (bgmSource == null)
        {
            Debug.LogWarning("⚠️ BGM Source가 null입니다!");
            return;
        }

        // [수정] 삼항 연산자를 사용하여 BGMType에 맞는 클립 선택
        AudioClip selectedClip = (type == BGMType.MainMenu) ? mainMenuClip : inGameClip;

        if (selectedClip == null)
        {
            Debug.LogWarning($"⚠️ {type}에 해당하는 BGM 오디오 클립이 null입니다!");
            return;
        }

        // [추가] 이미 같은 BGM이 재생 중이면 다시 시작하지 않음 (선택적)
        if (bgmSource.clip == selectedClip && bgmSource.isPlaying)
        {
            return;
        }

        bgmSource.clip = selectedClip;
        bgmSource.loop = true; // BGM은 항상 반복 재생
        bgmSource.Play();

        Debug.Log($"▶️ BGM 재생 시작됨: {selectedClip.name}");
    }

    /// <summary>
    /// 🔊 효과음을 재생합니다. (SFX Source를 통해, 겹쳐서 재생 가능)
    /// </summary>
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null)
        {
            Debug.LogWarning("⚠️ SFX 재생 실패 (클립 또는 AudioSource가 null)");
            return;
        }

        // [수정] PlayOneShot: 현재 재생 중인 소리를 멈추지 않고 새 소리를 겹쳐서 재생
        sfxSource.PlayOneShot(clip);
        Debug.Log($"🔊 재생되는 효과음: {clip.name}");
    }
}