using UnityEngine;

public class SpecialItem : MonoBehaviour
{
    // 아이템이 생성된 후 몇 초 뒤에 사라질지를 결정하는 변수 (기본값: 5초)
    public float lifetime = 5f;

    void Start()
    {
        // 일정 시간이 지나면 아이템이 자동으로 사라지도록 설정
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 태그가 "Player"인 오브젝트와 충돌했을 경우에만
        if (other.CompareTag("Player"))
        {
            // 충돌한 오브젝트에서 PlayerShooting 컴포넌트를 찾음
            PlayerShooting shooter = other.GetComponent<PlayerShooting>();
            if (shooter != null)
            {
                shooter.EnableSpecialAttack(); // 스페셜 공격 가능 상태로 설정
                Debug.Log("플레이어가 아이템(Item)과 충돌하여 스페셜 공격을 획득했습니다.");
            }

            // 태그가 "Item"인 경우에만 아이템 제거
            if (CompareTag("Item"))
            {
                Destroy(gameObject); // 자기 자신(아이템) 삭제
            }
        }
    }
}
