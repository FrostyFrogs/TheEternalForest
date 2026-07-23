using UnityEngine;
using UnityEngine.UI;

public class InsanityManager : MonoBehaviour
{
    [Header("Insanity")]
    public float insanity = 0f;
    public float maxInsanity = 100f;


    [Header("Optional UI")]
    public Slider insanityBar;

    

    void Start()
    {
        //DEMO TESTING
        if(insanityBar != null)
        {
            insanityBar.value = 0;
        }
    }

    void Update()
    {
        //DEMO AND TESTING PURPOSES ONLY INSANITY SLIDER
        if (insanityBar != null)
        {
            insanityBar.value = insanity / maxInsanity;
        }


        // Maximum insanity reached
        if (insanity >= maxInsanity)
        {
            BecomeInsane();
        }
    }



    public void IncreaseInsanity(float amount)
    {
        insanity += amount;

        insanity = Mathf.Clamp(
            insanity,
            0,
            maxInsanity
        );
    }



    public void DecreaseInsanity(float amount)
    {
        insanity -= amount;

        insanity = Mathf.Clamp(
            insanity,
            0,
            maxInsanity
        );
    }



    void BecomeInsane()
    {
        Debug.Log("Maximum insanity reached");

        // Add hallucinations/game effects here
    }
}