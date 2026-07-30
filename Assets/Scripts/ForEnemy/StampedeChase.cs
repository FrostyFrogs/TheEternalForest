using UnityEngine;

public class StampedeChase : MonoBehaviour
{
    private Vector3 movementDirection;

    private float speed;

    private bool moving;


    private Transform player;

    private bool chasing;


    public float lifeTime = 5f;


    [Header("Insanity")]
    public InsanityManager insanityManager;
    public float insanityDamage;



    public void StartCrossing(
        Vector3 right,
        float side,
        float moveSpeed)
    {
        movementDirection = -right * side;

        speed = moveSpeed;

        moving = true;


        transform.rotation =
            Quaternion.LookRotation(
                movementDirection
            );


        Destroy(gameObject, lifeTime);
    }



    public void StartChasing(
        Transform target,
        float chaseSpeed)
    {
        player = target;

        speed = chaseSpeed;

        chasing = true;
    }



    void Update()
    {
        if(moving)
        {
            transform.position +=
                movementDirection *
                speed *
                Time.deltaTime;
        }


        if(chasing)
        {
            Chase();
        }
    }



    void Chase()
    {
        if(player == null)
            return;


        Vector3 direction =
            player.position -
            transform.position;


        direction.y = 0;


        transform.position +=
            direction.normalized *
            speed *
            Time.deltaTime;


        if(direction != Vector3.zero)
        {
            transform.rotation =
                Quaternion.LookRotation(direction);
        }
    }



    // Original no-argument version
    public void PlayerDetected()
    {
        DamagePlayer();
    }



    // Compatibility with SpeedDetection.cs
    public void PlayerDetected(Transform target)
    {
        DamagePlayer();
    }



    // Extra compatibility if another script uses Collider
    public void PlayerDetected(Collider other)
    {
        DamagePlayer();
    }



    void DamagePlayer()
    {
        if(insanityManager != null)
        {
            insanityManager.IncreaseInsanity(
                insanityDamage
            );
        }


        Destroy(gameObject);
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerDetected(other.transform);
        }
    }
}