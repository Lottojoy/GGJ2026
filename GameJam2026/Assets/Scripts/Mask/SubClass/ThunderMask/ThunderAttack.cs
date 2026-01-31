using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderAttack : AttackMask
{
    protected override IEnumerator ExecuteAttackRoutine()
    {
        yield return new WaitForSeconds(_anticipationTime);

        
        Collider2D[] enemies = Physics2D.OverlapBoxAll(GetAttackPoint(), _hitboxSize, 0, _enemyLayer);
        foreach (var e in enemies)
        {
            e.GetComponent<Enemy>()?.DealDamage(_damage);
        }

        
        yield return new WaitForSeconds(_recoveryTime);
        Debug.Log("ฟันเสร็จสิ้น");
    }
}
