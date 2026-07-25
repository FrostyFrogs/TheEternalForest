//AI
using UnityEngine;
using System.Collections;

public class MatchManager : MonoBehaviour
{
    // The Match object in the scene
    public GameObject match;

    // Scripts on the Match object
    private Match matchScript;
    private MoveItem moveItem;


    // Number of matches available
    public int matchesRemaining = 5;

    // How long the match burns
    public float burnTime = 10f;

    // How far the match moves down when hidden
    public float dropDistance = 0.5f;

    // Speed of raising/lowering animation
    public float moveSpeed = 3f;


    // Current states
    private bool equipped = false;
    private bool lit = false;
    private bool moving = false;

    public bool IsMatchLit
    {
        get{return lit;}
    }

    // The normal position of the match in front of the camera
    private Vector3 defaultOffset;


    void Start()
    {
        // Get components from Match object
        matchScript = match.GetComponent<Match>();
        moveItem = match.GetComponent<MoveItem>();

        // Save the normal holding position
        defaultOffset = moveItem.currentOffset;

        // Hide match at the start
        match.SetActive(false);
    }



    void Update()
    {
        // Q = equip or unequip
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Equip match
            if (!equipped && matchesRemaining > 0 && !moving)
            {
                EquipMatch();
            }

            // Unequip match if it is not lit
            else if (equipped && !lit && !moving)
            {
                StartCoroutine(LowerMatch());
            }
        }


        // Right click = light match
        if (equipped && !lit && Input.GetMouseButtonDown(1))
        {
            StartCoroutine(LightMatch());
        }
    }

    public void AddMatches(int amount)
    {
        matchesRemaining += amount;
    }

    void EquipMatch()
    {
        // Show the match
        match.SetActive(true);

        // Start below the screen
        Vector3 startPosition = defaultOffset + Vector3.down * dropDistance;

        moveItem.currentOffset = startPosition;

        equipped = true;
        lit = false;

        // Raise into view
        StartCoroutine(RaiseMatch());
    }



    IEnumerator LightMatch()
    {
        // Small delay for strike animation
        yield return new WaitForSeconds(0.4f);

        // Light match
        lit = true;

        matchScript.Light();


        // Burn duration
        yield return new WaitForSeconds(burnTime);


        // Extinguish
        matchScript.Extinguish();


        // Remove burnt match
        StartCoroutine(BurnedMatch());
    }

    IEnumerator BurnedMatch()
    {
        moving = true;


        // Move match down
        Vector3 start = moveItem.currentOffset;

        Vector3 end = defaultOffset + Vector3.down * dropDistance;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * moveSpeed;

            moveItem.currentOffset =
                Vector3.Lerp(start, end, t);

            yield return null;
        }

        // Consume match
        matchesRemaining--;

        equipped = false;
        lit = false;

        moving = false;


        // Spawn next match if available
        if (matchesRemaining > 0)
        {
            EquipMatch();
        }
        else
        {
            match.SetActive(false);
        }
    }

    IEnumerator LowerMatch()
    {
        moving = true;

        Vector3 start = moveItem.currentOffset;

        Vector3 end = defaultOffset + Vector3.down * dropDistance;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * moveSpeed;

            moveItem.currentOffset = Vector3.Lerp(start, end, t);

            yield return null;
        }

        moveItem.currentOffset = end;

        // Hide match
        match.SetActive(false);

        equipped = false;

        moving = false;
    }

    IEnumerator RaiseMatch()
    {
        moving = true;

        Vector3 start = moveItem.currentOffset;

        Vector3 end = defaultOffset;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * moveSpeed;

            moveItem.currentOffset = Vector3.Lerp(start, end, t);

            yield return null;
        }


        moveItem.currentOffset = end;

        moving = false;
    }
}