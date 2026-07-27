using UnityEngine;

public class DeerChase : MonoBehaviour
{
    [Header("Looking")]
    public float lookAngle = 20f;
    public float requiredLookTime = 1.5f;
    public float maxMouseSpeed = 3f;


    [Header("Insanity")]
    public float failureInsanity = 25f;
    public float successInsanity = -10f;
    public float passiveInsanity = 0f;


    [Header("Event")]
    public float timeBeforeFailure = 10f;


    [Header("Audio")]
    public AudioSource audioSource;



    private Transform player;
    private Camera playerCamera;

    private MatchManager matchManager;
    private InsanityManager insanityManager;

    private DeerController controller;



    private float lookTimer;
    private float eventTimer;

    private bool active;



    public void Setup(
        Transform target,
        Camera cam,
        MatchManager match,
        InsanityManager insanity,
        DeerController owner)
    {
        player = target;
        playerCamera = cam;

        matchManager = match;
        insanityManager = insanity;

        controller = owner;


        FacePlayer();


        active = true;

        lookTimer = 0;
        eventTimer = 0;



        if(audioSource != null)
        {
            audioSource.Play();
        }
    }



    void Update()
    {
        if(!active)
            return;

        FacePlayer();


        eventTimer += Time.deltaTime;



        if(insanityManager != null)
        {
            insanityManager.IncreaseInsanity(
                passiveInsanity *
                Time.deltaTime
            );
        }



        if(
            matchManager != null &&
            matchManager.IsMatchLit &&
            PlayerLookingAtEnemy() &&
            MouseIsSlow()
        )
        {
            lookTimer += Time.deltaTime;


            if(lookTimer >= requiredLookTime)
            {
                FoundEnemy();
            }
        }
        else
        {
            lookTimer = 0;
        }



        if(eventTimer >= timeBeforeFailure)
        {
            MissedEnemy();
        }
    }



    bool PlayerLookingAtEnemy()
    {
        Vector3 direction =
            transform.position -
            playerCamera.transform.position;



        float angle =
            Vector3.Angle(
                playerCamera.transform.forward,
                direction
            );


        if(angle > lookAngle)
            return false;



        RaycastHit hit;


        if(Physics.Raycast(
            playerCamera.transform.position,
            direction.normalized,
            out hit,
            100f))
        {
            if(hit.transform.root == transform)
            {
                return true;
            }
        }


        return false;
    }



    bool MouseIsSlow()
    {
        float mouseMovement =
            Mathf.Abs(Input.GetAxis("Mouse X")) +
            Mathf.Abs(Input.GetAxis("Mouse Y"));


        return mouseMovement < maxMouseSpeed;
    }



    void FoundEnemy()
    {
        active = false;


        if(insanityManager != null)
        {
            insanityManager.DecreaseInsanity(
                successInsanity
            );
        }


        Remove();
    }



    void MissedEnemy()
    {
        active = false;


        if(insanityManager != null)
        {
            insanityManager.IncreaseInsanity(
                failureInsanity
            );
        }


        Remove();
    }



    void Remove()
    {
        if(controller != null)
        {
            controller.DeerDestroyed();
        }


        Destroy(gameObject);
    }



    void FacePlayer()
    {
        Vector3 direction = player.position - transform.position;

        direction.y = 0;

        if(direction == Vector3.zero)
            return;

        Quaternion rotation =
            Quaternion.LookRotation(direction);

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                rotation,
                5f * Time.deltaTime
            );
    }
}