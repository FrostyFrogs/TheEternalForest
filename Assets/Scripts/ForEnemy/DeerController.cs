using UnityEngine;

public class DeerController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Camera playerCamera;

    public MatchManager matchManager;
    public InsanityManager insanityManager;

    public GameObject deerPrefab;


    [Header("Spawn")]
    public float sideDistanceMin = 6f;
    public float sideDistanceMax = 12f;

    public float forwardDistanceMin = 10f;
    public float forwardDistanceMax = 25f;

    public float spawnHeightOffset = 0.5f;


    [Header("Behind Chance")]
    [Range(0f, 1f)]
    public float behindSpawnChance = 0.75f;


    private GameObject currentDeer;



    public void SpawnEnemy()
    {
        if(currentDeer != null)
            return;



        // Left or right
        float side =
            Random.value < 0.5f ? -1f : 1f;


        float sideDistance =
            Random.Range(
                sideDistanceMin,
                sideDistanceMax
            );


        float forwardDistance =
            Random.Range(
                forwardDistanceMin,
                forwardDistanceMax
            );



        // Front or behind
        float forwardDirection =
            Random.value < behindSpawnChance
            ? -1f
            : 1f;



        Vector3 spawnPosition =
            player.position +
            player.right * side * sideDistance +
            player.forward * forwardDistance * forwardDirection;


        spawnPosition.y += spawnHeightOffset;



        currentDeer =
            Instantiate(
                deerPrefab,
                spawnPosition,
                Quaternion.identity
            );



        DeerChase deer =
            currentDeer.GetComponent<DeerChase>();


        if(deer != null)
        {
            deer.Setup(
                player,
                playerCamera,
                matchManager,
                insanityManager,
                this
            );
        }
    }



    public void DeerDestroyed()
    {
        currentDeer = null;
    }
}