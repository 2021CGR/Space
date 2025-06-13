using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ESC 키로 일시정지 및 재개를 제어하고, 설정창과 메인메뉴 이동을 처리.
/// SettingsPanel이 열려 있어도 게임은 항상 멈춘 상태를 유지.
/// </summary>
public class PauseManager : MonoBehaviour
{
    [Header("UI 연결")]
    public GameObject pausePanel;     // 일시정지 UI 패널
    public GameObject settingsPanel;  // 설정창 패널

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("✅ ESC 키 입력 감지됨");

            if (pausePanel == null || settingsPanel == null)
            {
                Debug.LogWarning("❌ 패널 연결 안 됨!");
                return;
            }

            // 설정창이 열려 있는 경우 → 닫기
            if (settingsPanel.activeSelf)
            {
                Debug.Log("📦 ESC → 설정창 닫기");
                CloseSettings();
                return;
            }

            // 일시정지 메뉴가 열려 있는 경우 → 재개
            if (pausePanel.activeSelf)
            {
                Debug.Log("▶ ESC → ResumeGame()");
                ResumeGame();
            }
            else
            {
                Debug.Log("⏸ ESC → PauseGame()");
                PauseGame();
            }
        }
    }

    /// <summary>
    /// 게임을 일시정지 상태로 전환
    /// </summary>
    public void PauseGame()
    {
        Debug.Log("⏸ PauseGame() 실행됨");

        Time.timeScale = 0f;                        // 게임 정지
        pausePanel.SetActive(true);                 // Pause 패널 열기
        settingsPanel.SetActive(false);             // 설정창 닫기
    }

    /// <summary>
    /// 게임을 재개
    /// </summary>
    public void ResumeGame()
    {
        Debug.Log("▶ ResumeGame() 실행됨");

        Time.timeScale = 1f;                        // 게임 재개
        pausePanel.SetActive(false);                // Pause 패널 닫기
        settingsPanel.SetActive(false);             // 설정창도 닫기
    }

    /// <summary>
    /// 설정창 열기 (Pause 메뉴에서 호출)
    /// 게임은 계속 정지 상태 유지
    /// </summary>
    public void OpenSettings()
    {
        Debug.Log("⚙️ OpenSettings() 실행됨");

        pausePanel.SetActive(false);                // Pause 메뉴 닫기
        settingsPanel.SetActive(true);              // 설정창 열기

        // ❌ 게임을 재개하지 않음 — 계속 멈춰 있음 (Time.timeScale 유지)
    }

    /// <summary>
    /// 설정창 닫기
    /// Pause 메뉴로 복귀하지 않는다면 게임은 자동으로 재개됨
    /// </summary>
    public void CloseSettings()
    {
        Debug.Log("❌ CloseSettings() 실행됨");

        settingsPanel.SetActive(false);             // 설정창 닫기

        // PausePanel도 꺼져 있으면 게임 재개
        if (!pausePanel.activeSelf)
        {
            Debug.Log("✅ 설정창만 열려 있었으므로 게임 재개");
            Time.timeScale = 1f;
        }
    }

    /// <summary>
    /// 메인 메뉴로 이동
    /// </summary>
    public void QuitToMainMenu()
    {
        Debug.Log("🏁 QuitToMainMenu() 실행됨");

        Time.timeScale = 1f;                        // 시간 재개 후 씬 이동
        SceneManager.LoadScene("MainMenu");         // 메인 메뉴로 이동
    }
}
