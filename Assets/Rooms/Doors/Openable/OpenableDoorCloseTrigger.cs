using UnityEngine;

public class OpenableDoorCloseTrigger : MonoBehaviour
{
    private OpenableDoorBehavior openable;

    private void OnEnable()
    {
        openable = GetComponentInParent<OpenableDoorBehavior>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            openable.ResetDoor();
        }
    }
}
