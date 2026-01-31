using UnityEngine;

public class MobAttack : MonoBehaviour
{
    public int Damage = 10;
    public string playerTag = "Player";

    [Header("Attack Cooldown")]
    private float attackCooldown = 2f; // ตีทุกๆ 2 วิ

    private float nextAttackTime = 0f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(playerTag))
            return;

        // เช็คเวลาคูลดาวน์
        if (Time.time >= nextAttackTime)
        {
            AttackPlayer();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void AttackPlayer()
    {
        if (PlayerStats.Instance == null) return;

        Debug.Log(gameObject.name + " attacked player for " + Damage + " damage");
        PlayerStats.Instance.TakeDamage(Damage);
    }
}
