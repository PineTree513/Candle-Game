using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private bool followY;

    void Update()
    {
        if (followY)
        {
            transform.position = new Vector3(transform.position.x, targetTransform.position.y + 1, transform.position.z);
        }
    }
}
