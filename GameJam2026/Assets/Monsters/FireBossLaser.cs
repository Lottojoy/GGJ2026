using System.Collections;
using UnityEngine;

public class FireBossLaser : MonoBehaviour
{
    public Animator animator;
    public MobMover mover;
    public BossLaserController laserController;

    [Header("Detection")]
    public float detectRadius = 6f;

    [Header("Timing")]
    public float chargeTime = 1.5f;
    public float cooldown = 3f;

    private Transform player;
    private bool isAttacking = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isAttacking) return;
        if (!PlayerInRange()) return;

        StartCoroutine(AttackRoutine());
    }

    private bool PlayerInRange()
    {
        return Vector2.Distance(transform.position, player.position) <= detectRadius;
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        // 🎭 เตรียมยิง
        if (animator != null)
            animator.SetTrigger("ChargeLaser");

        yield return new WaitForSeconds(chargeTime);

        // 🔥 ยิงเลเซอร์จริง
        laserController.FireLaser();

        yield return new WaitForSeconds(cooldown);
        isAttacking = false;
    }
}
