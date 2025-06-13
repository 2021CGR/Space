using UnityEngine;

public class EnemyBulletController2D : MonoBehaviour
{
    [Header("총알 속도 및 데미지")]
    public float speed = 10f;
    public float lifetime = 5f;
    public int damage = 1;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // "Player" 태그가 붙은 오브젝트만 타격
        if (other.CompareTag("Player"))
        {
            Debug.Log("적 총알이 플레이어에 충돌!");

            // PlayerHealth2D 컴포넌트가 있을 경우 체력 감소
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
