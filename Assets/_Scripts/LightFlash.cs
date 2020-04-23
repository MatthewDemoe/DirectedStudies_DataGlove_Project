//2020-04-23
//Matthew Demoe 
//Developed for Directed Studies in IT under Alvaro Joffre Uribe-Quevedo

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlash : MonoBehaviour
{
    Light light;

    [SerializeField]
    float flashDuration = 1.5f;

    float flashTimer = 0.0f;

    [SerializeField]
    float flashIncrement = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        //StartFlashing();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartFlashing()
    {

        StartCoroutine("FlashLights");
    }

    IEnumerator FlashLights()
    {
        while (flashTimer <= flashDuration)
        {
            flashTimer += flashIncrement;

            float r = Random.Range(0.0f, 1.0f);
            float g = Random.Range(0.0f, 1.0f);
            float b = Random.Range(0.0f, 1.0f);

            light.color = new Color(r, g, b, 1.0f);

            yield return new WaitForSeconds(flashIncrement);
        }

        light.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        flashTimer = 0.0f;
        StopCoroutine("FlashLights");
    }
}
