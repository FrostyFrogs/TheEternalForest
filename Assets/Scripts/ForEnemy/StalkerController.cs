using UnityEngine;

public class StalkerController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Camera playerCamera;

    public MatchManager matchManager;
    public InsanityManager insanityManager;

    public GameObject enemyPrefab;


    [Header("Spawn")]
    public float spawnDistance = 25f;


    private GameObject currentEnemy;

    public void SpawnEnemy()
    {
        if(currentEnemy != null)
            return;


        Vector3 spawnPosition =
            player.position -
            player.forward * spawnDistance;


        currentEnemy =
            Instantiate(
                enemyPrefab,
                spawnPosition,
                Quaternion.identity
            );


        StalkerChase enemy =
            currentEnemy.GetComponent<StalkerChase>();


        if(enemy != null)
        {
            enemy.Setup(
                player,
                playerCamera,
                matchManager,
                insanityManager
            );
        }
    }



    public void EnemyDestroyed()
    {
        currentEnemy = null;
    }
}