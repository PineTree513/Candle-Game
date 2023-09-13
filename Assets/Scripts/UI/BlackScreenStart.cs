using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreenStart : MonoBehaviour
{
    private float opacity = 1;
    public GameObject panel;
    private Image img;

    void Start()
    {
        img = panel.GetComponent<Image>();
    }
    // Update is called once per frame
    void Update()
    {
        if (opacity > 0)
        {
            opacity += (float)(-.7 * Time.deltaTime);
            img.color = new Color(0, 0, 0, opacity);
        }
    }
}
