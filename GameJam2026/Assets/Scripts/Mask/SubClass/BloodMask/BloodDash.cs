using UnityEngine;
using System.Collections;

public class BloodOathDashMask : DashMask
{
    [Header("Blood Cost")]
    [SerializeField] private int dashHpCost = 4;

    protected override IEnumerator PerformDashRoutine(Vector3 dir)
    {
        Rigidbody2D rb = _movement.GetComponent<Rigidbody2D>();
        if (rb == null) yield break;

        MadnessResult result = RollMadness();

        float finalForce = _dashForce;
        int finalCost = dashHpCost;

        if (result == MadnessResult.Good)
        {
            finalForce *= 1.8f;
            finalCost = 0; // ไม่เสียเลือด
        }
        else if (result == MadnessResult.Bad)
        {
            finalForce *= 0.6f;
        }

        // Pay cost
        if (finalCost > 0)
            PlayerStats.Instance.TakeDamage(finalCost);

        rb.velocity = dir * finalForce;
        yield return new WaitForSeconds(_dashDuration);
        rb.velocity = Vector2.zero;
    }
}
