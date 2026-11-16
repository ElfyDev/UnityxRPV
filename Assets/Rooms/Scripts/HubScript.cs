using System.Collections.Generic;
using UnityEngine;

public class HubScript : MonoBehaviour
{
    [SerializeField] public List<GameObject> rooms;

    public void removeRoom(int order)
    {
        rooms.Remove(rooms[order]);
    }
}
