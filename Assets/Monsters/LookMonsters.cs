using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class LookMonsters : MonoBehaviour
{

    private Transform player;
    private Animator animator;
    private PlayerHealth playerHealth;
    private Transform playerCamera;

    public float moveDistance = 50f;
    public float idleDistance = 75f;
    public float attackDistance = 1.5f;

    [SerializeField] private Camera jumpscareCam;

    private NavMeshAgent agent;


    public GameObject audioHub;
    public Transform jumpscareTransform;
    public AudioSource jumpscare;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioHub = GameObject.FindWithTag("AudioHub");
        if (jumpscare == null)
        {
            jumpscareTransform = audioHub.transform.Find("Jumpscare");
            jumpscare = jumpscareTransform.GetComponent<AudioSource>();
        }

        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            player = GameObject.FindAnyObjectByType<PlayerController>().transform;
        }

        if (playerCamera == null)
        {
            Transform child = player.Find("Main Camera");
            playerCamera = child.transform;
        }

        if (animator == null) animator = GetComponent<Animator>();
        if (playerHealth == null && player != null) player.gameObject.GetComponent<PlayerHealth>();

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) player = GameObject.FindAnyObjectByType<PlayerController>().transform;
        
        if (playerCamera == null)
        {
            Transform child = player.Find("Camera");
            playerCamera = child.transform;
        }
        if (playerHealth == null && player != null) player.GetComponent<PlayerHealth>();


        float distance = Vector3.Distance(this.transform.position, player.position);
        Vector3 direction = (transform.position - playerCamera.position).normalized;
        bool isLooking = Vector3.Dot(playerCamera.forward, direction) > 0.7f;


        // attack
        
        if (!isLooking && distance <= attackDistance)
        {
            agent.isStopped = true;
            animator.SetBool("isIdle", false);
            animator.SetBool("Attack", true);

            Attack();

        }


        // idle
        if (isLooking)
        {
            agent.isStopped = true;


            animator.SetBool("isIdle", true);
        }

        else if (!isLooking && distance <= moveDistance)
        {
            animator.SetBool("isIdle", false);
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        // run
    }

    private float jumpscareDuration = 2f;


    private async void Attack()
    {
        if (jumpscareCam == null)
        {
            return;
        }

        player.gameObject.SetActive(false);

        if (!jumpscare.isPlaying) jumpscare.Play();

        playerCamera.gameObject.SetActive(false);
        jumpscareCam.GetComponent<Camera>().enabled = true;

        await Awaitable.WaitForSecondsAsync(jumpscareDuration, destroyCancellationToken);

        jumpscareCam.enabled = false;

        await Awaitable.WaitForSecondsAsync(1.0f, destroyCancellationToken);

        int getCurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(getCurrentSceneIndex);


    }


}
