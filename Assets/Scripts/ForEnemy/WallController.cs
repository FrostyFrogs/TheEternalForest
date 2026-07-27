using UnityEngine;

public class WallController : MonoBehaviour
{
    [Header("Movement")]
    public float normalSpeed = 2f;
    public float catchUpSpeed = 15f;

    private bool active;
    private bool catchingUp;


    [Header("Distance")]
    public float desiredDistance = 100f;


    [Header("References")]
    public Transform player;


    [Header("Insanity")]
    public InsanityManager insanityManager;
    public float insanityPerSecond = 10f;
    public float dangerDistance = 20f;



    void Update()
    {
        if(!active)
            return;

        if(player == null)
            return;


        float distanceBehind =
            player.position.z - transform.position.z;



        if(distanceBehind > desiredDistance)
        {
            catchingUp = true;
        }



        float speed =
            catchingUp
            ? catchUpSpeed
            : normalSpeed;



        transform.position +=
            transform.forward *
            speed *
            Time.deltaTime;



        if(distanceBehind <= desiredDistance)
        {
            catchingUp = false;
        }



        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );


        if(distance <= dangerDistance)
        {
            if(insanityManager != null)
            {
                insanityManager.IncreaseInsanity(
                    insanityPerSecond *
                    Time.deltaTime
                );
            }
        }
    }



    public void StartWall()
    {
        active = true;
    }



    public void ForceCatchUp(Vector3 targetPosition)
    {
        if(transform.position.z < targetPosition.z)
        {
            catchingUp = true;
        }
    }
}