using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionalDoor : InteractableBase
{
    [SerializeField] private PlayerMovement playerMovement;

    public override void Interact()
    {
        var transitionData = gameObject.GetComponent<TransitionData>();

        playerMovement.transition = true;
        playerMovement.scene = transitionData.targetScene;
        SaveDataStatic.saveData.playerX = transitionData.playerX;
        SaveDataStatic.saveData.playerY = transitionData.playerY;
        SaveDataStatic.saveData.scene = transitionData.targetScene;
        playerMovement.StopCoroutine("ScreenFade");
        playerMovement.StartCoroutine("ScreenFade", 1);
        playerMovement.Invoke("MoveScene", 1.5f);
    }
}
