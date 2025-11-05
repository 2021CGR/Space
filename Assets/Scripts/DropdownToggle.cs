using UnityEngine;

/// <summary>
/// 버튼 클릭 시 연결된 패널(드롭다운)을 켜고 끄는 토글 스크립트입니다.
/// </summary>
public class DropdownToggle : MonoBehaviour
{
    [SerializeField] // [추가] private 변수지만 인스펙터에서 보이기
    [Tooltip("이 버튼으로 켜고 끌 게임 오브젝트 (예: 해상도 옵션 패널)")]
    private GameObject dropdownPanel;

    /// <summary>
    /// [추가] 이 함수를 버튼의 OnClick() 이벤트에 연결하세요.
    /// </summary>
    public void ToggleDropdown()
    {
        if (dropdownPanel != null)
        {
            // [수정] 현재 활성화 상태를 가져와서, 그 반대 값으로 설정
            bool isActive = dropdownPanel.activeSelf;
            dropdownPanel.SetActive(!isActive);

            Debug.Log($"드롭다운 패널 상태 변경: {!isActive}");
        }
        else
        {
            Debug.LogWarning("DropdownToggle에 'dropdownPanel'이 연결되지 않았습니다.");
        }
    }
}