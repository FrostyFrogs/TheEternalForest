using UnityEngine;
using System.Collections;

public class StampedeController : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement playerMovement;
    public Transform player;
    public InsanityManager insanityManager;

    public GameObject stampedeEnemyPrefab;


    [Header("Audio")]
    public AudioSource warningSound;


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



    void Update()
    {
        // Temporary testing
        if(Input.GetKeyDown(KeyCode.U))
        {
            StartStampede();
        }
    }



    public void StartStampede()
    {
        if(active)
            return;


        StartCoroutine(
            StampedeEvent()
        );
    }



    IEnumerator StampedeEvent()
    {
        active = true;


        if(warningSound != null)
        {
            warningSound.Play();
        }


        warningStartPosition =
            player.position;



        yield return new WaitForSeconds(
            warningDelay
        );



        switch(playerMovement.state)
        {
            case PlayerMovement.MovementState.crouching:

                if(insanityManager != null)
                {
                    insanityManager.DecreaseInsanity(
                        successInsanity
                    );
                }

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



    void PlayStampedeSound(GameObject enemy)
    {
        AudioSource sound =
            enemy.GetComponent<AudioSource>();


        if(sound == null)
        {
            Debug.Log("No AudioSource on stampede prefab");
            return;
        }


        sound.Play();
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



        PlayStampedeSound(enemy);



        StampedeChase movement =
            enemy.GetComponent<StampedeChase>();


        if(movement != null)
        {
            movement.insanityManager =
                insanityManager;


            movement.insanityDamage =
                walkFailureInsanity;



            movement.StartCrossing(
                player.right,
                side,
                enemySpeed
            );
        }
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



        PlayStampedeSound(enemy);



        StampedeChase movement =
            enemy.GetComponent<StampedeChase>();


        if(movement != null)
        {
            movement.insanityManager =
                insanityManager;


            movement.insanityDamage =
                runFailureInsanity;



            movement.StartChasing(
                player,
                enemySpeed
            );
        }
    }
}