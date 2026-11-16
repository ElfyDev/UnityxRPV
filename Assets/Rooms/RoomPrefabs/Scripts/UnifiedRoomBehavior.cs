using UnityEngine;

public class UnifiedRoomBehavior : MonoBehaviour
{
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject spawn;

    private void OnEnable()
    {
        GameObject spawnObject = Instantiate(roomPrefab, spawn.transform.position, spawn.transform.rotation);

    }
}
