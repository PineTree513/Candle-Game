using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : InteractableBase
{
    [Header("Objects must contain an ISwitchable")]
    [SerializeField] private GameObject[] connectedGameObjects;

    public override void Interact()
    {
        transform.rotation *= Quaternion.AngleAxis(180, Vector3.up);

        foreach (GameObject connectedGameObject in connectedGameObjects)
        {
            connectedGameObject.GetComponent<ISwitchable>().Switch();
        }
    }
}
