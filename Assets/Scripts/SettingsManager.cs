using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 설정 메뉴 내에서 사운드 볼륨을 조절하는 스크립트.
/// 슬라이더 값이 변경되면 전체 오디오 볼륨에 실시간으로 반영된다.
/// </summary>
public class SettingsManager : MonoBehaviour
{
    [Header("UI 연결")]
    public Slider volumeSlider; // 사운드 볼륨 조절용 슬라이더

    void Start()
    {
        // 슬라이더 초기값을 현재 볼륨과 동기화
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;

            // 슬라이더 값이 변경될 때마다 SetVolume 함수가 호출됨
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    /// <summary>
    /// 슬라이더 값이 바뀔 때 호출됨 — 전체 볼륨 적용
    /// </summary>
    /// <param name="value">0.0 ~ 1.0 사이의 볼륨 값</param>
    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        Debug.Log("전체 볼륨: " + value);
    }
}
