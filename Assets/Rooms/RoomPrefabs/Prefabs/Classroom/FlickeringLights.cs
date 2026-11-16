using System.Collections;
using UnityEngine;

public class FlickeringLights : MonoBehaviour
{
    [SerializeField] private GameObject lights;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(FlickerRoutine());
    }

    
    private IEnumerator FlickerRoutine()
    {
        while (true){
            lights.SetActive(!lights.activeSelf);
            yield return new WaitForSeconds(1.5f);
        }
    }
}
