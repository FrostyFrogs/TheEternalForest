using UnityEngine;
using System.Collections;

public class BearTrapController : MonoBehaviour
{
    [Header("Trap")]
    public float trapTime = 2f;
    public float insanityDamage = 15f;


    [Header("Models")]
    public GameObject openModel;
    public GameObject closedModel;


    [Header("Audio")]
    public AudioSource trapSound;


    private bool triggered = false;



    void Start()
    {
        // Start open
        if(openModel != null)
            openModel.SetActive(true);

        if(closedModel != null)
            closedModel.SetActive(false);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;


        if (!other.CompareTag("Player"))
            return;


        triggered = true;


        // Swap model to closed
        if(openModel != null)
            openModel.SetActive(false);


        if(closedModel != null)
            closedModel.SetActive(true);



        // Play trap sound
        if(trapSound != null)
        {
            trapSound.Play();
        }



        // Find Rigidbody on player or parent
        Rigidbody rb =
            other.GetComponentInParent<Rigidbody>();


        if(rb != null)
        {
            StartCoroutine(
                TrapPlayer(rb)
            );
        }



        // Find insanity manager
        InsanityManager insanity =
            other.GetComponentInParent<InsanityManager>();


        if(insanity != null)
        {
            insanity.IncreaseInsanity(
                insanityDamage
            );
        }
    }



    IEnumerator TrapPlayer(Rigidbody rb)
    {
        bool oldKinematic =
            rb.isKinematic;


        rb.linearVelocity =
            Vector3.zero;

        rb.angularVelocity =
            Vector3.zero;


        rb.isKinematic = true;



        yield return new WaitForSeconds(
            trapTime
        );



        rb.isKinematic =
            oldKinematic;


        Destroy(gameObject);
    }
}