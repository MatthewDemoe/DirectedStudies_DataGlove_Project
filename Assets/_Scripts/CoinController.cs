//2020-04-23
//Matthew Demoe 
//Developed for Directed Studies in IT under Alvaro Joffre Uribe-Quevedo

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Coin"))
        {
            other.transform.SetParent(transform);
            other.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
