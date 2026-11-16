using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{

    private HubScript hubScript;

    private List<GameObject> rooms;
    [SerializeField] private GameObject player; // default
    [SerializeField] private GameObject redDoor;
    [SerializeField] private GameObject redButton;

    [SerializeField] private GameObject blueSpawn;
    [SerializeField] private GameObject redSpawn;

    [SerializeField] private GameObject blueDoor;
    [SerializeField] private GameObject blueButton;
    public GameObject openedDoor;
    public GameObject pressedButton;

    [SerializeField] private Quaternion doorOpenPos;
    [SerializeField] private Quaternion doorClosedPos;

    [SerializeField] private GameObject proximityUi;

    [SerializeField] private TMP_Text counterText;



    public bool doorOpenable = true;
    public bool buttonPressed = false;
    public bool doorAlreadyOpened = false;

    public bool buttonMovementCompleted;

    public string hubGameObjectName = "Hub";

    [SerializeField] private GameObject finalRoom;

    [SerializeField] private GameObject roomParent;



    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private GameObject audioHub;
    private Transform openDoorTransform;
    private Transform openButtonTransform;
    private AudioSource openDoor;
    private AudioSource openButton;

    void Start()
    {
        audioHub = GameObject.FindWithTag("AudioHub");
        if (openDoor == null)
        {
            openDoorTransform = audioHub.transform.Find("Door");
            openDoor = openDoorTransform.GetComponent<AudioSource>();
            
        }
        if (openButton == null)
        {
            openButtonTransform = audioHub.transform.Find("ButtonPress");
            openButton = openButtonTransform.GetComponent<AudioSource>();   
        }

        hubScript = GameObject.Find(hubGameObjectName).GetComponent<HubScript>();

        if (player == null)
        {
            player = GameObject.FindAnyObjectByType<PlayerController>().gameObject;
        }

        if (counterText != null) counterText.text = (hubScript.rooms.Count).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (roomParent == null)
        {
            roomParent = GameObject.FindWithTag("RoomsParent");
        }

        rooms = hubScript.rooms;

        if (doorAlreadyOpened && openedDoor != null)
        {
            DoorMovement();
            
        }

        if (buttonPressed && !buttonMovementCompleted && pressedButton != null)
        {
            ButtonMovement();
        }
    }

    private float doorProximity = 1.5f;
    private float buttonProximity = 1.5f;
    private float openSpeed = 100f;

    private Vector3 originalPos; // button original pos
    private Vector3 buttonPos; // new button pos

    public void Open()
    {
            if (!doorAlreadyOpened)
            {
                if (!buttonPressed)
                
                {
                Debug.Log("button pressed");
                if (Vector3.Distance(player.transform.position, redButton.transform.position) < buttonProximity || Vector3.Distance(player.transform.position, blueButton.transform.position) < buttonProximity)
                {
                    Debug.Log("button pressed. you can open door now");
                    openButton.Play();
                    buttonPressed = true;

                    buttonMovementCompleted = false;

                    if (Vector3.Distance(player.transform.position, redButton.transform.position) < doorProximity)
                    pressedButton = redButton;
                    
                    else if (Vector3.Distance(player.transform.position, blueButton.transform.position) < doorProximity)
                    pressedButton = blueButton;


                    originalPos = pressedButton.transform.localPosition;
                    buttonPos = pressedButton.transform.localPosition + new Vector3(0, -0.5f, 0);


}
            }
                else if (buttonPressed)
                {
                if (Vector3.Distance(player.transform.position, redDoor.transform.position) < doorProximity || Vector3.Distance(player.transform.position, blueDoor.transform.position) < doorProximity)
                {
                    Debug.Log("door has been opened");
                    openDoor.Play();
                    doorAlreadyOpened = true;

                    if (Vector3.Distance(player.transform.position, redDoor.transform.position) < doorProximity)
                    {
                        openedDoor = redDoor;
                        SpawnRoom(redSpawn);
                        


                    }
                    else if (Vector3.Distance(player.transform.position, blueDoor.transform.position) < doorProximity)
                    {
                        openedDoor = blueDoor;
                        SpawnRoom(blueSpawn);

                    }

                }
            }
            }
        
    }

    private void DoorMovement()
    {
        openedDoor.transform.rotation = Quaternion.RotateTowards(openedDoor.transform.rotation, doorOpenPos, openSpeed * Time.deltaTime);
    }


    private void ButtonMovement()
    {

        if (pressedButton.transform.localPosition != buttonPos)
        {
            pressedButton.transform.localPosition = Vector3.MoveTowards(pressedButton.transform.localPosition, buttonPos, openSpeed * Time.deltaTime);
        }
        else
        {
            pressedButton.transform.localPosition = Vector3.MoveTowards(pressedButton.transform.localPosition, originalPos, openSpeed * Time.deltaTime);
        }



        if (pressedButton.transform.localPosition == originalPos)
        {
            buttonMovementCompleted = true;
        }
    }

    private void SpawnRoom(GameObject spawnArea)
    {
        
        int randomIndex = Random.Range(0, rooms.Count);

        if (rooms.Count != 0)
        {
            foreach (Transform child in roomParent.transform)
            {
                Destroy(child.gameObject);
            }

            GameObject spawnedRoom = Instantiate(rooms[randomIndex], spawnArea.transform.position, spawnArea.transform.rotation, roomParent.transform);
            hubScript.removeRoom(randomIndex);

        }
        else
        {
            GameObject spawnedRoom = Instantiate(finalRoom, spawnArea.transform.position, spawnArea.transform.rotation, roomParent.transform);
        }

    }
}
