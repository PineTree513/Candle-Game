using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideEntrance : MonoBehaviour
{
    public PlayerMovement script;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (script.entered)
        {
            Destroy(gameObject);
        }
    }
}
