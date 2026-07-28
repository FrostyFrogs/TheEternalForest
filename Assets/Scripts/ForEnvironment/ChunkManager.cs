using UnityEngine;
using System.Collections.Generic;

public class ChunkManager : MonoBehaviour
{
    [Header("References")]
    public EncounterManager encounterManager;

    [Header("Chunks")]
    public GameObject[] chunkPrefabs;


    [Header("Generation")]
    public Transform player;
    public float chunkSize = 20f;


    [Header("Grid Size")]
    // 5 wide (-2 to +2)
    public int horizontalRange = 2;

    // 7 deep (-3 to +3)
    public int forwardRange = 3;
    public int backwardRange = 3;



    [Header("Wall")]
    public WallController wall;

    // How far player travels before wall starts
    public float wallStartDistance = 100f;


    private Dictionary<Vector2Int, GameObject> activeChunks =
        new Dictionary<Vector2Int, GameObject>();


    private Vector2Int lastPlayerChunk;

    private Vector3 startingPlayerPosition;

    private bool wallStarted;



    void Start()
    {
        startingPlayerPosition = player.position;

        lastPlayerChunk = GetPlayerChunk();

        UpdateChunks();
    }



    void Update()
    {
        CheckWall();


        Vector2Int currentChunk = GetPlayerChunk();


        if(currentChunk != lastPlayerChunk)
        {
            lastPlayerChunk = currentChunk;

            UpdateChunks();
            RemoveOldChunks();


            if(wall != null)
            {
                wall.ForceCatchUp(player.position);
            }
        }
    }



    void CheckWall()
    {
        if(wallStarted)
            return;


        float distance =
            Vector3.Distance(
                startingPlayerPosition,
                player.position
            );


        if(distance >= wallStartDistance)
        {
            if(wall != null)
            {
                wall.StartWall();

                wallStarted = true;
            }
        }
    }



    Vector2Int GetPlayerChunk()
    {
        int x =
            Mathf.FloorToInt(
                player.position.x / chunkSize
            );


        int z =
            Mathf.FloorToInt(
                player.position.z / chunkSize
            );


        return new Vector2Int(x, z);
    }



    void UpdateChunks()
    {
        Vector2Int playerChunk = GetPlayerChunk();


        for(int x = -horizontalRange;
            x <= horizontalRange;
            x++)
        {
            for(int z = -backwardRange;
                z <= forwardRange;
                z++)
            {
                Vector2Int chunkPosition =
                    new Vector2Int(
                        playerChunk.x + x,
                        playerChunk.y + z
                    );


                if(!activeChunks.ContainsKey(chunkPosition))
                {
                    SpawnChunk(chunkPosition);
                }
            }
        }
    }



    void SpawnChunk(Vector2Int coordinate)
    {
        
        GameObject prefab =
            chunkPrefabs[
                Random.Range(
                    0,
                    chunkPrefabs.Length
                )
            ];


        Vector3 position =
            new Vector3(
                coordinate.x * chunkSize,
                0,
                coordinate.y * chunkSize
            );


        GameObject chunk =
            Instantiate(
                prefab,
                position,
                Quaternion.identity
            );

        Chunk chunkScript = chunk.GetComponent<Chunk>();

        if(chunkScript != null && encounterManager != null)
        {
            chunkScript.Generate(
                encounterManager.difficulty
            );
        }


        activeChunks.Add(
            coordinate,
            chunk
        );
    }



    void RemoveOldChunks()
    {
        Vector2Int playerChunk = GetPlayerChunk();


        List<Vector2Int> remove =
            new List<Vector2Int>();


        foreach(KeyValuePair<Vector2Int, GameObject> chunk in activeChunks)
        {
            int distanceX =
                Mathf.Abs(
                    chunk.Key.x - playerChunk.x
                );


            int distanceZ =
                Mathf.Abs(
                    chunk.Key.y - playerChunk.y
                );


            if(distanceX > horizontalRange ||
               distanceZ > forwardRange)
            {
                Destroy(chunk.Value);

                remove.Add(chunk.Key);
            }
        }



        foreach(Vector2Int coordinate in remove)
        {
            activeChunks.Remove(coordinate);
        }
    }
}