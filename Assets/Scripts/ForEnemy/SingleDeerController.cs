using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Camera playerCamera;

    public MatchManager matchManager;
    public InsanityManager insanityManager;

    public AudioSource audioSource;



    [Header("Spawn")]
    public float sideDistanceMin = 6f;
    public float sideDistanceMax = 12f;

    public float forwardDistanceMin = 10f;
    public float forwardDistanceMax = 25f;



    [Header("Looking")]
    public float lookAngle = 20f;

    public float requiredLookTime = 1.5f;

    public float maxMouseSpeed = 3f;



    [Header("Insanity")]
    public float failureInsanity = 25f;
    public float successInsanity = -10f;



    [Header("Event")]
    public float timeBeforeFailure = 10f;



    private float lookTimer;
    private float eventTimer;

    private bool active;



    void Start()
    {
        gameObject.SetActive(false);
    }

    public void SpawnEnemy()
    {
        // Choose left or right
        float side = Random.value < 0.5f ? -1f : 1f;

        float sideDistance =
            Random.Range(
                sideDistanceMin,
                sideDistanceMax);

        float forwardDistance =
            Random.Range(forwardDistanceMin, forwardDistanceMax);

        transform.position = player.position + player.right * side * sideDistance + player.forward * forwardDistance;

        // Face player
        transform.LookAt(player);

        gameObject.SetActive(true);

        active = true;

        lookTimer = 0;
        eventTimer = 0;

        // Play sound cue
        audioSource.Play();
    }

    void Update()
    {
        if (!active)
            return;

        eventTimer += Time.deltaTime;

        // Only count looking if match is lit
        if (matchManager.IsMatchLit && PlayerLookingAtEnemy() && MouseIsSlow())
        {
            lookTimer += Time.deltaTime;
            if (lookTimer >= requiredLookTime)
            {
                FoundEnemy();
            }
        }
        else
        {
            lookTimer = 0;
        }



        // Player failed
        if(eventTimer >= timeBeforeFailure)
        {
            MissedEnemy();
        }

        insanityManager.IncreaseInsanity(0.01f);
    }

    bool PlayerLookingAtEnemy()
    {
        Vector3 direction = transform.position - playerCamera.transform.position;
        float angle = Vector3.Angle(playerCamera.transform.forward, direction);
        if(angle > lookAngle)
            return false;



        RaycastHit hit;


        if(Physics.Raycast(
            playerCamera.transform.position,
            direction.normalized,out hit, 100f))
        {
            if(hit.transform == transform)
            {
                return true;
            }
        }



        return false;
    }

    bool MouseIsSlow()
    {
        float mouseMovement = Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y"));
        return mouseMovement < maxMouseSpeed;
    }

    void FoundEnemy()
    {
        active = false;


        // Seeing it calms the player
        insanityManager.DecreaseInsanity(successInsanity);
        gameObject.SetActive(false);
    }

    void MissedEnemy()
    {
        active = false;
        // Failure increases insanity
        insanityManager.IncreaseInsanity(failureInsanity);
        gameObject.SetActive(false);
    }
}