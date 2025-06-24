using UnityEngine;

/// <summary>
/// 🎮 여러 배경을 Material Offset 방식으로 스크롤하는 스크립트
/// 🎮 SpriteRenderer 대신 MeshRenderer (Quad) 사용 필수
/// </summary>
public class MultiBackgroundScroll : MonoBehaviour
{
    /// <summary>
    /// 🎵 배경 1개 정보: MeshRenderer + 속도
    /// </summary>
    [System.Serializable]
    public class ScrollingBackground
    {
        public Renderer renderer; // 🎨 배경 Quad의 MeshRenderer
        public float scrollSpeed = 0.1f; // 🏃 배경 스크롤 속도 (유닛/초)
    }

    [Header("🎮 스크롤할 배경 목록 (최소 2개)")]
    public ScrollingBackground[] backgrounds; // 🎮 스크롤할 배경들을 배열로 관리

    void Update()
    {
        // 모든 배경을 매 프레임 스크롤
        foreach (ScrollingBackground bg in backgrounds)
        {
            // 🎯 현재 시간에 따라 배경 Offset 계산 (프레임 독립)
            float x = Mathf.Repeat(Time.time * bg.scrollSpeed, 1);

            // 🎯 새 Offset 적용 (y는 그대로)
            Vector2 offset = new Vector2(x, 0);

            // 🎨 Material에 Offset 적용 (배경이 부드럽게 반복됨)
            bg.renderer.material.mainTextureOffset = offset;
        }
    }
}
