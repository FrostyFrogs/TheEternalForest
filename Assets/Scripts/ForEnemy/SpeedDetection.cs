using UnityEngine;

public class StampedeDetection : MonoBehaviour
{
    public StampedeChase stampede;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            stampede.PlayerDetected(
                other.transform
            );
        }
    }
}