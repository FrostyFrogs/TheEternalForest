using UnityEngine;

public class Chunk : MonoBehaviour
{
    public LayerMask WhatIsGround;


    [Header("Objects")]
    public GameObject bearTrapPrefab;
    public GameObject matchboxPrefab;


    [Header("Matchboxes")]
    public int startingMatchboxes = 3;
    public int minimumMatchboxes = 2;


    [Header("Bear Traps")]
    public int startingBearTraps = 0;
    public int maximumBearTraps = 5;

    [Header("Spawn")]
    public float spawnRange = 187f;



    public void Generate(float difficulty)
    {

        SpawnMatchboxes(difficulty);
        SpawnBearTraps(difficulty);
    }



    void SpawnMatchboxes(float difficulty)
    {
        int amount =
            Mathf.RoundToInt(
                Mathf.Lerp(
                    startingMatchboxes,
                    minimumMatchboxes,
                    difficulty
                )
            );


        float chance =
            Mathf.Lerp(
                0.6f,
                0.35f,
                difficulty
            );


        for(int i = 0; i < amount; i++)
        {
            if(Random.value <= chance)
            {
                SpawnObject(matchboxPrefab);
            }
        }
    }



    void SpawnBearTraps(float difficulty)
    {
        int amount =
            Mathf.RoundToInt(
                Mathf.Lerp(
                    startingBearTraps,
                    maximumBearTraps,
                    difficulty
                )
            );


        float chance =
            Mathf.Lerp(
                0.15f,
                0.8f,
                difficulty
            );


        for(int i = 0; i < amount; i++)
        {
            if(Random.value <= chance)
            {
                SpawnObject(bearTrapPrefab);
            }
        }
    }



    void SpawnObject(GameObject prefab)
    {
        if(prefab == null)
            return;


        float x =
            Random.Range(
                -spawnRange,
                spawnRange
            );


        float z =
            Random.Range(
                -spawnRange,
                spawnRange
            );


        Vector3 randomPosition =
            transform.position +
            new Vector3(
                x,
                50f,
                z
            );


        RaycastHit hit;


        if(Physics.Raycast(
            randomPosition,
            Vector3.down,
            out hit,
            100f, WhatIsGround
        ))
        {
            Instantiate(
                prefab,
                hit.point,
                Quaternion.FromToRotation(
                    Vector3.up,
                    hit.normal
                ),
                transform
            );
        }
    }
}