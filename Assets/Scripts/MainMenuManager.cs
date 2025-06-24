using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 메인 메뉴 UI 버튼 동작을 관리하는 스크립트 (씬 이동 전담)
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [Header("게임 씬 이름")]
    public string gameSceneName = "GameScene";

    private void Start()
    {
        // ✅ 메인 메뉴 진입 시 배경음 재생
        BGMManager.Instance?.PlayBGM(BGMType.MainMenu);
    }

    /// <summary>
    /// 게임 시작 버튼
    /// </summary>
    public void OnStartButton()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    /// <summary>
    /// 게임 종료 버튼
    /// </summary>
    public void OnQuitButton()
    {
        Application.Quit();
        Debug.Log("게임 종료 요청됨 (에디터에서는 무시됨)");
    }
}
