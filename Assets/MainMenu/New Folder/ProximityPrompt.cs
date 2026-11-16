using UnityEngine;
using System.Collections.Generic;

public class ProximityIconManager : MonoBehaviour
{
    public float activationDistance = 3f;
    public float iconScale = 0.5f;

    public GameObject eIconPrefab;
    public GameObject keyIconPrefab;

    public List<GameObject> eInteractObjects = new();
    public List<GameObject> keyInteractObjects = new();

    private readonly Dictionary<GameObject, GameObject> activeIcons = new Dictionary<GameObject, GameObject>();

    void Update()
    {
        CleanupIcons();
        HandleIcons(eInteractObjects, eIconPrefab);
        HandleIcons(keyInteractObjects, keyIconPrefab);
    }

    void HandleIcons(List<GameObject> objects, GameObject prefab)
    {
        if (prefab == null || objects == null) return;

        foreach (var obj in objects)
        {
            if (obj == null || !obj) continue;
            if (!obj.activeInHierarchy) continue;

            float dist = Vector3.Distance(transform.position, obj.transform.position);
            bool inRange = dist <= activationDistance;

            activeIcons.TryGetValue(obj, out GameObject icon);

            if (inRange && icon == null)
            {
                icon = Instantiate(prefab, obj.transform.position + Vector3.up * 1.5f, Quaternion.identity);
                icon.transform.localScale *= iconScale;
                activeIcons[obj] = icon;
            }

            if (icon != null)
            {
                if (inRange)
                {
                    icon.SetActive(true);
                    icon.transform.position = obj.transform.position + Vector3.up * 1.5f;
                    if (Camera.main != null)
                        icon.transform.LookAt(Camera.main.transform);
                }
                else
                {
                    icon.SetActive(false);
                }
            }
        }
    }

    void CleanupIcons()
    {
        if (activeIcons.Count == 0) return;

        List<GameObject> toRemove = new List<GameObject>();

        foreach (var kv in activeIcons)
        {
            var target = kv.Key;
            if (target == null || !target || !target.activeInHierarchy)
                toRemove.Add(target);
        }

        foreach (var target in toRemove)
        {
            if (activeIcons.TryGetValue(target, out GameObject icon) && icon != null)
                Destroy(icon);
            activeIcons.Remove(target);
        }
    }

    void OnDestroy()
    {
        foreach (var kv in activeIcons)
        {
            if (kv.Value != null)
                Destroy(kv.Value);
        }
        activeIcons.Clear();
    }
}
