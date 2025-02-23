using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator main;
    void Awake()
    {
        main = this;
    }

    private int currentLevel = -1;

    [SerializeField]
    private Transform worldContainer;
    [SerializeField]
    private List<LevelGeneratorLevel> levels = new();
    public void NextLevel() {
        int nextLevel = currentLevel + 1;
        Debug.Log($"Next level: {nextLevel}");
        if (levels.Count > currentLevel) {
            currentLevel  = nextLevel;
            var level = levels[currentLevel];
            GenerateLevel(level);
        } else {
            Debug.Log("The end!");
            return;
        }
    }

    private void GenerateLevel(LevelGeneratorLevel level) {
        List<Transform> randomedSpots = level.ShuffledSpawnSpots();
        List<GameObject> spawnPrefabs = level.Spawns;
        foreach (Transform spotPrefab in randomedSpots) {
            Transform spot = Instantiate(spotPrefab, worldContainer);
            GameObject spawnPrefab = spawnPrefabs[0];
            GameObject spawn = Instantiate(spawnPrefab, spot);
            spawn.transform.localPosition = Vector3.zero;
            ProcessSpawn(spawn);
            spawnPrefabs.RemoveAt(0);
        }
    }

    private void ProcessSpawn(GameObject spawn) {
        // process spawn
    }
}


[System.Serializable]
public class LevelGeneratorLevel {
    [SerializeField]
    private List<Transform> spawnSpots;

    [SerializeField]
    private List<GameObject> spawns;

    public List<GameObject> Spawns {get {return new List<GameObject>(spawns);}}

    public List<Transform> ShuffledSpawnSpots()
    {
        System.Random rnd = new();
        return spawnSpots.OrderBy((item) => rnd.Next()).ToList();
    }
}