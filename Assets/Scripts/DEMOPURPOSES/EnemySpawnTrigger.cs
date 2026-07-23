using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour, Interactable
{
    // Drag your Enemy object here
    public EnemyController enemy;

    // Prevents activating it multiple times
    private bool activated = false;


    public void Interact()
    {
        // Stop multiple activations
        if (activated)
            return;


        activated = true;


        // Spawn the enemy
        enemy.SpawnEnemy();


        // Optional: remove the object after use
        Destroy(gameObject);
    }
}