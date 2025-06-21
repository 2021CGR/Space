using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResolutionOptionHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image targetImage;  // 강조할 이미지
    [SerializeField] private Color hoverColor = new Color(1f, 0.902f, 0.502f);  // 마우스 올릴 때 색
    [SerializeField] private Color normalColor = Color.white;              // 기본 색

    [SerializeField] private int width = 1920;  // 이 옵션의 해상도 가로
    [SerializeField] private int height = 1080; // 이 옵션의 해상도 세로

    void Start()
    {
        if (targetImage != null)
            targetImage.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetImage != null)
            targetImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetImage != null)
            targetImage.color = normalColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Screen.SetResolution(width, height, FullScreenMode.Windowed);
        PlayerPrefs.SetInt("ResolutionWidth", width);
        PlayerPrefs.SetInt("ResolutionHeight", height);
        PlayerPrefs.Save();

        Debug.Log($"🖥️ 해상도 변경: {width}x{height}");
    }
}
