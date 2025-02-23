using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TargetCursors : MonoBehaviour
{
    [SerializeField]
    private List<TargetCursorConfig> targetCursorConfig = new();
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private float borderPad;
    [SerializeField]
    private ShipControls player;

    private float pollTime = 1f;
    private float lastPoll = 0f;

    private Dictionary<string, GameObject> cursorCache = new();
    private Dictionary<string, List<CursorInstance>> cursorInstances = new();


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastPoll > pollTime)
        {
            Debug.Log($"PROBING: {Time.time}");
            ProbeTargets();
        }

        UpdateCursors();
    }

    private void UpdateCursors()
    {
        foreach (KeyValuePair<string, List<CursorInstance>> entry in cursorInstances)
        {
            foreach (CursorInstance instance in entry.Value)
            {
                RectTransform rectTransform = canvas.GetComponent<RectTransform>();
                Vector2 center = rectTransform.rect.center;
                Vector2 size = rectTransform.rect.size;
                Vector3 vpPos = cam.ViewportToScreenPoint(cam.WorldToViewportPoint(instance.cursor.transform.position));
                float yMin = center.y + borderPad;
                float yMax = center.y + size.y - borderPad;
                float xMin = center.x + borderPad;
                float xMax = center.x + size.x - borderPad;

                float k1 = size.y / size.x;
                float k2 = -size.y / size.x;

                // if (vpPos.z < 0)
                // {
                //     if (vpPos.y > k1 * vpPos.x && vpPos.y > k2 * vpPos.x)
                //     {
                //         vpPos.y = yMax;
                //     }
                //     else if (vpPos.y < k1 * vpPos.x && vpPos.y < k2 * vpPos.x)
                //     {
                //         vpPos.y = yMin;
                //     }
                //     else if (vpPos.y < k1 * vpPos.x && vpPos.y > k2 * vpPos.x)
                //     {
                //         vpPos.x = xMax;
                //     }
                //     else if (vpPos.y > k1 * vpPos.x && vpPos.y < k2 * vpPos.x)
                //     {
                //         vpPos.x = xMin;
                //     }
                // }

                Vector3 clamped = new Vector3(Mathf.Clamp(vpPos.x, xMin, xMax), Mathf.Clamp(vpPos.y, yMin, yMax), vpPos.z);

                if (vpPos.z < 0 && entry.Key == "Respawn")
                {
                }
                else if (entry.Key == "Respawn")
                {

                }

                TargetCursorConfig cursor = targetCursorConfig.FirstOrDefault(x => x.tag == entry.Key);
                instance.cursor.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(vpPos.y, vpPos.x) - 90);

                instance.cursor.GetComponent<RectTransform>().position = clamped;
            }
        }
    }

    private void ProbeTargets()
    {
        lastPoll = Time.time;
        // foreach (TargetCursorConfig targetCursor in targetCursorConfig)
        // {
        //     List<GameObject> objs = GameObject.FindGameObjectsWithTag(targetCursor.tag).ToList();
        //     GameObject nearest = objs.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).FirstOrDefault();
        //     AddOrUpdate(cursorCache, targetCursor.tag, nearest);
        // }

        foreach (TargetCursorConfig targetCursor in targetCursorConfig)
        {
            string curTag = targetCursor.tag;

            IEnumerable<GameObject> targets = GameObject.FindGameObjectsWithTag(curTag);

            if (!targets.Any(x => x != null))
            {
                continue;
            }

            if (targetCursor.trackInRange)
            {
                targets = targets.Where(t => Vector3.Distance(player.transform.position, t.transform.position) < targetCursor.trackRange);

                if (!targets.Any(x => x != null))
                {
                    continue;
                }
            }


            if (!targetCursor.trackAll)
            {
                if (cursorInstances.ContainsKey(curTag))
                {
                    GameObject target = targets.FirstOrDefault();

                    if (cursorInstances[curTag].Any(x => x.target == target))
                    {
                        continue;
                    }

                    CursorInstance instance = InstantiateCursor(targetCursor, targets);
                    cursorInstances[curTag].Add(instance);
                }
                else
                {
                    CursorInstance instance = InstantiateCursor(targetCursor, targets);

                    cursorInstances.Add(curTag, new() { instance });
                }
            }
            else
            {
                if (!cursorInstances.ContainsKey(curTag))
                {
                    cursorInstances.Add(curTag, new() { });
                }

                foreach (var target in targets)
                {
                    if (cursorInstances[curTag].Any(x => x.target == target))
                    {
                        continue;
                    }

                    CursorInstance instance = InstantiateCursor(targetCursor, targets);
                    cursorInstances[curTag].Add(instance);
                }
            }
        }
    }

    private CursorInstance InstantiateCursor(TargetCursorConfig cursorConfig, IEnumerable<GameObject> targets)
    {
        CursorInstance instance = new();
        instance.cursor = Instantiate(cursorConfig.cursorPrefab);
        instance.target = targets.FirstOrDefault();
        instance.config = cursorConfig;
        instance.cursor.transform.parent = transform;
        return instance;
    }

    private void AddOrUpdate(Dictionary<string, GameObject> dict, string key, GameObject value)
    {
        if (dict.ContainsKey(key))
        {
            dict[key] = value;
        }
        else
        {
            dict.Add(key, value);
        }
    }
}

[Serializable]
public class TargetCursorConfig
{
    public string tag;
    public GameObject cursorPrefab;
    public bool trackAll;
    public bool trackInRange;
    public float trackRange;
}

public class CursorInstance
{
    public GameObject cursor;
    public GameObject target;
    public TargetCursorConfig config;
}