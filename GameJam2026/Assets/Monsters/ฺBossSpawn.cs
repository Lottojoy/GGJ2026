using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    [Header("Mini Boss Settings")]
    public GameObject[] miniBossPrefabs;     // Prefab MiniBoss
    public int miniBossCount = 3;             // จำนวน MiniBoss
    public Transform[] spawnPoints;           // จุดเกิด (ห้ามซ้ำ)
    public float spawnDelay = 1f;

    [Header("Big Boss Settings")]
    public GameObject bigBossPrefab;          // Prefab BigBoss
    public Transform bigBossawnPoints;

    [Header("Effects")]
    public GameObject spawnEffect;

    // ---------------- Runtime ----------------
    private List<Transform> availableSpawnPoints = new List<Transform>();
    private List<GameObject> activeMiniBosses = new List<GameObject>();
    private bool bigBossSpawned = false;
    private bool miniBossSpawnFinished = false;
    void Start()
    {
        // เตรียม SpawnPoint ที่ยังว่าง
        availableSpawnPoints.AddRange(spawnPoints);

        StartCoroutine(SpawnMiniBossRoutine());
    }

    void Update()
    {
        if (miniBossSpawnFinished && !bigBossSpawned)
            CheckMiniBossesStatus();
    }

    // ---------------- MiniBoss ----------------
    private IEnumerator SpawnMiniBossRoutine()
{
    yield return new WaitForSeconds(spawnDelay);

    int spawnAmount = Mathf.Min(miniBossCount, availableSpawnPoints.Count);

    for (int i = 0; i < spawnAmount; i++)
    {
        SpawnMiniBoss();
        yield return new WaitForSeconds(0.5f);
    }

    // ✅ จุดสำคัญ
    miniBossSpawnFinished = true;
}


    private void SpawnMiniBoss()
    {
        if (miniBossPrefabs.Length == 0 || availableSpawnPoints.Count == 0)
            return;

        // สุ่ม Prefab
        GameObject prefab = miniBossPrefabs[Random.Range(0, miniBossPrefabs.Length)];

        // สุ่มจุดที่ยังไม่ถูกใช้
        int pointIndex = Random.Range(0, availableSpawnPoints.Count);
        Transform spawnPoint = availableSpawnPoints[pointIndex];
        availableSpawnPoints.RemoveAt(pointIndex);

        if (spawnEffect != null)
            Instantiate(spawnEffect, spawnPoint.position, Quaternion.identity);

        GameObject miniBoss = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        miniBoss.name = prefab.name + " (miniboss)";

        activeMiniBosses.Add(miniBoss);
    }

    // ---------------- Check ----------------
    private void CheckMiniBossesStatus()
    {
        for (int i = activeMiniBosses.Count - 1; i >= 0; i--)
        {
            if (activeMiniBosses[i] == null)
                activeMiniBosses.RemoveAt(i);
        }

        if (activeMiniBosses.Count == 0 && !bigBossSpawned)
        {
            SpawnBigBoss();
            bigBossSpawned = true;
        }
    }

    // ---------------- BigBoss ----------------
    private void SpawnBigBoss()
    {
        if (bigBossPrefab == null || availableSpawnPoints.Count == 0)
            return;

        

        if (spawnEffect != null)
            Instantiate(spawnEffect, bigBossawnPoints.position, Quaternion.identity);

        GameObject bigBoss = Instantiate(bigBossPrefab, bigBossawnPoints.position, Quaternion.identity);
        bigBoss.name = bigBossPrefab.name + " (BOSS)";

        Debug.Log("Big Boss Spawned!");
    }
}
