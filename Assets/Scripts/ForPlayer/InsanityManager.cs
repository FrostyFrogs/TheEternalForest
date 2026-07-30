using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class InsanityManager : MonoBehaviour
{
    [Header("Insanity")]
    public float insanity = 0f;
    public float maxInsanity = 100f;


    [Header("Visual Effects")]
    public Image insanityVignette;
    public float maxVignetteAlpha = 0.65f;


    [Header("Distance")]
    public Transform player;
    private Vector3 startPosition;
    public float distanceTraveled;


    [Header("Audio Effects")]
    public AudioSource insanityRing;
    public AudioSource breathing;

    public float maxRingVolume = 0.5f;
    public float maxBreatheVolume = 0.5f;


    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TMP_Text distanceText;


    [Header("Game Over Effect")]
    public Image insanityOverlay;

    public float gameOverFadeTime = 3f;
    public float finalRingVolume = 1f;


    private bool gameOverStarted;



    void Start()
    {
        if(player != null)
        {
            startPosition = player.position;
        }


        if(gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }



    void Update()
    {
        UpdateInsanityEffect();


        if(!gameOverStarted)
        {
            UpdateInsanityAudio();
        }


        if(player != null)
        {
            distanceTraveled =
                Vector3.Distance(
                    startPosition,
                    player.position
                );
        }


        if(insanity >= maxInsanity)
        {
            BecomeInsane();
        }


        if(gameOverStarted &&
           Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }



    public void IncreaseInsanity(float amount)
    {
        insanity += amount;

        insanity =
            Mathf.Clamp(
                insanity,
                0,
                maxInsanity
            );
    }



    public void DecreaseInsanity(float amount)
    {
        insanity -= amount;

        insanity =
            Mathf.Clamp(
                insanity,
                0,
                maxInsanity
            );
    }



    void BecomeInsane()
    {
        if(gameOverStarted)
            return;


        gameOverStarted = true;


        StartCoroutine(
            GameOverSequence()
        );
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
        if(insanityRing == null ||
           breathing == null)
            return;


        float insanityAmount =
            Mathf.Clamp01(
                insanity / 50f
            );


        insanityRing.volume =
            Mathf.Lerp(
                0f,
                maxRingVolume,
                insanityAmount
            );


        breathing.volume =
            Mathf.Lerp(
                0f,
                maxBreatheVolume,
                insanityAmount
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
            insanityOverlay != null
            ? insanityOverlay.color
            : Color.clear;


        float startAlpha =
            overlayColor.a;


        float startVolume =
            insanityRing != null
            ? insanityRing.volume
            : 0f;



        while(timer < gameOverFadeTime)
        {
            timer += Time.unscaledDeltaTime;


            float progress =
                timer / gameOverFadeTime;



            // Red screen fade
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



            // Ringing overwhelms audio
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



        if(gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }



        if(distanceText != null)
        {
            distanceText.text =
                "DISTANCE TRAVELED: " +
                Mathf.RoundToInt(distanceTraveled)
                + "m";
        }



        Time.timeScale = 0f;
    }



    void RestartGame()
    {
        Time.timeScale = 1f;


        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}