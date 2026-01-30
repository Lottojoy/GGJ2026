using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobHp : MonoBehaviour
{
   [Header("Health Settings")]
    public int maxHp = 100;
    public int currentHp;

    [Header("Flash Effect")]
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

   

    [Header("Item Drops")]
    public GameObject ItemPrefab; // (ใหม่) Prefab ของ Health Potion
    [Range(0, 100)]
    public float ItemDropChance = 5f; // (ใหม่) โอกาสดรอป Potion (เป็นเปอร์เซ็นต์)

    // --- ตัวแปรภายใน ---
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private bool isInvulnerable = false; // ตัวแปรตรวจสอบสถานะไร้เทียมทาน
    private float invulnerabilityDuration = 0.5f; // ระยะเวลาไร้เทียมทาน
    private Coroutine invulnerabilityCoroutine;

    void Awake()
    {
        currentHp = maxHp;
        spriteRenderer = GetComponent<SpriteRenderer>();
    

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    // --- ฟังก์ชัน TakeDamage (เหมือนเดิมจากโค้ดของคุณ) ---
    public void TakeDamage(int damage)
    {
        if (isInvulnerable)
        {
            Debug.Log(gameObject.name + " is invulnerable, damage ignored!");
            return; // ไม่รับความเสียหาย
        }

        currentHp -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage, current HP: " + currentHp);

        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }
        else
        {
            StartCoroutine(FlashEffect());
            // เริ่มสถานะไร้เทียมทาน
            StartCoroutine(InvulnerabilityPeriod());
        }
    }

    // --- ระบบไร้เทียมทาน ---
    private IEnumerator InvulnerabilityPeriod()
    {
        isInvulnerable = true;
        
        // ตัวเลือก 1: เปลี่ยนสีชั่วคราวเพื่อแสดงสถานะ
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f); // ทำให้โปร่งแสง
        }

        // รอระยะเวลาไร้เทียมทาน
        yield return new WaitForSeconds(invulnerabilityDuration);

        // คืนค่าปกติ
        isInvulnerable = false;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
        
        Debug.Log(gameObject.name + " is vulnerable again.");
    }
    


    // --- ฟังก์ชัน FlashEffect (เหมือนเดิมจากโค้ดของคุณ) ---
    private IEnumerator FlashEffect()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    // --- ฟังก์ชัน Die (เหมือนเดิมจากโค้ดของคุณทุกประการ) ---
    private void Die()
    {
        Debug.Log(gameObject.name + " has died.");
        
        

        float randomValue = Random.Range(0f, 100f); 

        // ถ้าเลขที่สุ่มได้น้อยกว่าหรือเท่ากับโอกาสดรอปที่ตั้งไว้
        if (randomValue <= ItemDropChance)
        {
            if (ItemPrefab != null)
            {
                // สร้าง Potion ขึ้นมา
                Debug.Log("Health Potion Dropped!");
                Instantiate(ItemPrefab, transform.position, Quaternion.identity);
            }
        }
        
        Destroy(gameObject);
    }
}
