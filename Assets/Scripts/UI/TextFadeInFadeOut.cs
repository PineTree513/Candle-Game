using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFadeInFadeOut : MonoBehaviour
{
    private float opacity = 0;
    public TextMeshProUGUI title;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        title.color = new Color(255, 255, 255, opacity);

        if (Time.timeSinceLevelLoad < 2)
        {
            opacity += (float)(1 * Time.deltaTime);
        }
        else
        {
            opacity += (float)(-.4 * Time.deltaTime);
        }
    }
}
