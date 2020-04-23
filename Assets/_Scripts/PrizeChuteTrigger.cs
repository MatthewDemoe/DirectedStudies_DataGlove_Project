//2020-04-23
//Matthew Demoe 
//Developed for Directed Studies in IT under Alvaro Joffre Uribe-Quevedo

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PrizeChuteTrigger : MonoBehaviour
{
    [SerializeField]
    UnityEvent onPrizeWon = new UnityEvent();

    [SerializeField]
    GameObject confetti;

    [SerializeField]
    AudioSource audio;

    [SerializeField]
    AudioClip ding;

    private void Start()
    {
        onPrizeWon.AddListener(() => {
            Destroy(Instantiate(confetti, transform.position, confetti.transform.rotation), 2.0f);
            audio.PlayOneShot(ding);
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Coin"))
        {
            onPrizeWon.Invoke();
            Destroy(other.gameObject);
        }
    }
}
