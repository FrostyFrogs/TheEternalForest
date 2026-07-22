using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    // Assign your PlayerCam here in the Inspector
    public Camera playerCamera;

    // Maximum distance the player can interact from
    public float interactDistance = 3f;

    void Update()
    {
        // Press E to interact
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

            RaycastHit hit;

            // Check if the ray hit something
            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                // Try to get an interactable object
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                // If we found one, interact with it
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
    }
}