using UnityEngine;
using System.Collections.Generic;

public class ChunkManager : MonoBehaviour
{
    [Header("Chunks")]
    public GameObject[] chunkPrefabs;

    [Header("Generation")]
    public Transform player;

    private List<GameObject> activeChunks = new List<GameObject>();

    private Transform nextSpawnPoint;


    [Header("Wall")]
    public WallController wall;
    public int startingChunks = 5;

    private bool wallStarted;



    void Start()
    {
        SpawnStartingChunk();
    }



    void Update()
    {
        if(nextSpawnPoint == null)
            return;


        float distance =
            Vector3.Distance(
                player.position,
                nextSpawnPoint.position
            );


        if(distance < 50f)
        {
            SpawnChunk();
        }
    }



    void SpawnStartingChunk()
    {
        GameObject chunk =
            Instantiate(
                chunkPrefabs[0],
                transform.position,
                Quaternion.identity
            );


        activeChunks.Add(chunk);


        Chunk chunkScript =
            chunk.GetComponent<Chunk>();


        nextSpawnPoint =
            chunkScript.endPoint;
    }



    void SpawnChunk()
    {
        GameObject prefab =
            chunkPrefabs[
                Random.Range(0, chunkPrefabs.Length)
            ];


        GameObject chunk =
            Instantiate(prefab);


        Chunk chunkScript =
            chunk.GetComponent<Chunk>();


        chunk.transform.position =
            nextSpawnPoint.position -
            chunkScript.startPoint.localPosition;


        chunk.transform.rotation =
            nextSpawnPoint.rotation;



        activeChunks.Add(chunk);


        nextSpawnPoint =
            chunkScript.endPoint;



        // Start wall once enough chunks exist
        if(!wallStarted && activeChunks.Count >= startingChunks)
        {
            if(wall != null)
            {
                wall.StartWall();
                wallStarted = true;

                Debug.Log("Wall Started");
            }
        }



        // Delete old chunks
        const int maxChunks = 6;

        if(activeChunks.Count > maxChunks)
        {
            Destroy(activeChunks[0]);

            activeChunks.RemoveAt(0);


            if(wall != null)
            {
                wall.ForceCatchUp(
                    nextSpawnPoint.position
                );
            }
        }
    }
}