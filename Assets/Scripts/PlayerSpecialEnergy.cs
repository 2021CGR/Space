using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 플레이어가 아이템을 먹었을 때 에너지를 획득하고,
/// 화면에 에너지 아이콘을 표시하는 스크립트
/// </summary>
public class PlayerSpecialEnergy : MonoBehaviour
{
    [Header("에너지 상태")]
    public bool hasEnergy = false;              // 현재 에너지 보유 여부

    [Header("UI 연결")]
    public GameObject energyIconUI;             // 에너지 아이콘 UI 오브젝트

    void Start()
    {
        // 게임 시작 시 에너지 아이콘은 숨김
        if (energyIconUI != null)
            energyIconUI.SetActive(false);
    }

    /// <summary>
    /// 에너지를 획득하면 호출됨
    /// </summary>
    public void GainEnergy()
    {
        hasEnergy = true;

        if (energyIconUI != null)
            energyIconUI.SetActive(true);       // 에너지 아이콘 표시
    }

    /// <summary>
    /// 에너지를 소모하면 호출됨
    /// </summary>
    public void ConsumeEnergy()
    {
        hasEnergy = false;

        if (energyIconUI != null)
            energyIconUI.SetActive(false);      // 아이콘 숨김
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 태그가 "Item"이면 에너지 아이템으로 간주
        if (other.CompareTag("Item"))
        {
            Debug.Log("⚡ 에너지 아이템 획득!");
            GainEnergy();                        // 에너지 활성화
            Destroy(other.gameObject);           // 아이템 제거
        }
    }
}
