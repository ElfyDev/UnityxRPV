using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GoalGateUnlocked : MonoBehaviour
{
    [SerializeField] private float openSpeed;
    private bool opened;

    [SerializeField] private float moveDistance;


    [SerializeField] private GameObject door;


    private Vector3 targetPosition;
    private void Start()
    {
        targetPosition = new Vector3(door.transform.position.x, door.transform.position.y + 3f, door.transform.position.z);
        opened = false;
    }
    private void Update()
    {
        Vector3 currentPosition = this.transform.position;

        if (opened)
        {
            door.transform.position = Vector3.MoveTowards(currentPosition, targetPosition, openSpeed * Time.deltaTime);

        }

        if (door.transform.position == targetPosition)
        {
            opened = false;
        }

    }

    public void OpenGate()
    {
        opened = true;
    }
}
