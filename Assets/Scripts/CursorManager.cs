using UnityEngine;

public class CursorManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static CursorManager Instance;

    private void Awake()
    {
        // 인스턴스가 없다면 이 오브젝트를 사용하고 유지시킴
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 이미 인스턴스가 존재하면 중복 제거
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 커서 표시 여부를 설정하는 함수
    /// </summary>
    /// <param name="isVisible">true: 커서 표시, false: 숨김</param>
    public void SetCursorVisible(bool isVisible)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
        Debug.Log($"🚀 커서 상태 변경됨 → Visible: {isVisible}, LockState: {Cursor.lockState}");
    }
}
