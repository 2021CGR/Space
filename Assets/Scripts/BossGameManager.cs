using UnityEngine;

/// <summary>
/// 보스를 쓰러뜨리면 클리어 패널이 뜨는 보스 전용 게임 매니저야!
/// </summary>
public class BossGameManager : MonoBehaviour
{
    public static BossGameManager instance;

    [Header("🎯 보스 오브젝트")]
    public GameObject boss; // Inspector에서 보스 오브젝트를 드래그해서 넣어줘

    [Header("🎉 클리어 패널")]
    public GameObject clearPanel; // 클리어 UI 패널 (비활성화 상태로 시작)

    private bool isBossDefeated = false; // 중복 처리 방지용

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        // 🎵 인게임 BGM 재생
        BGMManager.Instance?.PlayBGM(BGMType.InGame);

        // 🧼 클리어 패널은 처음에 꺼두자
        if (clearPanel != null)
            clearPanel.SetActive(false);
    }

    /// <summary>
    /// 보스가 쓰러졌을 때 호출되는 함수야!
    /// 보스 스크립트에서 죽을 때 이 함수를 호출해줘.
    /// </summary>
    public void OnBossDefeated()
    {
        if (isBossDefeated) return; // 중복 방지
        isBossDefeated = true;

        Debug.Log("🎉 보스를 처치했습니다! 클리어 패널을 표시합니다.");

        if (clearPanel != null)
            clearPanel.SetActive(true);
    }
}
