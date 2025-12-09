using UnityEngine;

/// <summary>
/// 2D 플레이어 캐릭터를 상하(X) 및 좌우(Y)로 자유롭게 이동시키는 컨트롤러
/// 이동 범위는 화면 내부로 제한되며, 프레임과 무관하게 부드럽게 작동한다.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("이동 속도 설정")]
    public float moveSpeed = 5f;              // 이동 속도 (유닛/초)

    [Header("이동 제한 범위")]
    public float minX = -8.5f;                // 왼쪽으로 이동 가능한 최소 X 좌표
    public float maxX = 8.5f;                 // 오른쪽으로 이동 가능한 최대 X 좌표
    public float minY = -4.5f;                // 아래로 이동 가능한 최소 Y 좌표
    public float maxY = 4.5f;                 // 위로 이동 가능한 최대 Y 좌표

    [Header("스무딩 설정")]
    public float smoothTime = 0.08f;

    private Vector3 currentVelocity;
    private Vector3 velocityRef;

    void Update()
    {
        // 1. 사용자 입력 받기 (WASD 또는 방향키)
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // A/D 또는 ←/→ 키
        float verticalInput = Input.GetAxisRaw("Vertical");     // W/S 또는 ↑/↓ 키

        Vector2 direction = new Vector2(horizontalInput, verticalInput).normalized;

        Vector3 targetVelocity = new Vector3(direction.x, direction.y, 0f) * moveSpeed;
        currentVelocity = Vector3.SmoothDamp(currentVelocity, targetVelocity, ref velocityRef, smoothTime);

        Vector2 newPosition = (Vector2)transform.position + (Vector2)(currentVelocity * Time.deltaTime);

        // 5. 새로운 위치를 X/Y 제한 범위 내로 제한 (Clamp 사용)
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        transform.position = newPosition;

        transform.rotation = Quaternion.identity;
    }

    void LateUpdate()
    {
        // 7. Z값을 강제로 고정하여 Sprite가 뒤로 사라지는 현상 방지
        Vector3 fixedPosition = transform.position;
        fixedPosition.z = 0.5f;  // 플레이어 Z 고정값 (원하면 0으로도 가능)
        transform.position = fixedPosition;
    }
}
