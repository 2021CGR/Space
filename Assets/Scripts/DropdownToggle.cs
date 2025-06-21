using UnityEngine;

public class DropdownToggle : MonoBehaviour
{
    [SerializeField] private GameObject dropdownPanel; // 해상도 옵션 묶음

    public void ToggleDropdown()
    {
        if (dropdownPanel != null)
        {
            bool isActive = dropdownPanel.activeSelf;
            dropdownPanel.SetActive(!isActive);
        }
    }
}
