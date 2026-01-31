using UnityEngine;
using System.Collections;

public abstract class AttackMask : MaskBase
{
    [Header("Attack Settings")]
    [SerializeField] protected int _damage = 25;
    [SerializeField] protected LayerMask _enemyLayer;
    [SerializeField] protected Vector2 _hitboxSize = new Vector2(0.8f, 0.8f);
    [SerializeField] protected float _offsetDistance = 1f;

    [Header("Timing Settings")]
    [SerializeField] protected float _anticipationTime = 0.2f; // เวลาก่อนฟัน (ง้าง)
    [SerializeField] protected float _recoveryTime = 0.1f;     // เวลาหลังฟัน (พัก)

    protected Vector2 _lookDir = Vector2.down;

    protected override void Update()
    {
        base.Update();
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (h != 0 || v != 0) _lookDir = new Vector2(h, v).normalized;
    }

    protected override void UseMask()
    {
        StartCooldown();
        StartCoroutine(ExecuteAttackRoutine());
    }

    // เปลี่ยนจากฟังก์ชันธรรมดาเป็น Coroutine เพื่อคุมจังหวะ
    protected abstract IEnumerator ExecuteAttackRoutine();

    protected Vector2 GetAttackPoint() => (Vector2)transform.position + (_lookDir * _offsetDistance);

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(GetAttackPoint(), _hitboxSize);
    }
}