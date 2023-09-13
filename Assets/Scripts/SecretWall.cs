using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SecretWall : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private float fadeTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopCoroutine("FadeIn");
            StartCoroutine("FadeOut");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopCoroutine("FadeOut");
            StartCoroutine("FadeIn");
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = fadeTime * tilemap.color.a;

        while (tilemap.color.a < 1)
        {
            elapsedTime += Time.deltaTime;
            tilemap.color = new Color(255, 255, 255, elapsedTime / fadeTime);

            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = fadeTime * (1 - tilemap.color.a);

        while (tilemap.color.a > 0)
        {
            elapsedTime += Time.deltaTime;
            tilemap.color = new Color(255, 255, 255, 1 - elapsedTime / fadeTime);

            yield return null;
        }
    }
}
