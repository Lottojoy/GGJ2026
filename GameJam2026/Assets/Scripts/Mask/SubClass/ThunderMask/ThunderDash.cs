using UnityEngine;
using System.Collections;

/// <summary>
/// สกิล Dash สายฟ้าที่ใช้แรงส่งทางฟิสิกส์แทนการเคลื่อนที่ทีละช่อง
/// </summary>
public class ThunderDash : DashMask
{
    private Rigidbody2D _rb;

    protected override void Start()
    {
        base.Start();
        if (_movement != null) _rb = _movement.GetComponent<Rigidbody2D>();
    }

    protected override IEnumerator PerformDashRoutine(Vector3 dir)
    {
        _movement.LockMovement(true);
        if (_rb != null) _rb.velocity = (Vector2)dir * _dashForce;

        yield return new WaitForSeconds(_dashDuration);

        if (_rb != null) _rb.velocity = Vector2.zero;
        _movement.LockMovement(false);
    }
}