using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShieldMask : MaskBase
{
    [Header("Shield Settings")]
    [SerializeField] protected float _duration = 2f;

    protected override void UseMask()
    {
        StartCooldown();
        StartCoroutine(ShieldEffectRoutine());
    }

    protected abstract IEnumerator ShieldEffectRoutine();
}

