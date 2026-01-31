using UnityEngine;
using System.Collections;

public abstract class AttackMask : MaskBase
{
    [Header("Attack Settings")]
    [SerializeField] protected int _damage = 25;
    [SerializeField] protected LayerMask _enemyLayer;
    [SerializeField] protected Vector2 _hitboxSize = new Vector2(1.2f, 1.2f);
    [SerializeField] protected float _offsetDistance = 1.2f;

    [Header("Timing")]
    [SerializeField] protected float _anticipationTime = 0.15f;
    [SerializeField] protected float _recoveryTime = 0.1f;

    protected PlayerMovement _movement;

    protected override void Start()
    {
        base.Start();
        _movement = FindAnyObjectByType<PlayerMovement>();
    }

    protected override void UseMask()
    {
        StartCooldown();
        StartCoroutine(ExecuteAttackRoutine());
    }

    protected abstract IEnumerator ExecuteAttackRoutine();

    // จุดสำคัญ: คำนวณตำแหน่งโจมตีจากตัว Player ไม่ใช่จาก UI
    protected Vector2 GetAttackPoint()
    {
        if (_movement == null) return Vector2.zero;
        return (Vector2)_movement.transform.position + (_movement.LastMoveDirection * _offsetDistance);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(GetAttackPoint(), _hitboxSize);
    }
}