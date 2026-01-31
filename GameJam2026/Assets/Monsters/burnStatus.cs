using System.Collections;
using UnityEngine;

public class BurnStatus : MonoBehaviour
{
    public float tickInterval = 1f;

    Coroutine burnRoutine;
    MobHp mobHp;

    void Awake()
    {
        mobHp = GetComponent<MobHp>();
    }
 
    public void ApplyBurn(int damagePerTick, float duration)
    {
        if (mobHp == null) return;

        if (burnRoutine != null)
            StopCoroutine(burnRoutine);

        burnRoutine = StartCoroutine(BurnCoroutine(damagePerTick, duration));
    }

    IEnumerator BurnCoroutine(int damage, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            mobHp.TakeDamage(damage);
            elapsed += tickInterval;
            yield return new WaitForSeconds(tickInterval);
        }

        burnRoutine = null;
    }
}
