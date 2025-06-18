using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 모든 씬에서 공유되는 사운드 설정 관리자.
/// - 슬라이더로 볼륨 조절 가능
/// - PlayerPrefs로 저장 및 복원
/// - 씬 전환 시에도 볼륨 초기화 안 됨
/// </summary>
public class SettingsManager : MonoBehaviour
{
    [Header("UI 연결")]
    public Slider volumeSlider;

    private const string VolumeKey = "MasterVolume";
    private float previousVolume;

    void Awake()
    {
        // ✅ 씬 진입 시 저장된 볼륨을 AudioListener에 우선 반영
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);
        AudioListener.volume = savedVolume;
        previousVolume = savedVolume;

        Debug.Log($"[SettingsManager.Awake] 볼륨 복원: {savedVolume:F2}");
    }

    void Start()
    {
        if (volumeSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f);

            // ✅ 슬라이더 값을 설정하되 SetVolume이 자동 실행되지 않도록 처리
            volumeSlider.SetValueWithoutNotify(savedVolume);

            // ✅ 슬라이더 조작 시에만 SetVolume이 호출되도록 연결
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    /// <summary>
    /// 슬라이더 값 변경 시 호출됨 — 볼륨 적용 + 저장 + 디버그 출력
    /// </summary>
    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat(VolumeKey, value);
        PlayerPrefs.Save();

        // 🔊 디버그 출력
        if (value > previousVolume)
            Debug.Log($"🔊 볼륨 증가: {previousVolume:F2} → {value:F2}");
        else if (value < previousVolume)
            Debug.Log($"🔉 볼륨 감소: {previousVolume:F2} → {value:F2}");
        else
            Debug.Log($"🔁 볼륨 동일: {value:F2}");

        previousVolume = value;
    }
}

