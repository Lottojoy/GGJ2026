using System.Collections;
using UnityEngine;

public class ThunderAttack : AttackMask
{
    protected override IEnumerator ExecuteAttackRoutine()
    {
        if (_movement == null) yield break;

        _movement.LockMovement(true);
        yield return new WaitForSeconds(_anticipationTime);

        // ตรวจสอบศัตรูในระยะ Hitbox
        Collider2D[] enemies = Physics2D.OverlapBoxAll(GetAttackPoint(), _hitboxSize, 0, _enemyLayer);
        foreach (var e in enemies)
        {
            if (e.TryGetComponent(out MobHp enemy))
            {
                enemy.TakeDamage(GetFinalDamage()); //เพิ่ม madness
                Debug.Log($"Hit: {e.name}");
            }
        }

        yield return new WaitForSeconds(_recoveryTime);
        _movement.LockMovement(false);
    }
}