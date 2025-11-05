using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 보스 체력을 UI에 실시간으로 반영하고, 등장 시 체력바를 서서히 페이드 인하는 스크립트
/// </summary>
[RequireComponent(typeof(CanvasGroup))] // [추가] 페이드 인/아웃을 위해 CanvasGroup 필수
public class BossHealthBar : MonoBehaviour
{
    [Header("UI 연결")]
    [Tooltip("체력바의 채워지는 이미지 (Fill Amount 방식)")]
    public Image fillImage;
    [Tooltip("전체 UI의 투명도 조절용 (자동 할당 시도)")]
    public CanvasGroup canvasGroup;

    [Header("보스 연결")]
    [Tooltip("체력 정보를 가져올 Boss 스크립트 (없으면 'Boss' 태그로 검색)")]
    public Boss boss;
    [Tooltip("체력바가 나타나는 속도 (1 = 1초)")]
    public float fadeSpeed = 1f;

    private bool isFadingIn = false; // 현재 페이드 인 중인지

    void Start()
    {
        // [추가] CanvasGroup 자동 할당
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        // 체력바는 처음엔 완전히 투명
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        // [수정] 보스 자동 연결 (태그 기반)
        if (boss == null)
        {
            GameObject obj = GameObject.FindWithTag("Boss");
            if (obj != null)
            {
                boss = obj.GetComponent<Boss>();
                if (boss == null)
                {
                    Debug.LogWarning("'Boss' 태그 오브젝트에서 Boss 스크립트를 찾지 못했습니다.");
                }
            }
            else
            {
                Debug.LogWarning("씬에 'Boss' 태그를 가진 오브젝트가 없습니다.");
            }
        }
    }

    void Update()
    {
        // [수정] 필수 컴포넌트 중 하나라도 없으면 중단 (오류 방지)
        if (boss == null || fillImage == null || canvasGroup == null)
        {
            // [추가] 보스가 죽어서 사라진 경우 체력바도 비활성화
            if (boss == null)
            {
                gameObject.SetActive(false);
            }
            return;
        }

        // 보스 등장 완료 시 체력바 페이드 인 시작
        // [수정] isFadingIn 플래그를 체크하여 중복 실행 방지
        if (boss.HasEntered() && canvasGroup.alpha < 1f && !isFadingIn)
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

        // [수정] 보스가 등장한 이후에만 체력 실시간 반영
        if (boss.HasEntered())
        {
            fillImage.fillAmount = boss.GetHealthPercent();
        }
    }
}