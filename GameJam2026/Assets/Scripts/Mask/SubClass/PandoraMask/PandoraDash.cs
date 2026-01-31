using UnityEngine;
using System.Collections;

public class PandoraDashMask : DashMask
{
    [Range(0f, 1f)] public float goodChance = 0.5f;

    protected override IEnumerator PerformDashRoutine(Vector3 dir)
    {
        bool isGood = Random.value <= goodChance;

        Rigidbody2D rb = _movement.GetComponent<Rigidbody2D>();
        if (rb == null) yield break;

        float dashForce = isGood ? _dashForce * 1.5f : _dashForce * 0.5f;
        float dashTime = isGood ? _dashDuration * 1.2f : _dashDuration * 0.6f;

        rb.velocity = dir * dashForce;

        yield return new WaitForSeconds(dashTime);

        rb.velocity = Vector2.zero;
    }
}
