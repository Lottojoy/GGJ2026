using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobMover : MonoBehaviour
{
   public float moveSpeed = 5f;

    public float separationDistance = 1f; // ระยะห่างขั้นต่ำ
    public float separationStrength = 2f; // ความแรงในการหลบกัน
    public SpriteRenderer spriteRenderer;
    public bool switchFilp = false;
    private Transform player; // ไม่ต้อง assign ใน Inspector แล้ว

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // ค้นหา GameObject ที่มี Tag เป็น "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Player object with tag 'Player' not found!");
        }
    }

    void FixedUpdate()
    {
        if (player == null)
            return;

        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        Vector2 separation = Vector2.zero;

        // หามอนสเตอร์อื่นในฉาก
        GameObject[] allMobs = GameObject.FindGameObjectsWithTag("Mob");

        foreach (GameObject mob in allMobs)
        {
            if (mob == this.gameObject) continue;

            float distance = Vector2.Distance(transform.position, mob.transform.position);
            if (distance < separationDistance)
            {
                // เพิ่มเวกเตอร์ผลักจากตัวอื่น
                Vector2 pushDir = (transform.position - mob.transform.position).normalized;
                separation += pushDir * (separationDistance - distance);
            }
        }

        // รวมเวกเตอร์การเดินไปยังผู้เล่นกับการหลบกัน
        Vector2 finalDir = (directionToPlayer + separationStrength * separation).normalized;

        rb.velocity = finalDir * moveSpeed;
        FacePlayer();
    }
    private void FacePlayer()
    {
        if (switchFilp)
        {
//_HACKATHON_API_ ตรวจสอบทิศทางโดยเทียบตำแหน่งแกน X
        if (player.position.x > transform.position.x)
        {
            // ถ้า Player อยู่ทาง "ขวา" ของมอนสเตอร์
            // ให้หันขวา (ภาพปกติ ไม่ต้อง Flip)
            spriteRenderer.flipX = true;
        }
        else if (player.position.x < transform.position.x)
        {
            // ถ้า Player อยู่ทาง "ซ้าย" ของมอนสเตอร์
            // ให้หันซ้าย (กลับด้านภาพตามแกน X)
            spriteRenderer.flipX = false;
        }
        // กรณีที่ตำแหน่ง X เท่ากันพอดี Sprite จะไม่เปลี่ยนทิศทาง ซึ่งเป็นพฤติกรรมที่ถูกต้อง
        }
        else
        {
            //_HACKATHON_API_ ตรวจสอบทิศทางโดยเทียบตำแหน่งแกน X
        if (player.position.x > transform.position.x)
        {
            // ถ้า Player อยู่ทาง "ขวา" ของมอนสเตอร์
            // ให้หันขวา (ภาพปกติ ไม่ต้อง Flip)
            spriteRenderer.flipX = false;
        }
        else if (player.position.x < transform.position.x)
        {
            // ถ้า Player อยู่ทาง "ซ้าย" ของมอนสเตอร์
            // ให้หันซ้าย (กลับด้านภาพตามแกน X)
            spriteRenderer.flipX = true;
        }
        // กรณีที่ตำแหน่ง X เท่ากันพอดี Sprite จะไม่เปลี่ยนทิศทาง ซึ่งเป็นพฤติกรรมที่ถูกต้อง
        }
        
    }

}
