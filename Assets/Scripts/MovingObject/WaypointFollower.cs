using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour, ISwitchable
{
    [SerializeField] private Transform[] waypoints;
    private int currentPoint = 0;

    [SerializeField] private int speed;

    [SerializeField] private Collider2D coll;

    [SerializeField] private LayerMask player;

    [SerializeField] private bool crushDetection;

    [SerializeField] private bool door;

    public bool startActive = true;
    public bool active;

    void Start()
    {
        active = startActive;

        transform.position = waypoints[0].position;
    }

    void Update()
    {
        WaypointLogic();

        if (active)
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoints[currentPoint].position, speed * Time.deltaTime);
        }
    }

    private void WaypointLogic()
    {
        if (CrushingPlayer())
        {
            PreviousPoint();
        }

        if (Vector2.Distance(waypoints[currentPoint].position, transform.position) < .1f)
        {
            NextPoint();

            if (door)
            {
                active = false;
            }
        }
    }

    private bool CrushingPlayer()
    {
        return Physics2D.BoxCast(coll.bounds.center, new Vector3(coll.bounds.size.x - .1f, coll.bounds.size.y - .1f, coll.bounds.size.z), 0f, Vector2.down, .1f, player);
    }

    private void PreviousPoint()
    {
        currentPoint--;

        if (currentPoint == -1)
        {
            currentPoint = waypoints.Length - 1;
        }
    }

    private void NextPoint()
    {
        currentPoint++;

        if (currentPoint == waypoints.Length)
        {
            currentPoint = 0;
        }
    }

    public void Switch()
    {
        active = !active;
    }
}
