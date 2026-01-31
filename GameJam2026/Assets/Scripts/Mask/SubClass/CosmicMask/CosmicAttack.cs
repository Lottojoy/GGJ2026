using UnityEngine;
using System.Collections;

public class CosmicAttackMask : AttackMask
{
    [SerializeField] private float pullForce = 8f;
    protected override IEnumerator ExecuteAttackRoutine()
    {
        yield return new WaitForSeconds(_anticipationTime);
        Vector2 attackPoint = GetAttackPoint();
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPoint,_hitboxSize,0f,_enemyLayer);

        foreach (var hit in hits)
        {
            // ดาเมจ
            if (hit.TryGetComponent(out MobHp hp))
            {
                hp.TakeDamage(GetFinalDamage());
            }

            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 dir = (attackPoint - (Vector2)hit.transform.position).normalized;
                rb.AddForce(dir * pullForce, ForceMode2D.Impulse);
            }
        }

        // Recovery
        yield return new WaitForSeconds(_recoveryTime);
    }
}
