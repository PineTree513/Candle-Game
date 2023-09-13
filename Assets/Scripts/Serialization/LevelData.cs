using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelData
{
    public bool levelComplete;

    public float bestTime;

    public string levelName;

    public LevelData(bool _levelComplete, float _bestTime, string _levelName)
    {
        levelComplete = _levelComplete;
        bestTime = _bestTime;
        levelName = _levelName;
    }
}
