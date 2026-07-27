using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    public DeerController deer;
    public StalkerController stalker;
    public SmilerController smiler;
    public StampedeController stampede;


    [Header("Difficulty")]
    public float difficulty;

    public float timeDifficulty = 0.002f;
    public float distanceDifficulty = 0.002f;


    private float timeAlive;
    private float distanceTravelled;

    private Vector3 lastPosition;



    [Header("Deer")]
    public float deerChance = 0.35f;
    public float deerBaseCooldown = 45f;
    private float deerTimer;



    [Header("Stalker")]
    public float stalkerChance = 0.20f;
    public float stalkerBaseCooldown = 90f;
    private float stalkerTimer;



    [Header("Smiler")]
    public float smilerChance = 0.15f;
    public float smilerBaseCooldown = 120f;
    private float smilerTimer;



    [Header("Stampede")]
    public float stampedeChance = 0.05f;
    public float stampedeBaseCooldown = 180f;
    private float stampedeTimer;



    [Header("Minimum Cooldowns")]
    public float minimumCooldown = 20f;



    void Start()
    {
        lastPosition = player.position;


        // Randomize starting timers so everything does not sync
        deerTimer =
            Random.Range(0, deerBaseCooldown);

        stalkerTimer =
            Random.Range(0, stalkerBaseCooldown);

        smilerTimer =
            Random.Range(0, smilerBaseCooldown);

        stampedeTimer =
            Random.Range(0, stampedeBaseCooldown);
    }



    void Update()
    {
        CalculateDifficulty();


        deerTimer -= Time.deltaTime;
        stalkerTimer -= Time.deltaTime;
        smilerTimer -= Time.deltaTime;
        stampedeTimer -= Time.deltaTime;



        if(deerTimer <= 0)
        {
            DeerRoll();

            deerTimer =
                GetCooldown(deerBaseCooldown);
        }



        if(stalkerTimer <= 0)
        {
            StalkerRoll();

            stalkerTimer =
                GetCooldown(stalkerBaseCooldown);
        }



        if(smilerTimer <= 0)
        {
            SmilerRoll();

            smilerTimer =
                GetCooldown(smilerBaseCooldown);
        }



        if(stampedeTimer <= 0)
        {
            StampedeRoll();

            stampedeTimer =
                GetCooldown(stampedeBaseCooldown);
        }
    }



    void CalculateDifficulty()
    {
        timeAlive += Time.deltaTime;


        distanceTravelled +=
            Vector3.Distance(
                player.position,
                lastPosition
            );


        lastPosition = player.position;


        difficulty =
            (timeAlive * timeDifficulty) +
            (distanceTravelled * distanceDifficulty);


        difficulty =
            Mathf.Clamp01(difficulty);
    }



    float GetCooldown(float baseCooldown)
    {
        return Mathf.Lerp(
            baseCooldown,
            minimumCooldown,
            difficulty
        );
    }



    void DeerRoll()
    {
        float chance =
            deerChance +
            difficulty * 0.6f;


        if(Random.value < chance)
        {
            if(deer != null)
                deer.SpawnEnemy();
        }
    }



    void StalkerRoll()
    {
        float chance =
            stalkerChance +
            difficulty * 0.4f;


        if(Random.value < chance)
        {
            if(stalker != null)
                stalker.SpawnEnemy();
        }
    }



    void SmilerRoll()
    {
        float chance =
            smilerChance +
            difficulty * 0.6f;


        if(Random.value < chance)
        {
            if(smiler != null)
                smiler.SpawnEnemy();
        }
    }



    void StampedeRoll()
    {
        float chance =
            stampedeChance +
            difficulty * 0.5f;


        if(Random.value < chance)
        {
            if(stampede != null)
                stampede.StartStampede();
        }
    }
}