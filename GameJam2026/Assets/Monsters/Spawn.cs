using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject[] Mob_spawn1;
     public int[] initialRates; // อัตราเริ่มต้น
    public int[] currentRates; // อัตราปัจจุบัน (จะเปลี่ยนแปลงตามเวลา)
    
     [Header("Rate Change Over Time")]
    public float rateChangeDuration = 300f; // เวลาทั้งหมดในการเปลี่ยน rates (300 วินาที = 5 นาที)
    public AnimationCurve rateChangeCurve = AnimationCurve.Linear(0, 0, 1, 1); // Curve สำหรับการเปลี่ยนค่า
    public GameObject[] Block_spawn;

    [Header("Spawn Timing")]
    public float currentInterval;
    private float timerSpawn;
    public float startInterval = 2f;  // เวลาเริ่มต้นในการเสก (วินาที)
    public float minInterval = 0.2f;  // ความถี่ต่ำสุดที่ยอมให้เสกได้
    public float decreaseRate = 0.05f; // ลดความถี่ทุกครั้งหลังเสก

    [Header("Game Time")]
    private float gameTime = 0f;
    public bool autoUpdateRates = true; // เปิด/ปิดการปรับ rates อัตโนมัติ

    int index_Block;
    int totalRate;
    int randomPoint;


    // Update is called once per frame
    void Start()
    {
        currentInterval = startInterval;
        
        currentRates = new int[initialRates.Length];
        for (int i = 0; i < initialRates.Length; i++)
        {
            currentRates[i] = initialRates[i];
        }
    }

    void Update()
    {
        // อัพเดทเวลาเกม
        gameTime += Time.deltaTime;
        
        // ปรับเปลี่ยน rates ตามเวลา (ถ้าเปิดใช้งาน)
        if (autoUpdateRates)
        {
            UpdateRatesOverTime();
        }
        
        timerSpawn += Time.deltaTime;

        if (timerSpawn >= currentInterval)
        {
             SpawnMob();
            timerSpawn = 0f;
            currentInterval = Mathf.Max(minInterval, currentInterval - decreaseRate);

        }
       
        
        
    }
    // ฟังก์ชันปรับเปลี่ยน rates ตามเวลา
    private void UpdateRatesOverTime()
    {
        // คำนวณ progress (0 ถึง 1)
        float progress = Mathf.Clamp01(gameTime / rateChangeDuration);
        
        // ใช้ Curve เพื่อให้การเปลี่ยนแปลงราบรื่น
        float curveProgress = rateChangeCurve.Evaluate(progress);
        
        // ตัวอย่าง: สำหรับ 2 mobs
        if (currentRates.Length == 2)
        {
            // MobA: ลดจาก initialRates[0] ลงเหลือ 0
            currentRates[0] = (int)Mathf.Lerp(initialRates[0], 0, curveProgress);
            
            // MobB: เพิ่มจาก initialRates[1] ขึ้นเป็น 100
            // แต่ต้องคำนวณให้ผลรวมใกล้เคียง 100
            currentRates[1] = (int)Mathf.Lerp(initialRates[1], 100, curveProgress);
            
            // ป้องกันค่าติดลบ
            currentRates[0] = Mathf.Max(0, currentRates[0]);
            currentRates[1] = Mathf.Min(100, currentRates[1]);
        }
    }
     // ฟังก์ชัน Spawn Mob
    private void SpawnMob()
    {
        // สุ่มตำแหน่ง spawn
        index_Block = Random.Range(0, Block_spawn.Length);
        
        // คำนวณ total rate
        totalRate = 0;
        for (int i = 0; i < currentRates.Length; i++)
        {
            totalRate += currentRates[i];
        }
        
        // ถ้า totalRate เป็น 0 ไม่ต้อง spawn
        if (totalRate <= 0)
        {
            Debug.LogWarning("Total rate is 0, no mobs will spawn!");
            return;
        }
        
        // สุ่มเลือก mob
        randomPoint = Random.Range(0, totalRate);
        
        for (int i = 0; i < currentRates.Length; i++)
        {
            if (randomPoint < currentRates[i])
            {
                Instantiate(Mob_spawn1[i], Block_spawn[index_Block].transform.position, Quaternion.identity);
                break;
            }
            else
            {
                randomPoint -= currentRates[i];
            }
        }
    }
    
    // ฟังก์ชันช่วยเหลือ: คำนวณผลรวม rates ของ mobs อื่นๆ
    private int GetSumOfOtherRates(int excludeIndex)
    {
        int sum = 0;
        for (int i = 0; i < currentRates.Length; i++)
        {
            if (i != excludeIndex)
            {
                sum += currentRates[i];
            }
        }
        return sum;
    }
    
    // ปรับ rates ให้ผลรวมประมาณ 100
    private void NormalizeRates()
    {
        int total = 0;
        for (int i = 0; i < currentRates.Length; i++)
        {
            total += currentRates[i];
        }
        
        // ถ้าผลรวมเกิน 100 ปรับลด
        if (total > 100)
        {
            float scale = 100f / total;
            for (int i = 0; i < currentRates.Length; i++)
            {
                currentRates[i] = Mathf.RoundToInt(currentRates[i] * scale);
            }
        }
    }
}
