using UnityEngine;
using System.Collections;

public class EchoAttackMask : AttackMask
{
    [Header("Echo Settings")]
    [SerializeField] private float echoDelay = 0.5f;
    [SerializeField] private float echoDamageMultiplier = 0.7f;

    protected override IEnumerator ExecuteAttackRoutine()
    {
        if (_movement == null) yield break;

        _movement.LockMovement(true);
        yield return new WaitForSeconds(_anticipationTime);

        Vector2 attackPoint = GetAttackPoint();

        // ===== Hit แรก =====
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            attackPoint, _hitboxSize, 0f, _enemyLayer
        );

        foreach (var h in hits)
        {
            if (h.TryGetComponent(out MobHp hp))
                hp.TakeDamage(GetFinalDamage());
        }

        // ===== Echo Hit =====
        yield return new WaitForSeconds(echoDelay);

        MadnessResult result = RollMadness();
        float echoMultiplier = echoDamageMultiplier;

        if (result == MadnessResult.Good)
            echoMultiplier = 1.2f;
        else if (result == MadnessResult.Bad)
            echoMultiplier = 0.4f;

        Collider2D[] echoHits = Physics2D.OverlapBoxAll(
            attackPoint, _hitboxSize, 0f, _enemyLayer
        );

        foreach (var h in echoHits)
        {
            if (h.TryGetComponent(out MobHp hp))
                hp.TakeDamage(Mathf.RoundToInt(GetFinalDamage() * echoMultiplier));
        }

        yield return new WaitForSeconds(_recoveryTime);
        _movement.LockMovement(false);
    }
}