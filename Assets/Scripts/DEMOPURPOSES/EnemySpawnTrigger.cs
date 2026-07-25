using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour, Interactable
{
    public EnemyController enemy;
    
    public void Interact()
    {
        enemy.SpawnEnemy();
    }
}