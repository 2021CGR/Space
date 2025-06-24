using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("🎵 Audio Mixer 연결")]
    public AudioMixer audioMixer;

    private const string BGM_PARAM = "BackGroundVolume";
    private const string SFX_PARAM = "SFXVolume";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        ApplySavedVolume();
        Debug.Log($"🛡 SettingsManager 유지됨: {gameObject.name}");
    }

    /// <summary>
    /// 메인 메뉴로 이동
    /// </summary>
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// 옵션 메뉴로 이동
    /// </summary>
    public void GoToOptionMenu()
    {
        SceneManager.LoadScene("OptionMenu");
    }

    /// <summary>
    /// 게임 다시 시작
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// 게임 종료
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("게임 종료 요청");
        Application.Quit();
    }

    /// <summary>
    /// AudioMixer 볼륨 적용
    /// </summary>
    public void SetVolume(string parameter, float value)
    {
        float clamped = Mathf.Clamp(value, 0.0001f, 1f);
        float db = Mathf.Log10(clamped) * 20f;

        audioMixer.SetFloat(parameter, db);
        Debug.Log($"🎚️ {parameter} 볼륨 적용: {value} → {db} dB");
    }

    /// <summary>
    /// 저장된 볼륨 불러오기
    /// </summary>
    private void ApplySavedVolume()
    {
        float savedBgm = PlayerPrefs.GetFloat(BGM_PARAM, 1f);
        float savedSfx = PlayerPrefs.GetFloat(SFX_PARAM, 1f);

        SetVolume(BGM_PARAM, savedBgm);
        SetVolume(SFX_PARAM, savedSfx);
    }
}




