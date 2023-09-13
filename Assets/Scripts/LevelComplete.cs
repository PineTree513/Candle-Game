using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    [SerializeField] private int world;
    [SerializeField] private int level;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SaveDataStatic.saveData.levelArray[world][level].levelComplete = true;
            explosion.Play();
            Destroy(gameObject);
        }
    }
}
