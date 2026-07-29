using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InsanityManager : MonoBehaviour
{
    [Header("Insanity")]
    public float insanity = 0f;
    public float maxInsanity = 100f;

    [Header("Visual Effects")]
    public Image insanityVignette;

    public float maxVignetteAlpha = 0.65f;

    [Header("Audio Effects")]
    public AudioSource insanityRing;
    public AudioSource breathing;

    public float maxRingVolume = 0.5f;
    public float maxBreatheVolume = 0.5f;

    [Header("Game Over Effect")]
    public Image insanityOverlay;

    public float gameOverFadeTime = 3f;

    public float finalRingVolume = 1f;

    private bool gameOverStarted;

    void Update()
    {
        UpdateInsanityEffect();

        if(!gameOverStarted)
        {
            UpdateInsanityAudio();
        }

        if(insanity >= maxInsanity)
        {
            BecomeInsane();
        }
    }



    public void IncreaseInsanity(float amount)
    {
        insanity += amount;
        insanity = Mathf.Clamp(insanity, 0, maxInsanity);
    }



    public void DecreaseInsanity(float amount)
    {
        insanity -= amount;

        insanity = Mathf.Clamp(insanity, 0, maxInsanity);
    }



    void BecomeInsane()
    {
        if(gameOverStarted)
            return;


        gameOverStarted = true;

        StartCoroutine(GameOverSequence());
    }

    void UpdateInsanityEffect()
    {
        if(insanityVignette == null)
            return;


        float insanityPercent =
            insanity / maxInsanity;


        Color color =
            insanityVignette.color;


        color.a =
            Mathf.Lerp(
                0f,
                maxVignetteAlpha,
                insanityPercent
            );


        insanityVignette.color = color;
    }

    void UpdateInsanityAudio()
    {
        breathing.pitch = 2f;

        if(insanityRing == null)
            return;

        if(breathing == null)
            return;


        float ringAmount =
            Mathf.Clamp01(insanity / 50f);

        float breatheAmount =
            Mathf.Clamp01(insanity / 50f);


        insanityRing.volume =
            Mathf.Lerp(
                0f,
                maxRingVolume,
                ringAmount
            );

        breathing.volume =
            Mathf.Lerp(
                0f,
                maxBreatheVolume,
                breatheAmount
            );

        if(!insanityRing.isPlaying)
        {
            insanityRing.Play();
        }

        if(!breathing.isPlaying)
        {
            breathing.Play();
        }
    }

    IEnumerator GameOverSequence()
    {
        float timer = 0f;


        Color overlayColor =
            insanityOverlay.color;


        float startAlpha =
            overlayColor.a;



        float startVolume =
            insanityRing.volume;



        while(timer < gameOverFadeTime)
        {
            timer += Time.deltaTime;


            float progress =
                timer / gameOverFadeTime;



            // Fade screen to red
            if(insanityOverlay != null)
            {
                overlayColor.a =
                    Mathf.Lerp(
                        startAlpha,
                        1f,
                        progress
                    );


                insanityOverlay.color =
                    overlayColor;
            }



            // Drown out everything with ringing
            if(insanityRing != null)
            {
                insanityRing.volume =
                    Mathf.Lerp(
                        startVolume,
                        finalRingVolume,
                        progress
                    );
            }



            yield return null;
        }



        // Put death/restart/menu code here
    }
}