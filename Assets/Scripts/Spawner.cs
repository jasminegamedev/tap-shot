using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpawnObject
{
    [Tooltip("How likely is it that this object will spawn.")]
    public float chance;
    [Tooltip("Game object to spawn.")]
    public GameObject go;
}

[System.Serializable]
public struct SpawnWave
{
    [Tooltip("How many points does the player need to be in this wave.")]
    public int reqScore;
    [Tooltip("List of objects that can be spawned in this wave.")]
    public List<SpawnObject> spawnObjects;
}

// Component used for regularly spawning game objects using custom spawn wave system.
public class Spawner : MonoBehaviour
{
    [Tooltip("How often do we spawn a new wall of objects.")]
    public float spawnInterval = 2;

    [Tooltip("List of Object Spawn Points.")]
    public List<GameObject> SpawnPoints = new List<GameObject>();

    [Tooltip("List of Spawn Wave structures.")]
    public List<SpawnWave> SpawnWaves = new List<SpawnWave>();

    void Start()
    {
        StartCoroutine("SpawnObstacles");
    }

    // Spawn a wall of obstacles every few seconds.
    IEnumerator SpawnObstacles()
    {
        yield return new WaitForSeconds(spawnInterval);
        int SpawnCount = 0;
        if (GameManager.Instance.isStarted && !GameManager.Instance.isGameOver)
        {
            SpawnWave wave = GetSpawnWave();
            foreach (GameObject spawnpoint in SpawnPoints)
            {
                if (Random.Range(0, 2) == 1 && SpawnCount != SpawnPoints.Count - 1)
                {
                    var spawnObject = GetSpawnObject(wave);
                    if(spawnObject != null)
                    {
                        Instantiate(spawnObject, spawnpoint.transform.position, spawnpoint.transform.rotation);
                        SpawnCount++;
                    }
                }
            }
        }
        StartCoroutine("SpawnObstacles");
    }

    // Get the current spawn wave structure.
    private SpawnWave GetSpawnWave()
    {
        for(int i = SpawnWaves.Count - 1; i >= 0; i--)
        {
            if (GameManager.Instance.Score >= SpawnWaves[i].reqScore)
            {
                return SpawnWaves[i];
            }
        }
        return SpawnWaves[SpawnWaves.Count - 1];
    }

    // Get a random spawn object for the current wave.
    private GameObject GetSpawnObject(SpawnWave wave)
    {
        float waveTotal = 0;
        foreach (SpawnObject obj in wave.spawnObjects)
        {
            waveTotal += obj.chance;
        }

        float rand = Random.Range(0f, waveTotal);

        float startPoint = 0;
        foreach(SpawnObject obj in wave.spawnObjects)
        {
            if(rand >= startPoint && rand <= startPoint + obj.chance)
            {
                return obj.go;
            }
            else
            {
                startPoint += obj.chance;
            }
        }
        return null;
    }
}
