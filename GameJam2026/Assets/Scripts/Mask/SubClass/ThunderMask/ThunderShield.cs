using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderShield : ShieldMask
{
    protected override IEnumerator ShieldEffectRoutine()
    {
        PlayerStats.Instance.SetInvincible(true);
        Debug.Log("Shield Active");
        yield return new WaitForSeconds(_duration);
        PlayerStats.Instance.SetInvincible(false);
        Debug.Log("Shield Expired");
    }
}
