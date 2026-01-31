using UnityEngine;
using System.Collections;

public class FireAttackMask : AttackMask
{
    [Header("Fire Settings")]
    [SerializeField] private int burnDamage = 2;
    [SerializeField] private float burnDuration = 3f;

    protected override IEnumerator ExecuteAttackRoutine()
    {
        yield return new WaitForSeconds(_anticipationTime);
        Vector2 attackPoint = GetAttackPoint();
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            attackPoint,
            _hitboxSize,
            0f,
            _enemyLayer
        );

        foreach (var hit in hits)
        {
            // ดาเมจปกติ
            if (hit.TryGetComponent(out MobHp mobHp))
            {
                mobHp.TakeDamage(GetFinalDamage());
            }

            // Burn
            if (hit.TryGetComponent(out BurnStatus burn))
            {
                burn.ApplyBurn(burnDamage, burnDuration);
            }
        }

        yield return new WaitForSeconds(_recoveryTime);
    }
}
