using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TargetCursors : MonoBehaviour
{
    [SerializeField]
    private List<TargetCursor> targetCursors = new();
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private float borderPad;

    private float pollTime = 1f;
    private float lastPoll = 0f;

    private Dictionary<string, GameObject> cursorCache = new();


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastPoll > pollTime)
        {
            lastPoll = Time.time;
            foreach (TargetCursor targetCursor in targetCursors)
            {
                List<GameObject> objs = GameObject.FindGameObjectsWithTag(targetCursor.tag).ToList();
                GameObject nearest = objs.OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).FirstOrDefault();
                AddOrUpdate(cursorCache, targetCursor.tag, nearest);
            }
        }

        foreach (KeyValuePair<string, GameObject> entry in cursorCache)
        {
            RectTransform rectTransform = canvas.GetComponent<RectTransform>();
            Vector2 center = rectTransform.rect.center;
            Vector2 size = rectTransform.rect.size;
            Vector3 vpPos = cam.ViewportToScreenPoint(cam.WorldToViewportPoint(entry.Value.transform.position));
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
                Debug.Log($"LOL");
            }
            else if (entry.Key == "Respawn")
            {
                Debug.Log("NOT LOL");

            }

            TargetCursor cursor = targetCursors.FirstOrDefault(x => x.tag == entry.Key);
            cursor.img.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(vpPos.y, vpPos.x) - 90);

            cursor.img.GetComponent<RectTransform>().position = clamped;
        }
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
public class TargetCursor
{
    public string tag;
    public Image img;
}