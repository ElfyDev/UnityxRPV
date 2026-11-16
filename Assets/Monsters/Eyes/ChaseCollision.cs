using UnityEngine;

public class ChaseCollision : MonoBehaviour
{
    public ChaseMonsters monster;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            monster.StartChase();
            this.gameObject.SetActive(false);
        }
    }
}
