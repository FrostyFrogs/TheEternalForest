using UnityEngine;

public class Match : MonoBehaviour
{
    public GameObject flame;
    public Light flameLight;

    public Light glowLight;

    void Start()
    {
        flame.SetActive(false);
        flameLight.enabled = false;
        glowLight.enabled = false;
    }

    public void Light()
    {
        flame.SetActive(true);
        flameLight.enabled = true;
        glowLight.enabled = true;
    }

    public void Extinguish()
    {
        flame.SetActive(false);
        flameLight.enabled = false;
        glowLight.enabled = false;
    }
}