using UnityEngine;
using System.Collections;

public class ForwardEntity : MonoBehaviour
{
    [Header("Distance")]
    // Distance required for entity to disappear
    public float disappearDistance = 3f;

    [Header("Fade")]
    public float fadeStartDistance = 12f;
    public float fadeEndDistance = 5f;
    public float disappearFadeTime = 0.2f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    [Header("Looking")]
    // Maximum distance where looking counts
    public float lookDistance = 40f;

    // How wide the player's view cone is
    public float lookAngle = 45f;


    [Header("Insanity")]
    public float insanityPerSecond = 5f;

    private InsanityManager insanityManager;



    [Header("Lifetime")]
    // Prevents the entity existing forever
    public float lifeTime = 15f;

    [Header("Movement")]
    public float retreatSpeed = 1f;
    public float shiftRadius = 0.5f;
    public float shiftSpeed = 1.5f;
    private Vector3 startPosition;

    private Transform player;

    private float timer;

    private bool disappearing;



    // Called by the manager after spawning
    public void Setup(
        Transform target,
        InsanityManager manager)
    {   
        startPosition = transform.position;
        player = target;
        insanityManager = manager;

        spriteRenderer = GetComponent<SpriteRenderer>();

        if(spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }




    void Update()
    {
        if(player == null)
            return;

        if (disappearing)
            return;

        ShiftAround();
        MoveAway();
        FacePlayer();

        // Distance between player and entity
        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        // Player reached it
        FadeWithDistance(distance);

        if(distance <= disappearDistance)
        {
            Disappear();
            return;
        }



        // If player ignores it, increase insanity
        if(!IsPlayerLooking())
        {
            if(insanityManager != null)
            {
                insanityManager.IncreaseInsanity(
                    insanityPerSecond *
                    Time.deltaTime
                );
            }
        }



        // Lifetime timer
        timer += Time.deltaTime;


        if(timer >= lifeTime)
        {
            Disappear();
        }
    }



    bool IsPlayerLooking()
    {
        // Direction from player to entity
        Vector3 direction =
            transform.position -
            player.position;



        // Too far away
        if(direction.magnitude > lookDistance)
        {
            return false;
        }



        // Compare player facing direction
        float angle =
            Vector3.Angle(
                player.forward,
                direction
            );


        // Inside viewing cone
        return angle <= lookAngle;
    }



    void Disappear()
    {
        if(disappearing)
            return;

        disappearing = true;

        StartCoroutine(FadeOut());
    }


    IEnumerator FadeOut()
    {
        float timer = 0f;

        Color color = spriteRenderer.color;
        float startAlpha = color.a;

        while (timer < disappearFadeTime)
        {
            timer += Time.deltaTime;

            color.a = Mathf.Lerp(
                startAlpha,
                0f,
                timer / disappearFadeTime
            );

            spriteRenderer.color = color;

            yield return null;
        }

        Destroy(gameObject);
    }

    void FadeWithDistance(float distance)
    {
        if(spriteRenderer == null)
            return;


        float alpha = Mathf.InverseLerp(
            fadeEndDistance,
            fadeStartDistance,
            distance
        );


        Color color = originalColor;
        color.a = alpha;

        spriteRenderer.color = color;
    }

    void ShiftAround()
    {
        Vector3 offset = new Vector3(
            Mathf.Sin(Time.time * shiftSpeed),
            0,
            Mathf.Cos(Time.time * shiftSpeed * 0.8f)
        ) * shiftRadius;

        transform.position = startPosition + offset;
    }

    void MoveAway()
    {
        Vector3 direction =
            (transform.position - player.position).normalized;

        direction.y = 0;

        startPosition +=
            direction *
            retreatSpeed *
            Time.deltaTime;
    }

    void FacePlayer()
    {
        Vector3 direction =
            player.position - transform.position;

        direction.y = 0;

        if(direction == Vector3.zero)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(-direction);

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                2f * Time.deltaTime
            );
    }
}