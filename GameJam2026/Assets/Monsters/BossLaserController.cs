using UnityEngine;
using System.Collections;

public class BossLaserController : MonoBehaviour
{
    [Header("Laser Objects")]
    public Transform laserPivot;
    public GameObject[] warningLasers;
    public GameObject[] damageLasers;

    [Header("Timing")]
    public float warningTime = 1.2f;
    public float laserTime = 2f;
    public float rotateSpeed = 90f;

    public MobMover mover;

    [Header("Warning Blink")]
public float warningBlinkInterval = 0.2f; // ความเร็วการกะพริบ
    private float originalSpeed;

    void Start()
    {
        originalSpeed = mover.moveSpeed;

        SetAll(warningLasers, false);
        SetAll(damageLasers, false);
        laserPivot.gameObject.SetActive(false);
    }

    public void FireLaser()
    {
        StartCoroutine(LaserRoutine());
    }

    private IEnumerator LaserRoutine()
{
    mover.moveSpeed = 0f;

    laserPivot.gameObject.SetActive(true);

    // ⚠️ WARNING กะพริบ
    float warnTimer = 0f;
    bool isOn = false;

    while (warnTimer < warningTime)
    {
        isOn = !isOn;
        SetAll(warningLasers, isOn);

        warnTimer += warningBlinkInterval;
        yield return new WaitForSeconds(warningBlinkInterval);
    }

    // 🔥 ยิงจริง
    SetAll(warningLasers, false);
    SetAll(damageLasers, true);

    float fireTimer = 0f;
    while (fireTimer < laserTime)
    {
        laserPivot.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
        fireTimer += Time.deltaTime;
        yield return null;
    }

    SetAll(damageLasers, false);
    laserPivot.gameObject.SetActive(false);

    mover.moveSpeed = originalSpeed;
}


    private void SetAll(GameObject[] objs, bool state)
    {
        foreach (var obj in objs)
            if (obj != null)
                obj.SetActive(state);
    }
}
