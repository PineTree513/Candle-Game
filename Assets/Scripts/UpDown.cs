using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDown : MonoBehaviour
{
    [SerializeField] private float length;
    [SerializeField] private float speed;
    private Vector2 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        //float offset = Mathf.PingPong(Time.time * speed, length) - length / 2;
        float offset = Mathf.Sin(Mathf.PingPong(Time.time * speed, Mathf.PI * 2f) + Mathf.PI * .5f) * length;

        transform.position = startPosition + Vector2.up * offset;
    }
}
