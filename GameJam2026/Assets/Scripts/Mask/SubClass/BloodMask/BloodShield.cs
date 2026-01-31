using UnityEngine;
using System.Collections;

public class BloodOathShieldMask : ShieldMask
{
    //madness good = ดูดแรง, madness = bad ดูดเลือดตัวเอง
    [Header("Blood Drain")]
    [SerializeField] private float drainRadius = 1.6f;
    [SerializeField] private int drainAmount = 5;
    [SerializeField] private LayerMask enemyLayer;

    protected override IEnumerator ShieldEffectRoutine()
    {
        _isInvincible = true;
        yield return new WaitForSeconds(_duration);
        _isInvincible = false;

        MadnessResult result = RollMadness();

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position, drainRadius, enemyLayer
        );

        if (result == MadnessResult.Bad)
        {
            // โทษ: ดูดเลือดตัวเอง
            PlayerStats.Instance.TakeDamage(drainAmount);
            yield break;
        }

        int healTotal = 0;

        foreach (var h in hits)
        {
            if (h.TryGetComponent(out MobHp hp))
            {
                hp.TakeDamage(drainAmount);
                healTotal += drainAmount;
            }
        }

        if (result == MadnessResult.Good)
            healTotal = Mathf.RoundToInt(healTotal * 1.5f);

        PlayerStats.Instance.Heal(healTotal);
    }
}
