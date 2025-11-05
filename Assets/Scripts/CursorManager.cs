using UnityEngine;

/// <summary>
/// 마우스 커서의 가시성(Visible)과 잠금 상태(LockState)를 관리하는 싱글톤 매니저입니다.
/// </summary>
public class CursorManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static CursorManager Instance;

    private void Awake()
    {
        // [수정] 싱글톤 인스턴스 설정 (BGMManager와 동일한 로직)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 커서 표시 여부와 잠금 상태를 설정합니다.
    /// </summary>
    /// <param name="isVisible">true: 커서 표시, false: 커서 숨김 및 잠금</param>
    public void SetCursorVisible(bool isVisible)
    {
        Cursor.visible = isVisible;

        // [수정] isVisible이 true이면 커서 잠금 해제(None), false이면 중앙에 잠금(Locked)
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;

        Debug.Log($"🚀 커서 상태 변경됨 → Visible: {isVisible}, LockState: {Cursor.lockState}");
    }
}