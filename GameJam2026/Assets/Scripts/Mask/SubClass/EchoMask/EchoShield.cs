using UnityEngine;
using System.Collections;

public class EchoShieldMask : ShieldMask
{
    [Header("Echo Shield Settings")]
    [SerializeField] private float pulseRadius = 1.8f;
    [SerializeField] private float pulseDamageMultiplier = 0.6f;
    [SerializeField] private LayerMask enemyLayer;

    protected override IEnumerator ShieldEffectRoutine()
    {
        _isInvincible = true;

        // เปิดโล่
        yield return new WaitForSeconds(_duration);
        _isInvincible = false;

        // ===== Echo Pulse =====
        MadnessResult result = RollMadness();
        float dmgMultiplier = pulseDamageMultiplier;

        if (result == MadnessResult.Good)
            dmgMultiplier = 1.2f;
        else if (result == MadnessResult.Bad)
            dmgMultiplier = 0.4f;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position, pulseRadius, enemyLayer
        );

        foreach (var h in hits)
        {
            if (h.TryGetComponent(out MobHp hp))
                hp.TakeDamage(Mathf.RoundToInt(GetFinalDamage() * dmgMultiplier));
        }
    }
}

