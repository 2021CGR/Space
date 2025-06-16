using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 보스 체력을 UI에 실시간으로 반영하고, 등장 시 체력바를 서서히 페이드 인하는 스크립트
/// </summary>
public class BossHealthBar : MonoBehaviour
{
    public Image fillImage;           // 체력바 이미지 (fill type)
    public CanvasGroup canvasGroup;   // 전체 UI의 투명도 조절용
    public Boss boss;                 // 보스 스크립트 연결
    public float fadeSpeed = 1f;      // 페이드 인 속도 (1초 = 1)

    private bool isFadingIn = false;  // 현재 페이드 인 중인지


    void Start()
    {
        // 체력바는 처음엔 완전히 투명
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        // 보스 자동 연결 (태그 기반)
        if (boss == null)
        {
            GameObject obj = GameObject.FindWithTag("Boss");
            if (obj != null)
                boss = obj.GetComponent<Boss>();
        }
    }

    void Update()
    {
        if (boss == null || fillImage == null || canvasGroup == null) return;

        // 보스 등장 완료 시 체력바 페이드 인 시작
        if (boss.HasEntered() && canvasGroup.alpha < 1f)
        {
            isFadingIn = true;
        }

        // 체력바 페이드 인 처리
        if (isFadingIn)
        {
            canvasGroup.alpha += fadeSpeed * Time.deltaTime;

            if (canvasGroup.alpha >= 1f)
            {
                canvasGroup.alpha = 1f;
                isFadingIn = false; // 완료되면 중단
            }
        }

        // 실시간 체력 반영
        fillImage.fillAmount = boss.GetHealthPercent();
    }
}
