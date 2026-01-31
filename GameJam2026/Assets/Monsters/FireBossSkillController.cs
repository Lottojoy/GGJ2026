using System.Collections;
using UnityEngine;

public class FireBossSkillController : MonoBehaviour
{
    public enum BossSkill
    {
        LaserSpin,
        FireRing
    }

    [Header("Common")]
    public MobMover mover;
    public Animator animator;
    public float cooldown = 3f;

    [Header("Warning Blink")]
    public float blinkInterval = 0.2f;

    [Header("Laser Spin")]
    public Transform laserPivot;
    public GameObject[] laserWarnings;
    public GameObject[] laserDamages;
    public float laserRotateSpeed = 90f;
    public float laserWarningTime = 1.2f;
    public float laserFireTime = 3f;

    [Header("Fire Ring")]
    public GameObject[] ringWarnings;
    public GameObject[] ringDamages;
    public float ringWarningTime = 1.2f;
    public float ringFireTime = 2f;

    bool isUsingSkill = false;
    float originalSpeed;

    [Header("Detection")]
public float detectRadius = 6f;
private Transform player;

    void Update()
    {
         if (isUsingSkill) return;
        if (!PlayerInRange()) return;

        UseRandomSkill();
    }   


    void Start()
{
    player = GameObject.FindGameObjectWithTag("Player")?.transform;

    originalSpeed = mover.moveSpeed;

    SetAll(laserWarnings, false);
    SetAll(laserDamages, false);
    SetAll(ringWarnings, false);
    SetAll(ringDamages, false);

    if (laserPivot != null)
        laserPivot.gameObject.SetActive(false);
}


    public void UseRandomSkill()
    {
         Debug.Log("Try use skill");
        if (!isUsingSkill)
            StartCoroutine(SkillRoutine());
    }

    private bool PlayerInRange()
    {
    if (player == null) return false;

    return Vector2.Distance(transform.position, player.position) <= detectRadius;
    }

    private IEnumerator SkillRoutine()
    {
        isUsingSkill = true;
        mover.moveSpeed = 0f;

        BossSkill skill = (BossSkill)Random.Range(0, 2);

        if (skill == BossSkill.LaserSpin)
            yield return StartCoroutine(LaserSpinRoutine());
        else
            yield return StartCoroutine(FireRingRoutine());

        mover.moveSpeed = originalSpeed;

        yield return new WaitForSeconds(cooldown);
        isUsingSkill = false;
    }

    // ================= LASER SPIN =================
    private IEnumerator LaserSpinRoutine()
    {
        animator?.SetTrigger("ChargeLaser");

        if (laserPivot != null)
            laserPivot.gameObject.SetActive(true);

        yield return StartCoroutine(BlinkWarning(laserWarnings, laserWarningTime));

        SetAll(laserWarnings, false);
        SetAll(laserDamages, true);

        float timer = 0f;
        while (timer < laserFireTime)
        {
            if (laserPivot != null)
                laserPivot.Rotate(0f, 0f, laserRotateSpeed * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        SetAll(laserDamages, false);

        if (laserPivot != null)
            laserPivot.gameObject.SetActive(false);
    }

    // ================= FIRE RING =================
    private IEnumerator FireRingRoutine()
    {
        animator?.SetTrigger("ChargeRing");

        yield return StartCoroutine(BlinkWarning(ringWarnings, ringWarningTime));

        SetAll(ringWarnings, false);
        SetAll(ringDamages, true);

        yield return new WaitForSeconds(ringFireTime);

        SetAll(ringDamages, false);
    }

    // ================= WARNING BLINK =================
    private IEnumerator BlinkWarning(GameObject[] warnings, float duration)
    {
        float timer = 0f;
        bool on = false;

        while (timer < duration)
        {
            on = !on;
            SetAll(warnings, on);

            timer += blinkInterval;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private void SetAll(GameObject[] objs, bool state)
    {
        foreach (var obj in objs)
            if (obj != null)
                obj.SetActive(state);
    }
    void OnDrawGizmosSelected()
{
    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(transform.position, detectRadius);
}
}
