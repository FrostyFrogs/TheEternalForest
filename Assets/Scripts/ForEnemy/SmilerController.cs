using UnityEngine;

public class SmilerController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public InsanityManager insanityManager;

    public GameObject entityPrefab;


    [Header("Spawn")]
    // How far ahead of the player it appears
    public float spawnDistance = 25f;
    public float offsetY = 0f;

    // Currently active entity
    private GameObject currentEntity;



    void Update()
    {
        // TEST KEY
        if(Input.GetKeyDown(KeyCode.Y))
        {
            SpawnEnemy();
        }

        // Check if the old entity was destroyed
        if(currentEntity == null)
        {
            return;
        }
    }



    public void SpawnEnemy()
    {
        if(player == null || entityPrefab == null)
            return;


        // Spawn in front of player
        Vector3 spawnPosition =
            player.position +
            player.forward * spawnDistance;

        spawnPosition.y += offsetY;


        currentEntity =
            Instantiate(
                entityPrefab,
                spawnPosition,
                Quaternion.identity
            );


        SmilerChase entity =
            currentEntity.GetComponent<SmilerChase>();


        if(entity != null)
        {
            entity.Setup(
                player,
                insanityManager
            );
        }
    }
}