using UnityEngine;
using System.Collections;

public class EchoDashMask : DashMask
{
    [Header("Echo Dash Settings")]
    [SerializeField] private float echoDelay = 0.3f;
    [SerializeField] private float echoForceMultiplier = 0.7f;

    protected override IEnumerator PerformDashRoutine(Vector3 dir)
    {
        Rigidbody2D rb = _movement.GetComponent<Rigidbody2D>();
        if (rb == null) yield break;

        //dash แรก
        rb.velocity = dir * _dashForce;
        yield return new WaitForSeconds(_dashDuration);
        rb.velocity = Vector2.zero;

        //dash echo
        yield return new WaitForSeconds(echoDelay);

        MadnessResult result = RollMadness();
        float echoForce = _dashForce * echoForceMultiplier;

        if (result == MadnessResult.Good)
            echoForce = _dashForce * 1.2f;
        else if (result == MadnessResult.Bad)
            echoForce = _dashForce * 0.4f;

        rb.velocity = dir * echoForce;
        yield return new WaitForSeconds(_dashDuration * 0.6f);
        rb.velocity = Vector2.zero;
    }
}
