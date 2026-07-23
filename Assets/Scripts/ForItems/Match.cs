using UnityEngine;

public class Match : MonoBehaviour
{
    public GameObject flame;

    public Light flameLight;

    void Start()
    {
        flame.SetActive(false);
        flameLight.enabled = false;
    }

    public void Light()
    {
        flame.SetActive(true);
        flameLight.enabled = true;
    }

    public void Extinguish()
    {
        flame.SetActive(false);
        flameLight.enabled = false;
    }
}