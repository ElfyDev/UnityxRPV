using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class OpenGateStart : MonoBehaviour
{

    [SerializeField] private float openSpeed;
    private bool opened;

    [SerializeField] private float moveDistance;


    [SerializeField] private AudioSource openGate;

    [SerializeField] private GameObject door;


    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = new Vector3(door.transform.position.x, door.transform.position.y + moveDistance, door.transform.position.z);
        opened = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition = this.transform.position;

        if (opened)
        {
            door.transform.position = Vector3.MoveTowards(currentPosition, targetPosition, openSpeed * Time.deltaTime);
            
            if (!openGate.isPlaying)
            {
                openGate.Play();
            }
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenGate();
            Debug.Log("yes");
        }
    }
}
