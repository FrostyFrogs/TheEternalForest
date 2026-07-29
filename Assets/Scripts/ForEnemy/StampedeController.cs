using UnityEngine;
using System.Collections;

public class StampedeController : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement playerMovement;
    public Transform player;
    public InsanityManager insanityManager;

    public GameObject stampedeEnemyPrefab;

    public AudioSource warningSound;
    public AudioSource stampedeSound;


    [Header("Timing")]
    public float warningDelay = 3f;


    [Header("Spawn")]
    public float forwardSpawnDistance = 10f;
    public float sideSpawnDistance = 15f;
    private Vector3 warningStartPosition;


    [Header("Enemy")]
    public float enemySpeed = 20f;


    private bool active;


    [Header("Insanity")]
    public float walkFailureInsanity = 20f;
    public float runFailureInsanity = 35f;
    public float successInsanity = 10f;

    public void StartStampede()
    {
        if(active)
            return;


        StartCoroutine(StampedeEvent());
    }



    IEnumerator StampedeEvent()
    {
        active = true;


        if(warningSound != null)
            warningSound.Play();


        // Save where player was when warning started
        warningStartPosition = player.position;


        // Give player time to react
        yield return new WaitForSeconds(warningDelay);



        switch(playerMovement.state)
        {
            case PlayerMovement.MovementState.crouching:

                insanityManager.DecreaseInsanity(
                    successInsanity
                );

                break;



            case PlayerMovement.MovementState.walking:

                SpawnWalkingStampede();

                break;



            case PlayerMovement.MovementState.sprinting:

                SpawnSprintStampede();

                break;
        }



        active = false;
    }



    void SpawnWalkingStampede()
    {
        float side =
            Random.value > 0.5f ? 1 : -1;



        Vector3 spawnPosition =
            warningStartPosition +
            player.forward * forwardSpawnDistance +
            player.right * side * sideSpawnDistance;



        GameObject enemy =
            Instantiate(
                stampedeEnemyPrefab,
                spawnPosition,
                Quaternion.identity
            );



        StampedeChase movement =
            enemy.GetComponent<StampedeChase>();


        movement.insanityManager = insanityManager;
        movement.insanityDamage = walkFailureInsanity;



        movement.StartCrossing(
            player.right,
            side,
            enemySpeed
        );
    }



    void SpawnSprintStampede()
    {
        float side =
            Random.value > 0.5f ? 1 : -1;



        Vector3 spawnPosition =
            player.position +
            player.right * side * sideSpawnDistance;



        GameObject enemy =
            Instantiate(
                stampedeEnemyPrefab,
                spawnPosition,
                Quaternion.identity
            );



        StampedeChase movement =
            enemy.GetComponent<StampedeChase>();


        movement.insanityManager = insanityManager;
        movement.insanityDamage = runFailureInsanity;



        movement.StartChasing(
            player,
            enemySpeed
        );
    }
}