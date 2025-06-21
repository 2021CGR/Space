using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance?.PlayHoverSound();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance?.PlayClickSound();
    }
}
