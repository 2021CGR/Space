using UnityEngine;

/// <summary>
/// 총알이 오른쪽(X+) 방향으로 일정 속도로 이동하며
/// 일정 시간 후 자동으로 파괴되는 스크립트
/// </summary>
public class Bullet : MonoBehaviour
{
    public float speed = 10f;              // 총알 이동 속도
    public float lifetime = 3f;            // 총알이 사라지는 시간 (초)

    void Start()
    {
        // 1. 일정 시간이 지나면 총알을 자동으로 삭제함
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // 2. 매 프레임 오른쪽(X+)으로 이동 (Vector2.right 사용)
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}
