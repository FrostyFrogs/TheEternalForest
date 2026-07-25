using UnityEngine;

public class StampedeEnemy : MonoBehaviour
{
    private Vector3 movementDirection;

    private float speed;

    private bool moving;


    private Transform player;

    private bool chasing;

    public float lifeTime = 5f;

    public InsanityManager insanityManager;
    public float insanityDamage;



    // Used for walking state
    public void StartCrossing(Vector3 right, float side, float moveSpeed)
    {
        movementDirection = -right * side;
        speed = moveSpeed;
        moving = true;

        transform.rotation = Quaternion.LookRotation(movementDirection);

        Destroy(gameObject, lifeTime);
    }



    // Used for sprinting
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
        Vector3 direction =
            player.position -
            transform.position;


        direction.y = 0;


        transform.position +=
            direction.normalized *
            speed *
            Time.deltaTime;


        transform.rotation =
            Quaternion.LookRotation(direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {   
            insanityManager.IncreaseInsanity(insanityDamage);
            Destroy(gameObject);
        }
    }
}