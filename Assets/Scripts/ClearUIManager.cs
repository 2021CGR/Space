using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임 클리어 시 UI를 표시하고 버튼 처리까지 관리하는 스크립트
/// </summary>
public class ClearUIManager : MonoBehaviour
{
    public GameObject clearPanel;  // 클리어 UI 패널

    void Start()
    {
        if (clearPanel != null)
            clearPanel.SetActive(false); // 시작 시 숨기기
    }

    /// <summary>
    /// 외부에서 클리어 화면을 표시하도록 호출됨
    /// </summary>
    public void ShowClear()
    {
        if (clearPanel != null)
        {
            clearPanel.SetActive(true);
            Time.timeScale = 0f; // 게임 정지
            Debug.Log("🎉 클리어 화면 표시!");
        }
    }

    /// <summary>
    /// 메인 메뉴로 이동 (버튼용)
    /// </summary>
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
