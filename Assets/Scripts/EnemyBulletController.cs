using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    [Header("총알 이동 속도 설정")]
    public float speed = 10f; // 총알의 이동 속도

    [Header("총알 지속 시간")]
    public float lifetime = 5f; // 총알이 생성되고 몇 초 후에 사라질지 설정

    void Start()
    {
        // lifetime이 지난 후 총알을 자동으로 삭제
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // 매 프레임마다 총알을 왼쪽(x-) 방향으로 이동시킴
        // Vector3.left는 (-1, 0, 0) 방향 벡터임
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // 플레이어와 충돌했는지 확인
        if (other.CompareTag("Player"))
        {
            // 플레이어에게 데미지를 줄 수 있도록 호출
            // 예: other.GetComponent<PlayerHealth>().TakeDamage(1);

            // 총알 제거
            Destroy(gameObject);
        }
    }
}
