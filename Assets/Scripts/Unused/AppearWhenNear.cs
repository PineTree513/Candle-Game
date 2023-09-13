using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AppearWhenNear : MonoBehaviour
{
    [SerializeField] private GameObject targetGameObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            targetGameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            targetGameObject.SetActive(false);
        }
    }
}

/* 
[SerializeField] private Transform player;
[SerializeField] private Transform textOwner;
[SerializeField] private TextMeshProUGUI text;
[SerializeField] private float minDistance;
[SerializeField] private float maxDistance;
private float minMaxDif;
[SerializeField] private float alphaVal;

private void Start()
{
    minMaxDif = maxDistance - minDistance;
}

void Update()
{
    alphaVal = 255 * (1 - (Vector2.Distance(player.position, textOwner.position) - minDistance) / minMaxDif);
    text.alpha = Mathf.Clamp(alphaVal, 0, 255);
}
*/