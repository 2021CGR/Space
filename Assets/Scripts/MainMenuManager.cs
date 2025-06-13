using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 메인 메뉴 UI 버튼 동작을 관리하는 스크립트
/// - 게임 시작
/// - 설정 창 열기/닫기
/// - 게임 종료
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [Header("게임 씬 이름")]
    public string gameSceneName = "GameScene"; // 실제 게임 씬 이름과 정확히 일치해야 함

    [Header("UI 패널")]
    public GameObject settingsPanel;           // 설정창 패널 (Canvas 내 UI 오브젝트)

    /// <summary>
    /// "게임 시작" 버튼 눌렀을 때 호출됨
    /// </summary>
    public void OnStartButton()
    {
        SceneManager.LoadScene(gameSceneName); // 게임 씬으로 전환
    }

    /// <summary>
    /// "설정" 버튼 눌렀을 때 설정 창 표시
    /// </summary>
    public void OnSettingsButton()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);     // 설정 패널 보여주기
    }

    /// <summary>
    /// 설정창 안의 "닫기" 버튼에서 호출됨
    /// </summary>
    public void OnCloseSettingsButton()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);    // 설정 패널 숨기기
    }

    /// <summary>
    /// "게임 종료" 버튼 눌렀을 때 호출됨
    /// </summary>
    public void OnQuitButton()
    {
        Application.Quit();                    // PC에서만 작동함
        Debug.Log("게임 종료 요청됨 (에디터에서는 무시됨)");
    }
}
