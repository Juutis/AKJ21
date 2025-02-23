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
    private Canvas canvas;
    [SerializeField]
    private float borderPad;
    [SerializeField]
    private ShipControls player;

    private float pollTime = 1f;
    private float lastPoll = 0f;
    private Camera cam;
    private RectTransform parentRect;

    private Dictionary<string, List<CursorInstance>> cursorInstances = new();


    void Start()
    {
        cam = Camera.main;
        parentRect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastPoll > pollTime)
        {
            ProbeTargets();
        }

        UpdateCursors();
    }

    private void UpdateCursors()
    {
        foreach (KeyValuePair<string, List<CursorInstance>> entry in cursorInstances)
        {
            List<CursorInstance> deleted = new();

            foreach (CursorInstance instance in entry.Value)
            {
                if (instance.target == null )
                {
                    deleted.Add(instance);
                    Destroy(instance.cursor);
                    continue;
                }

                RectTransform rectTransform = canvas.GetComponent<RectTransform>();
                Vector3 targetPos = instance.target.transform.position;
                Vector3 dir = cam.transform.position - targetPos;
                Vector3 proj = Vector3.Dot(dir, cam.transform.forward) * cam.transform.forward;
                Vector3 oproj = dir - proj;
                float projDot = Vector3.Dot(proj, cam.transform.forward);

                // Object behind camera
                if (projDot > 0)
                {
                    targetPos = targetPos + (1 / (oproj.magnitude + 0.001f)) * 1000f * dir.magnitude * cam.transform.up;
                    instance.cursor.SetActive(true);
                }
                else
                {

                    if (dir.magnitude < instance.config.trackMinRange)
                    {
                        instance.cursor.SetActive(false);
                    }
                    else
                    {
                        instance.cursor.SetActive(true);
                    }
                }


                Vector2 vpPos = WorldSpaceToCanvas(rectTransform, cam, targetPos);

                RectTransform rect = instance.cursor.GetComponent<RectTransform>();
                rect.anchoredPosition = vpPos;
                ClampToWindow(rect, parentRect);

                rect.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(rect.anchoredPosition.x - vpPos.x, vpPos.y - rect.anchoredPosition.y) * Mathf.Rad2Deg);
            }

            entry.Value.RemoveAll(x => deleted.Contains(x));
        }
    }

    private void ProbeTargets()
    {
        lastPoll = Time.time;

        foreach (TargetCursorConfig targetCursor in targetCursorConfig)
        {
            if (targetCursor.disabled) { continue; }

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

                    cursorInstances[curTag].ForEach(x => Destroy(x.cursor));
                    CursorInstance instance = InstantiateCursor(targetCursor, targets.FirstOrDefault());
                    cursorInstances[curTag].Add(instance);
                }
                else
                {
                    CursorInstance instance = InstantiateCursor(targetCursor, targets.FirstOrDefault());

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

                    CursorInstance instance = InstantiateCursor(targetCursor, target);
                    cursorInstances[curTag].Add(instance);
                }
            }
        }
    }

    private CursorInstance InstantiateCursor(TargetCursorConfig cursorConfig, GameObject target)
    {
        CursorInstance instance = new();
        instance.cursor = Instantiate(cursorConfig.cursorPrefab);
        instance.target = target;
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

    private Vector2 WorldSpaceToCanvas(RectTransform canvasRect, Camera camera, Vector3 worldPos)
    {
        Vector2 viewportPosition = camera.WorldToViewportPoint(worldPos);

        float x = viewportPosition.x * canvasRect.sizeDelta.x - canvasRect.sizeDelta.x * 0.5f;
        float y = viewportPosition.y * canvasRect.sizeDelta.y - canvasRect.sizeDelta.y * 0.5f;

        Vector2 canvasPos = new Vector2(x, y);

        return canvasPos;
    }

    private void ClampToWindow(RectTransform element, RectTransform parent)
    {
        Vector3 pos = element.localPosition;

        Vector3 minPosition = parent.rect.min - element.rect.min;
        Vector3 maxPosition = parent.rect.max - element.rect.max;

        pos.x = Mathf.Clamp(element.localPosition.x, minPosition.x, maxPosition.x);
        pos.y = Mathf.Clamp(element.localPosition.y, minPosition.y, maxPosition.y);

        element.localPosition = pos;
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
    public float trackMinRange;
    public bool disabled;
}

public class CursorInstance
{
    public GameObject cursor;
    public GameObject target;
    public TargetCursorConfig config;
}