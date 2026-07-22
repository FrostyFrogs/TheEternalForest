using UnityEngine;

public class MatchPickup : MonoBehaviour, Interactable
{
    // Number of matches this pickup gives
    public int matchesToGive = 5;

    public void Interact()
    {
        // Find the player's MatchManager
        MatchManager manager = FindObjectOfType<MatchManager>();

        // Give the player matches
        manager.AddMatches(matchesToGive);

        // Remove this pickup
        Destroy(gameObject);
    }
}