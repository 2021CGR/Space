using UnityEngine;

/// <summary>
/// 여러 배경 Sprite 오브젝트를 가로 방향으로 자연스럽게 스크롤시켜
/// 무한 루프 배경을 구현하는 스크립트
/// </summary>
public class BackgroundManager : MonoBehaviour
{
    [Header("배경 설정")]
    public GameObject[] backgrounds;         // 스크롤에 사용할 배경 오브젝트들 (2개 이상 필요)
    public float scrollSpeed = 2f;           // 배경이 왼쪽으로 움직이는 속도 (유닛/초)

    private float backgroundWidth;           // 각 배경의 가로 길이 (World 기준)

    void Start()
    {
        // 첫 번째 배경의 SpriteRenderer에서 실제 Sprite의 폭을 구함
        SpriteRenderer sr = backgrounds[0].GetComponent<SpriteRenderer>();

        // bounds.size.x는 Sprite의 실제 월드 단위 너비
        backgroundWidth = sr.bounds.size.x;
    }

    void Update()
    {
        foreach (GameObject bg in backgrounds)
        {
            // 배경을 매 프레임 왼쪽으로 이동시킴
            bg.transform.Translate(Vector2.left * scrollSpeed * Time.deltaTime);

            // 현재 배경의 오른쪽 끝 X 좌표 계산
            float rightEdge = bg.transform.position.x + backgroundWidth / 2f;

            // 화면의 왼쪽 경계 계산 (0 대신 여유를 주어 Viewport 0.01 사용)
            float leftScreenEdge = Camera.main.ViewportToWorldPoint(new Vector3(0 , 0, 0)).x;

            // 만약 배경이 화면 왼쪽 바깥으로 완전히 벗어나면 재배치
            if (rightEdge < leftScreenEdge)
            {
                // 가장 오른쪽에 있는 배경의 X 좌표를 구함
                float rightMostX = GetRightMostX();

                // 현재 배경의 새로운 위치 계산
                Vector3 newPos = bg.transform.position;

                // 새로운 위치는 가장 오른쪽 배경의 오른쪽 끝에 이어 붙이되,
                // 0.01f만큼 겹치게 배치해 틈이 생기지 않도록 함
                newPos.x = rightMostX + backgroundWidth - 0.01f;

                // 위치를 소수점 둘째 자리로 반올림해 픽셀 단위 떨림을 방지
                newPos.x = Mathf.Round(newPos.x * 100f) / 100f;

                // 위치 재설정
                bg.transform.position = newPos;
            }
        }
    }

    /// <summary>
    /// 현재 배경들 중 가장 오른쪽에 있는 오브젝트의 중심 X 좌표를 반환
    /// </summary>
    /// <returns>가장 오른쪽 배경의 중심 X 좌표</returns>
    float GetRightMostX()
    {
        float maxX = float.MinValue;
        foreach (GameObject bg in backgrounds)
        {
            if (bg.transform.position.x > maxX)
            {
                maxX = bg.transform.position.x;
            }
        }
        return maxX;
    }
}
