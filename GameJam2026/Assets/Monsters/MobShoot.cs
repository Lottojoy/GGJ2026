using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobShoot : MonoBehaviour
{
    [Header("Detection Settings")]
    public float range = 10f;
    public LayerMask whatToHit;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public int shootAmount = 3;
    public float shootInterval = 0.4f;
    public float shootCooldown = 2f;

    private Transform target;
    private bool isShooting = false;
    private Vector2 direction;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) target = player.transform;
        if (firePoint == null) firePoint = transform;
    }

    void Update()
    {
        if (target == null) return;

        // คำนวณทิศทางไปยังผู้เล่น
        direction = (target.position - transform.position).normalized;

        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget <= range)
        {
            // ตรวจสอบ Line of Sight
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, range, whatToHit);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                // ลบ RotateTowardsTarget() ออกจากตรงนี้ เพื่อไม่ให้ตัวมอนหมุน
                if (!isShooting)
                {
                    StartCoroutine(ShootRoutine());
                }
            }
        }
    }

    IEnumerator ShootRoutine()
    {
        isShooting = true;

        for (int i = 0; i < shootAmount; i++)
        {
            Shoot();
            yield return new WaitForSeconds(shootInterval);
        }

        yield return new WaitForSeconds(shootCooldown);
        isShooting = false;
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            // 1. คำนวณมุมที่จะให้กระสุนพุ่งไป (อิงจาก direction)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);

            // 2. สร้างกระสุนโดยใช้มุมที่คำนวณได้ แทนการใช้ rotation ของมอนสเตอร์
            Instantiate(bulletPrefab, firePoint.position, bulletRotation);
            Debug.Log("Monster Shot without rotating body!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}