using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("🎚️ 슬라이더 연결")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    [Header("🎵 Audio Mixer 연결")]
    public AudioMixer audioMixer;

    // AudioMixer 파라미터 이름
    private const string BGM_PARAM = "BackGroundVolume";
    private const string SFX_PARAM = "SFXVolume";

    // ✅ 중복 방지용 인스턴스
    private static SettingsManager instance;

    private void Awake()
    {
        // ✅ 중복된 SettingsManager가 있으면 제거
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // ✅ 처음 생성된 경우엔 유지
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        Debug.Log($"🛡 SettingsManager 유지됨: {gameObject.name}");
    }

    private void Start()
    {
        // 🔧 이벤트 중복 방지를 위해 기존 이벤트 제거
        bgmSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.RemoveAllListeners();

        // 🔹 저장된 값 불러오기 (없으면 1.0f)
        float savedBgmVolume = PlayerPrefs.GetFloat(BGM_PARAM, 1f);
        float savedSfxVolume = PlayerPrefs.GetFloat(SFX_PARAM, 1f);

        // 🔹 슬라이더에 값 먼저 반영
        bgmSlider.value = savedBgmVolume;
        sfxSlider.value = savedSfxVolume;

        // 🔹 Mixer에 실제로 적용
        SetVolume(BGM_PARAM, savedBgmVolume);
        SetVolume(SFX_PARAM, savedSfxVolume);

        Debug.Log($"📌 [SettingsManager.Start] 🎵 BGM 복원: {savedBgmVolume}, 🔊 SFX 복원: {savedSfxVolume}");

        // ✅ 슬라이더 값 변경 시 직접 연결
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    public void OnBGMVolumeChanged(float value)
    {
        SetVolume(BGM_PARAM, value);
        PlayerPrefs.SetFloat(BGM_PARAM, value);
        Debug.Log($"📢 배경음 복원 변경: {value}");
    }

    public void OnSFXVolumeChanged(float value)
    {
        SetVolume(SFX_PARAM, value);
        PlayerPrefs.SetFloat(SFX_PARAM, value);
        Debug.Log($"🔊 효과음 복원 변경: {value}");
    }

    private void SetVolume(string parameter, float value)
    {
        float clamped = Mathf.Clamp(value, 0.0001f, 1f);
        float db = Mathf.Log10(clamped) * 20f;

        audioMixer.SetFloat(parameter, db);
        Debug.Log($"🎚️ {parameter} 볼륨 설정됨: {value} → dB: {db}");
    }
}
