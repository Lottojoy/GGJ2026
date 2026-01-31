using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderShield : ShieldMask
{
    protected override IEnumerator ShieldEffectRoutine()
    {
        _isInvincible = true;
        yield return new WaitForSeconds(_duration);
        _isInvincible = false;
    }
}
