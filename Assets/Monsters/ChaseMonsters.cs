using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ChaseMonsters : MonoBehaviour
{

    public Transform player;
    public Animator animator;
    public PlayerHealth playerHealth;
    public Transform playerCamera;

    public float attackDistance = 1.5f;

    [SerializeField] private GameObject chaseLights;

    private bool chasing = false;
    [SerializeField] private GameObject skin;
    [SerializeField] private GameObject chasecollider;



    [SerializeField] private Camera jumpscareCam;
    [SerializeField] private Camera chaseCam;


    private NavMeshAgent agent;

    [SerializeField] private bool doesAppear;
    private bool canAttack;

    [SerializeField] private AudioSource chaseTheme;

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

        if (doesAppear)
        {
            chaseLights.SetActive(false);
            skin.SetActive(false);
            chaseCam.enabled = false;
            chasecollider.SetActive(true);
            canAttack = false;
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
        
        if (distance <= attackDistance && canAttack)
        {
            agent.isStopped = true;
            animator.SetBool("isIdle", false);

            animator.SetBool("Attack", true);

            Attack();

        }


        
         if (chasing)
        {
            animator.SetBool("isIdle", true);
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

        playerCamera.gameObject.SetActive(false);
        jumpscareCam.GetComponent<Camera>().enabled = true;

        if (!jumpscare.isPlaying) jumpscare.Play();

        await Awaitable.WaitForSecondsAsync(jumpscareDuration, destroyCancellationToken);

        jumpscareCam.enabled = false;

        await Awaitable.WaitForSecondsAsync(1.0f, destroyCancellationToken);

        int getCurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(getCurrentSceneIndex);


    }

    public async void StartChase()
    {

        if (doesAppear)
        {
            chaseTheme.Play();
            chaseLights.SetActive(true);
            skin.SetActive(true);
            playerCamera.gameObject.SetActive(false);
            chaseCam.enabled = true;
            chasecollider.SetActive(false);
        }
        await Awaitable.WaitForSecondsAsync(1.0f, destroyCancellationToken);

        chaseCam.enabled = false;
        playerCamera.gameObject.SetActive(true);
        canAttack = true;
        agent.isStopped = false;


        chasing = true;
    }

}
