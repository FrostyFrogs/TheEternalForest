using UnityEngine;
using System.Collections;

public class StalkerChase : MonoBehaviour
{
    [Header("Looking")]
    public float lookAngle = 25f;
    public float requiredLookTime = 3f;


    [Header("Insanity")]
    public float passiveInsanity = 5f;
    public float screamInsanity = 30f;
    public float successInsanity = 15f;

    [Header("Following")]
    public float followDistance = 15f;
    public float followSpeed = 3f;


    [Header("Audio")]
    public AudioSource screamAudio;



    private Transform player;
    private Camera playerCamera;

    private MatchManager matchManager;
    private InsanityManager insanityManager;


    private float lookTimer;
    private bool screamed;
    private bool disappearing;
    private bool angered;



    public void Setup(
        Transform target,
        Camera cam,
        MatchManager match,
        InsanityManager insanity)
    {
        player = target;
        playerCamera = cam;
        matchManager = match;
        insanityManager = insanity;


        FacePlayer();
    }



    void Update()
    {
        if(disappearing || player == null)
            return;


        FollowBehindPlayer();


        if(insanityManager != null)
        {
            insanityManager.IncreaseInsanity(
                passiveInsanity *
                Time.deltaTime
            );
        }


        if(PlayerLooking())
        {
            lookTimer += Time.deltaTime;


            if(lookTimer >= requiredLookTime)
            {
                React();
            }
        }
        else
        {
            lookTimer = 0;
        }
}



    bool PlayerLooking()
    {
        Vector3 direction =
            transform.position -
            playerCamera.transform.position;


        float angle =
            Vector3.Angle(
                playerCamera.transform.forward,
                direction
            );


        return angle <= lookAngle;
    }



    void React()
    {
        if(matchManager.IsMatchLit)
        {
            if(screamed)
                return;

            screamed = true;


            if(screamAudio != null)
                screamAudio.Play();


            if(insanityManager != null)
            {
                insanityManager.IncreaseInsanity(
                    screamInsanity
                );
            }


            lookTimer = 0;
        }
        else
        {
            disappearing = true;
            StartCoroutine(Remove());
        }
    }


    void FacePlayer()
    {
        Vector3 direction =
            player.position -
            transform.position;

        direction.y = 0;


        if(direction != Vector3.zero)
        {
            transform.rotation =
                Quaternion.LookRotation(direction);
        }
    }



    IEnumerator Remove()
    {
        yield return new WaitForSeconds(1f);

        insanityManager.DecreaseInsanity(successInsanity);
        Destroy(gameObject);
    }

    void FollowBehindPlayer()
    {
        Vector3 targetPosition =
            player.position -
            player.forward * followDistance;


        targetPosition.y = transform.position.y;


        transform.position =
            Vector3.MoveTowards(
                transform.position,
                targetPosition,
                followSpeed * Time.deltaTime
            );


        FacePlayer();
    }
}