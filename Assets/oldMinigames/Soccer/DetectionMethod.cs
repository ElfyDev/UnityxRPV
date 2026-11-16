using UnityEngine;

public class DetectionMethod : MonoBehaviour
{
    [SerializeField] private GameObject scoreUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            scoreUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            scoreUI.SetActive(false);
        }
    }
}
