using UnityEngine;
using System.Collections;

public class FireShieldMask : ShieldMask
{
    [Header("Fire Shield Settings")]
    [SerializeField] private int burnDamage = 1;
    [SerializeField] private float burnDuration = 2f;
    [SerializeField] private float shieldRadius = 1.2f;
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
                shieldRadius,
                enemyLayer
            );

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out BurnStatus burn))
                {
                    burn.ApplyBurn(burnDamage, burnDuration);
                }
            }

            yield return null;
        }

        _isInvincible = false;
    }
}
