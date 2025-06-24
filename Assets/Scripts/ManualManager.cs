using UnityEngine;

/// <summary>
/// 설명서 UI를 열고 닫는 귀여운 매니저야!
/// </summary>
public class ManualManager : MonoBehaviour
{
    // 📄 설명서 UI 패널 (ManualPanel)
    public GameObject manualPanel;

    void Start()
    {
        // 🚫 시작할 때 설명서 패널은 꺼져 있어야 해!
        manualPanel.SetActive(false);
    }

    /// <summary>
    /// 📖 설명서 열기 버튼을 누르면 실행되는 함수야!
    /// </summary>
    public void OpenManual()
    {
        manualPanel.SetActive(true); // 설명서 패널 보이기
    }

    /// <summary>
    /// ❌ 닫기 버튼을 누르면 실행되는 함수야!
    /// </summary>
    public void CloseManual()
    {
        manualPanel.SetActive(false); // 설명서 패널 숨기기
    }
}
