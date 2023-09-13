using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Flicker : MonoBehaviour
{
    public Light2D lightSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 20) == 0)
        {
            lightSource.intensity = .5f;
        }
        else
        {
            lightSource.intensity = 1f;
        }
    }
}
