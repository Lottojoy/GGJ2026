using UnityEngine;
using System.Collections;

public class PandoraAttackMask : AttackMask
{
    [Header("Pandora Settings")]
    [Range(0f, 1f)] public float goodChance = 0.5f;

    [SerializeField] private int selfDamage = 5;
    [SerializeField] private int bonusBurnDamage = 3;
    [SerializeField] private float bonusBurnDuration = 2f;

    protected override IEnumerator ExecuteAttackRoutine()
    {
        yield return new WaitForSeconds(_anticipationTime);

        bool isGood = Random.value <= goodChance;
        Vector2 attackPoint = GetAttackPoint();

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            attackPoint,
            _hitboxSize,
            0f,
            _enemyLayer
        );

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out MobHp mobHp))
            {
                int finalDamage = isGood ? _damage * 2 : _damage / 2;
                mobHp.TakeDamage(GetFinalDamage());
            }

            if (isGood && hit.TryGetComponent(out BurnStatus burn))
            {
                burn.ApplyBurn(bonusBurnDamage, bonusBurnDuration);
            }
        }

        if (!isGood)
        {
            // Penalty: ผู้เล่นเจ็บเอง
            PlayerStats.Instance.TakeDamage(selfDamage);
        }

        yield return new WaitForSeconds(_recoveryTime);
    }
}
