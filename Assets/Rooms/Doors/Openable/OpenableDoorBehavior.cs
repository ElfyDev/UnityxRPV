using UnityEngine;
using UnityEngine.InputSystem;

public class OpenableDoorBehavior : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private float openSpeed = 100f;

    private PlayerLocomotionInput input;

    bool OpeningDoor = false;
    bool hasOpened = false;

    public AudioSource openDoor;
    private GameObject audioHub;
    private Transform openDoorTransform;

    void Start()
    {
        door.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (openDoor == null)
        {
            audioHub = GameObject.FindWithTag("AudioHub");
            openDoorTransform = audioHub.transform.Find("Door");
            openDoor = openDoorTransform.GetComponent<AudioSource>();
;        }
    }

    // Update is called once per frame
    void Update()
    {
        if (OpeningDoor)
        {
            door.transform.rotation = Quaternion.RotateTowards(door.transform.rotation, Quaternion.Euler(-180, 100, 0), openSpeed * Time.deltaTime);
        }

        if (Quaternion.Angle(door.transform.rotation, Quaternion.Euler(-180, 100, 0)) < 0.1f)
        {
            OpeningDoor = false;

        }
    }

    void OpenDoor()
    {
        if (!hasOpened)
        {
            OpeningDoor = true;
            hasOpened = true;
            openDoor.Play();
        }
        
        
    }

    public void ResetDoor()
    {
        door.transform.rotation = Quaternion.Euler(-180, 0, 0);
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenDoor();
        }
    }
}
