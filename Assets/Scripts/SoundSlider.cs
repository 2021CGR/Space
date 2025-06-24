using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    public enum SoundType { BGM, SFX }
    public SoundType soundType;

    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();

        // 저장된 값 불러오기
        if (soundType == SoundType.BGM)
            slider.value = PlayerPrefs.GetFloat("BackGroundVolume", 1f);
        else
            slider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(float value)
    {
        if (soundType == SoundType.BGM)
        {
            SettingsManager.Instance.SetVolume("BackGroundVolume", value);
            PlayerPrefs.SetFloat("BackGroundVolume", value);
        }
        else
        {
            SettingsManager.Instance.SetVolume("SFXVolume", value);
            PlayerPrefs.SetFloat("SFXVolume", value);
        }
    }
}
