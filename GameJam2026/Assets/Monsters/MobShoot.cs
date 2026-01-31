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
    public Transform firePoint; // ให้สร้าง Empty Object เป็นลูกของมอนสเตอร์แล้วลากมาใส่
    public int shootAmount = 3;
    public float shootInterval = 0.4f; // ระยะห่างระหว่างนัด
    public float shootCooldown = 2f;   // ระยะห่างระหว่างชุด

    private Transform target;
    private bool isShooting = false;
    private Vector2 direction;

    void Start()
    {
        // ค้นหา Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }

        // ป้องกันกรณีลืมลาก firePoint ให้ใช้ตัวมอนสเตอร์เองไปก่อน
        if (firePoint == null)
        {
            firePoint = transform;
        }
    }

    void Update()
    {
        if (target == null) return;

        direction = (target.position - transform.position).normalized;
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget <= range)
        {
            // ยิงเส้นสีเขียวออกมาให้เห็นในหน้า Scene (ตอน Play เกม)
            Debug.DrawRay(transform.position, direction * range, Color.green);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, range, whatToHit);

            if (hit.collider != null)
            {
                // ดูว่ามันชนโดนอะไรกันแน่
                Debug.Log("Raycast hit: " + hit.collider.name);

                if (hit.collider.CompareTag("Player"))
                {
                    RotateTowardsTarget();
                    if (!isShooting) StartCoroutine(ShootRoutine());
                }
            }
        }
    }

    void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    IEnumerator ShootRoutine()
    {
        isShooting = true;

        for (int i = 0; i < shootAmount; i++)
        {
            Shoot();
            yield return new WaitForSeconds(shootInterval);
        }

        // รอ Cooldown หลังจากยิงครบชุดแล้ว
        yield return new WaitForSeconds(shootCooldown);
        isShooting = false;
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            // สร้างกระสุน
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Debug.Log("Monster Shot!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}