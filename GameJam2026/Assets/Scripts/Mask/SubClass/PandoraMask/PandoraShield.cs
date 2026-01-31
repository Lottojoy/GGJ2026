using UnityEngine;
using System.Collections;

// โล่ duration ลดลง
public class PandoraShieldMask : ShieldMask
{
    [Range(0f, 1f)] public float goodChance = 0.5f;
    [SerializeField] private int healAmount = 10;

    protected override IEnumerator ShieldEffectRoutine()
    {
        bool isGood = Random.value <= goodChance;

        _isInvincible = true;

        float duration = isGood ? _duration * 1.5f : _duration * 0.5f;

        if (isGood)
        {
            PlayerStats.Instance.Heal(healAmount);
        }

        yield return new WaitForSeconds(duration);

        _isInvincible = false;
    }
}
