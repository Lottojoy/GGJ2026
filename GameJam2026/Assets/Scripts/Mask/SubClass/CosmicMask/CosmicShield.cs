using UnityEngine;
using System.Collections;

public class CosmicShieldMask : ShieldMask
{
    [Header("Cosmic Shield Settings")]
    [SerializeField] private float gravityRadius = 2f;
    [SerializeField] private float pullForce = 5f;
    [SerializeField] private LayerMask enemyLayer;

    protected override IEnumerator ShieldEffectRoutine()
    {
        _isInvincible = true;

        float timer = 0f;

        while (timer < _duration)
        {
            timer += Time.deltaTime;

            Collider2D[] hits = Physics2D.OverlapCircleAll(
                transform.position,
                gravityRadius,
                enemyLayer
            );

            foreach (var hit in hits)
            {
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 dir =
                        (transform.position - hit.transform.position).normalized;

                    rb.AddForce(dir * pullForce * Time.deltaTime,
                                ForceMode2D.Force);
                }
            }

            yield return null;
        }

        _isInvincible = false;
    }
}
