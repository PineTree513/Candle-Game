using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject PlayerLight = other.gameObject.transform.GetChild(0).gameObject;

            PlayerLight.SetActive(false);
        }
    }
}
