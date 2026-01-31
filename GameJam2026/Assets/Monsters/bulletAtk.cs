using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletAtk : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 3f; // เวลาก่อนกระสุนจะถูกทำลาย
    public int Damage = 10;

    void Start()
    {
        // ทำลายกระสุนหลังจากเวลาผ่านไป lifeTime วินาที
        // เพื่อไม่ให้กระสุนรกอยู่ใน Scene
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // เคลื่อนที่กระสุนไปข้างหน้า (ตามแกน Y ของมัน)
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    // ฟังก์ชันนี้จะทำงานเมื่อกระสุนชนกับ Collider อื่น
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ถ้าชน Player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit Player!");
            /*PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
            playerHealth.TakeDamage(Damage);
            }*/
            Destroy(gameObject); // ทำลายกระสุนทันที
        }
        // ถ้าชนอย่างอื่นที่ไม่ใช่ Enemy หรือตัวปืนเอง
        else if (!other.CompareTag("Mob"))
        {
            Destroy(gameObject); // ทำลายกระสุนเมื่อชนกำแพงหรือสิ่งกีดขวาง
        }
    }
}
