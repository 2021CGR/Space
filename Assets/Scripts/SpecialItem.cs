using UnityEngine;

public class SpecialItem : MonoBehaviour
{
    public enum ItemType { Lightning, DualShot }
    public ItemType type = ItemType.Lightning;
    public float lifetime = 5f;

    void Start() => Destroy(gameObject, lifetime);

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var shooter = other.GetComponent<PlayerShooting>();
        if (shooter == null) return;

        switch (type)
        {
            case ItemType.Lightning:
                shooter.EnableSpecialAttack();
                // [수정] 번개 아이템일 때만 에너지 UI 활성화
                var energy = other.GetComponent<PlayerSpecialEnergy>();
                if (energy != null) energy.GainEnergy();
                
                Debug.Log("⚡ 번개 아이템: 스페셜(레이저) 획득");
                break;
            case ItemType.DualShot:
                shooter.EnableDualShot();
                Debug.Log("🔫 듀얼샷 아이템: 2발 업그레이드");
                break;
        }
        Destroy(gameObject);
    }
}