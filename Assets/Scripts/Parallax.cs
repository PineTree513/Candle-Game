using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallaxStrength;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private Vector3 playerStartPos;
    [SerializeField] private Vector3 layerStartPos;

    [SerializeField] private Transform mainCameraTransform;

    void Start()
    {
        //layerStartPos = transform.position;
        //playerStartPos = Vector3.zero;
    }

    void FixedUpdate()
    {
        transform.position = new Vector2(mainCameraTransform.position.x, mainCameraTransform.position.y) * parallaxStrength;

        /*
        if (playerMovement.entered & !playerMovement.transition)
        {
            if (playerStartPos == Vector3.zero)
            {
                playerStartPos = playerTransform.position;
            }

            transform.position = layerStartPos + (playerTransform.position - playerStartPos) * parallaxStrength;
        }
        */
    }
}
