using UnityEngine;
using System.Collections;

/// <summary>
/// คลาสฐานสำหรับหน้ากากสายแดช จัดการทิศทางและการเริ่ม Cooldown
/// </summary>
public abstract class DashMask : MaskBase
{
    [Header("Dash Settings")]
    [SerializeField] protected float _dashSpeed = 20f;
    [SerializeField] protected int _dashDistance = 3;
    [SerializeField] protected PlayerGridMovement _movement;

    protected override bool CanUseMask() => _movement != null;

    protected override void UseMask()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dashDir = Vector3.zero;

        // กรองทิศทาง: เลือกแกนที่กดชัดเจนที่สุดเพียงอย่างเดียวเพื่อความแม่นยำ
        if (Mathf.Abs(h) > Mathf.Abs(v)) dashDir.x = h > 0 ? 1 : -1;
        else if (Mathf.Abs(v) > 0) dashDir.y = v > 0 ? 1 : -1;

        // ถ้าไม่กดทิศทาง ให้ดึงทิศจากความต่างของ MovePoint กับตัวละคร
        if (dashDir == Vector3.zero)
        {
            Vector3 diff = _movement.MovePoint.position - transform.position;
            if (diff.magnitude > 0.1f)
                dashDir = new Vector3(Mathf.Abs(diff.x) > Mathf.Abs(diff.y) ? (diff.x > 0 ? 1 : -1) : 0,
                                      Mathf.Abs(diff.y) >= Mathf.Abs(diff.x) ? (diff.y > 0 ? 1 : -1) : 0, 0);
            else dashDir = Vector3.down;
        }

        StartCooldown();
        StartCoroutine(PerformDashRoutine(dashDir));
    }

    protected abstract IEnumerator PerformDashRoutine(Vector3 dir);
}