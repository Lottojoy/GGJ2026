using UnityEngine;
using System.Collections;

public abstract class DashMask : MaskBase
{
    [Header("Dash Settings")]
    [SerializeField] protected float _dashForce = 20f;
    [SerializeField] protected float _dashDuration = 0.2f;
    protected PlayerMovement _movement;

    protected override void Start()
    {
        base.Start();
        _movement = FindAnyObjectByType<PlayerMovement>();
    }

    protected override bool CanUseMask() => _movement != null;

    protected override void UseMask()
    {
        Vector2 dashDir = _movement.CurrentInput.normalized;
        if (dashDir == Vector2.zero) dashDir = _movement.LastMoveDirection;

        StartCooldown();
        StartCoroutine(PerformDashRoutine(dashDir));
    }

    protected abstract IEnumerator PerformDashRoutine(Vector3 dir);
}