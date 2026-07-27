using UnityEngine;
using System.Collections;

public class BearTrap : MonoBehaviour
{
    [Header("Trap")]
    public float trapTime = 2f;
    public float insanityDamage = 15f;

    [Header("Audio")]
    public AudioSource trapSound;

    private bool triggered = false;


    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;

        if (!other.CompareTag("Player"))
            return;


        triggered = true;


        // Play trap sound
        if (trapSound != null)
        {
            trapSound.Play();
        }


        // Find Rigidbody on player or parent
        Rigidbody rb = other.GetComponentInParent<Rigidbody>();

        if (rb != null)
        {
            StartCoroutine(TrapPlayer(rb));
        }


        // Find insanity manager
        InsanityManager insanity =
            other.GetComponentInParent<InsanityManager>();

        if (insanity != null)
        {
            insanity.IncreaseInsanity(insanityDamage);
        }
    }



    IEnumerator TrapPlayer(Rigidbody rb)
    {
        // Save original Rigidbody settings
        bool oldKinematic = rb.isKinematic;


        // Stop current movement
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;


        // Temporarily disable physics movement
        rb.isKinematic = true;


        // Wait while trapped
        yield return new WaitForSeconds(trapTime);


        // Restore physics
        rb.isKinematic = oldKinematic;


        Destroy(gameObject);
    }
}