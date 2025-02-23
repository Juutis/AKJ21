using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator main;
    void Awake()
    {
        if (main == null)
        {
            main = this;
        }
    }

    private int currentLevel = -1;

    public int Level = 0;

    [SerializeField]
    private Transform worldContainer;
    [SerializeField]
    private List<LevelGeneratorLevel> levels = new();
    public void NextLevel() {
        int nextLevel = Level;
        Debug.Log($"Next level: {nextLevel}");
        if (levels.Count > nextLevel) {
            currentLevel  = nextLevel;
            var level = levels[currentLevel];
            GenerateLevel(level);
        } else {
            Debug.Log("The end!");
            return;
        }
    }

    private void GenerateLevel(LevelGeneratorLevel level) {
        List<Vector3> positions = new List<Vector3>();
        foreach(var layer in level.Layers) {

            foreach (var spawn in layer.Spawns) {
                for(var i = 0; i < spawn.Count; i++) {
                    
                    var attempts = 0;
                    Vector3 position = Vector3.zero;
                    while (true) {
                        var distance = Random.Range(layer.MinRadius, layer.MaxRadius);
                        var direction = Random.onUnitSphere;
                        var levelOrigin = WorldOrigin.OfActiveWorld;
                        var positionCandidate = levelOrigin.transform.position + direction * distance;
                        if (isOccupied(positionCandidate, positions)) {
                            attempts++;
                            if (attempts > 30) {
                                Debug.LogError("FAILED TO SPAWN OBJECT");
                                break;
                            }
                        } else {
                            position = positionCandidate;
                            break;
                        }
                    }
                    positions.Add(position);

                    GameObject spawnedObject = Instantiate(spawn.Prefab);
                    spawnedObject.transform.position = position;
                    ProcessSpawn(spawnedObject);
                }
            }
        }
    }

    private bool isOccupied(Vector3 position, List<Vector3> positions) {
        var margin = 30.0f;
        foreach (var oldPos in positions) {
            if (Vector3.Distance(oldPos, position) < margin) {
                return true;
            }
        }
        return false;
    }

    private void ProcessSpawn(GameObject spawn) {
        // process spawn
    }
}


[System.Serializable]
public class LevelGeneratorLevel {
    public List<LevelLayer> Layers;
}

[System.Serializable]
public class LevelLayer {
    public float MinRadius = 10.0f;
    public float MaxRadius = 10.0f;
    public List<LevelSpawn> Spawns;
}

[System.Serializable]
public class LevelSpawn {
    public int Count;
    public GameObject Prefab;
}