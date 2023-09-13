using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public LevelData[][] levelArray = new LevelData[][] { new LevelData[] { new LevelData(false, 0, "tutorial") } };

    public float playerX;
    public float playerY;

    public string scene;

    public float playTime;
}
