using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobShoot : MonoBehaviour
{
    public float Range;
    public Transform Target;

    // --- เพิ่มตัวแปรเหล่านี้เข้าไป ---
    public GameObject bulletPrefab; // Prefab ของกระสุน
    private Transform firePoint;     // จุดที่จะให้กระสุนออกมา

    public int Shoot_amout;
    public int Shoot_cooldown;

    // -----------------------------
    public LayerMask whatToHit; // Layer ที่จะให้ Raycast ตรวจจับ+
    bool Detected = false;
    bool onShort = false;
    Vector2 Direction;

    void Start()
    {
        if (Target == null)
        {
            Target = GameObject.FindGameObjectWithTag("Player").transform;
            firePoint = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (Target == null) return;

        Vector2 targetPos = Target.position;
        Direction = targetPos - (Vector2)transform.position;

        // --- แก้ไขบรรทัด Raycast ---
        // เพิ่ม whatToHit เข้าไปเป็นพารามิเตอร์ตัวที่ 4
        RaycastHit2D rayInfo = Physics2D.Raycast(firePoint.position, Direction, Range, whatToHit);
        // -------------------------

        // ไม่ต้องเช็ค tag แล้ว เพราะ LayerMask จัดการให้แล้ว
        if (rayInfo.collider != null)
        {
            if (Detected == false)
            {
                Detected = true;
                Debug.Log("เจอ Player");
            }
        }
        else
        {
            if (Detected == true)
            {
                Detected = false;
            }
        }

        // --- เพิ่มส่วนของการยิงและหันปืน ---
        if (Detected && onShort == false)
        {
            // ทำให้ปืนหันไปหา Player
            // การ -90f เพราะ Sprite ของเราโดยปกติจะหันขึ้นด้านบน (แกน Y)
            onShort = true;
             StartCoroutine(Oncooldown());
            

        }
        // --------------------------------
    }

    // --- เพิ่มฟังก์ชันนี้เข้าไป ---
    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            // สร้างกระสุน (Instantiate) จาก Prefab ที่ตำแหน่งและทิศทางของ firePoint
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
        else
        {
            Debug.LogError("Bullet Prefab หรือ Fire Point ยังไม่ได้ตั้งค่า!");
        }
    }
    // ---------------------------

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }

    IEnumerator Oncooldown()
    {

        yield return new WaitForSeconds(Shoot_cooldown);
        for (int i = 0; i < Shoot_amout; i++)
        {
            Debug.Log("ยิง");
            float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            Shoot(); // เรียกฟังก์ชันยิง
                yield return new WaitForSeconds(0.4f);
        }
            onShort = false;
    }
}
