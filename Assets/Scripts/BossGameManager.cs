using UnityEngine;

/// <summary>
/// 보스를 쓰러뜨리면 클리어 패널이 뜨는 보스 전용 게임 매니저입니다. (싱글톤)
/// </summary>
public class BossGameManager : MonoBehaviour
{
    // [추가] 싱글톤 인스턴스 (외부에서 쉽게 접근 가능)
    public static BossGameManager instance;

    [Header("🎯 보스 오브젝트")]
    [Tooltip("씬에 있는 보스 오브젝트 (HP바 연동 등에 사용)")]
    public GameObject boss; // [수정] 주석 명확화 (현재 코드에서는 직접 사용되진 않음)

    [Header("🎉 클리어 패널")]
    [Tooltip("보스 처치 시 활성화할 클리어 UI 패널")]
    public GameObject clearPanel;

    [Header("🎬 엔딩 스토리")]
    [Tooltip("보스 처치 후 나올 엔딩 스토리 매니저 (선택 사항)")]
    public EndingStoryManager endingStoryManager;

    private bool isBossDefeated = false; // 중복 호출 방지용

    private void Awake()
    {
        // [추가] 싱글톤 인스턴스 설정
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        // [수정] BGM 매니저가 존재할 경우에만 인게임 BGM 재생
        BGMManager.Instance?.PlayBGM(BGMType.InGame);

        // [추가] 클리어 패널은 처음에 비활성화
        if (clearPanel != null)
            clearPanel.SetActive(false);
    }

    /// <summary>
    /// 보스가 쓰러졌을 때 호출되는 함수입니다. (예: Boss.cs의 Die()에서 호출)
    /// </summary>
    public void OnBossDefeated()
    {
        // [추가] 이미 패배 처리가 되었다면 중복 실행 방지
        if (isBossDefeated) return;
        isBossDefeated = true;

        Debug.Log("🎉 보스 처치! 엔딩 스토리 또는 클리어 화면으로 이동");

        // [수정] 엔딩 스토리가 연결되어 있다면 먼저 실행
        if (endingStoryManager != null)
        {
            endingStoryManager.StartEnding();
        }
        else
        {
            // 엔딩 스토리가 없으면 바로 클리어 UI 표시
            ShowClearUI();
        }
    }

    /// <summary>
    /// 클리어 UI를 표시하고 커서를 보이게 함 (EndingStoryManager에서 호출 가능)
    /// </summary>
    public void ShowClearUI()
    {
        Debug.Log("🏆 클리어 화면 표시");

        if (clearPanel != null)
            clearPanel.SetActive(true);

        // 클리어 시 커서 보이기
        CursorManager.Instance?.SetCursorVisible(true);
    }
}