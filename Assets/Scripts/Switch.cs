using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private GameObject text;
    [SerializeField] private SpriteRenderer sprite;

    [SerializeField] private bool switchPulled = false;
    [SerializeField] private bool playerTouching = false;

    [SerializeField] private WaypointFollower[] waypointFollowers;

    private void Update()
    {
        if (playerTouching & Input.GetKeyDown(KeyCode.W))
        {
            switchPulled = !switchPulled;
            if (sprite != null)
            {
                sprite.flipX = switchPulled;
            }
        }

        if (waypointFollowers != null)
        {
            foreach (WaypointFollower waypointFollower in waypointFollowers)
            {
                //waypointFollower.active = switchPulled ^ waypointFollower.startActive;
                waypointFollower.active = !waypointFollower.active;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            text.SetActive(true);
            playerTouching = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            text.SetActive(false);
            playerTouching = false;
        }
    }
}
