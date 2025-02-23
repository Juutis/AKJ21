using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    private GobboShip shipAi;
    private float spawned;
    private float spawnDuration = 1.0f;
    private bool started = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shipAi = GetComponentInChildren<GobboShip>(true);
        shipAi.enabled = false;
        shipAi.gameObject.SetActive(false);
        Invoke("go", Random.Range(0.0f, 1.0f));
    }

    public void go() {
        started = true;
        spawned = Time.time;
        shipAi.gameObject.SetActive(true);
        Update();
    }

    // Update is called once per frame
    void Update()
    {
        if(!started) {
            return;
        }
        var t = (Time.time - spawned) / spawnDuration;
        Mathf.Clamp(t, 0.0f, 1.0f);
        var scaleX = Mathf.Lerp(0.0f, 1.0f, t);
        var scaleY = Mathf.Lerp(0.0f, 1.0f, t);
        var scaleZ = Mathf.Lerp(100.0f, 1.0f, t);
        transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        if (t >= 0.999) {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            unleashGobbo();
        }
    }

    void unleashGobbo() {
        Destroy(gameObject);
        shipAi.enabled = true;
        shipAi.transform.parent = null;
    }
}
