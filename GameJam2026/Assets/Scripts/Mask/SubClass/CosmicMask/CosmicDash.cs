using UnityEngine;
using System.Collections;

public class CosmicDashMask : DashMask
{
    [Header("Cosmic Dash Settings")]
    [SerializeField] private float pullRadius = 1.5f;
    [SerializeField] private float pullForce = 6f;
    [SerializeField] private LayerMask enemyLayer;

    protected override IEnumerator PerformDashRoutine(Vector3 dir)
    {
        Rigidbody2D rb = _movement.GetComponent<Rigidbody2D>();
        if (rb == null) yield break;

        float timer = 0f;
        rb.velocity = dir * _dashForce;

        while (timer < _dashDuration)
        {
            timer += Time.deltaTime;

            Collider2D[] hits = Physics2D.OverlapCircleAll(
                _movement.transform.position,
                pullRadius,
                enemyLayer
            );

            foreach (var hit in hits)
            {
                Rigidbody2D enemyRb = hit.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    Vector2 dirToPlayer =
                        (_movement.transform.position - hit.transform.position).normalized;

                    enemyRb.AddForce(dirToPlayer * pullForce * Time.deltaTime,
                                     ForceMode2D.Force);
                }
            }

            yield return null;
        }

        rb.velocity = Vector2.zero;
    }
}
