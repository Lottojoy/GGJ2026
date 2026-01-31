using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    [Header("Mini Boss Settings")]
    public GameObject[] miniBossPrefabs;     // Prefab MiniBoss
    public int miniBossCount = 3;             // จำนวน MiniBoss
    public Transform[] spawnPositions;        // จุดเกิด MiniBoss
    public float spawnDelay = 1f;

    [Header("Big Boss Settings")]
    public GameObject bigBossPrefab;          // Prefab BigBoss
    public Transform bigBossSpawnPoint;

    [Header("Effects")]
    public GameObject spawnEffect;

    private List<GameObject> activeMiniBosses = new List<GameObject>();
    private bool allMiniBossesSpawned = false;
    private bool bigBossSpawned = false;

    void Start()
    {
        StartCoroutine(SpawnAllMiniBosses());
    }

    void Update()
    {
        if (allMiniBossesSpawned && !bigBossSpawned)
        {
            CheckMiniBossesStatus();
        }
    }

    // -------------------- Spawn MiniBoss --------------------
    private IEnumerator SpawnAllMiniBosses()
    {
        yield return new WaitForSeconds(spawnDelay);

        for (int i = 0; i < miniBossCount; i++)
        {
            SpawnMiniBoss();
            yield return new WaitForSeconds(0.5f);
        }

        allMiniBossesSpawned = true;
    }

    private void SpawnMiniBoss()
    {
       if (miniBossPrefabs.Length == 0) return;

    int bossIndex = Random.Range(0, miniBossPrefabs.Length);
    GameObject prefab = miniBossPrefabs[bossIndex];

    Vector3 spawnPos = GetRandomSpawnPosition();

    if (spawnEffect != null)
    {
        Instantiate(spawnEffect, spawnPos, Quaternion.identity);
    }

    GameObject miniBoss = Instantiate(prefab, spawnPos, Quaternion.identity);

    // ⭐ ตั้งชื่อ MiniBoss
    miniBoss.name = prefab.name + " (miniboss)";

    activeMiniBosses.Add(miniBoss);
    }

    // -------------------- Check MiniBoss --------------------
    private void CheckMiniBossesStatus()
    {
        for (int i = activeMiniBosses.Count - 1; i >= 0; i--)
        {
            if (activeMiniBosses[i] == null)
            {
                activeMiniBosses.RemoveAt(i);
            }
        }

        if (activeMiniBosses.Count == 0)
        {
            SpawnBigBoss();
            bigBossSpawned = true;
        }
    }

    // -------------------- Spawn BigBoss --------------------
    private void SpawnBigBoss()
    {
       if (bigBossPrefab == null) return;

    Vector3 spawnPos = bigBossSpawnPoint != null
        ? bigBossSpawnPoint.position
        : transform.position;

    if (spawnEffect != null)
    {
        Instantiate(spawnEffect, spawnPos, Quaternion.identity);
    }

    GameObject bigBoss = Instantiate(bigBossPrefab, spawnPos, Quaternion.identity);

    // ⭐ ตั้งชื่อ BigBoss
    bigBoss.name = bigBossPrefab.name + " (BOSS)";

    Debug.Log("Big Boss Spawned!");
    }

    // -------------------- Utility --------------------
    private Vector3 GetRandomSpawnPosition()
    {
        if (spawnPositions.Length > 0)
        {
            int index = Random.Range(0, spawnPositions.Length);
            return spawnPositions[index].position;
        }

        return transform.position + new Vector3(
            Random.Range(-5f, 5f),
            Random.Range(-3f, 3f),
            0f
        );
    }
}
