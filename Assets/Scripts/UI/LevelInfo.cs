using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class LevelInfo : MonoBehaviour
{
    [SerializeField] private int world;
    [SerializeField] private int level;

    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private TextMeshProUGUI bestTime;
    [SerializeField] private GameObject levelComplete;

    void Start()
    {
        LevelData levelData = SaveDataStatic.saveData.levelArray[world][level];
        levelName.text = levelData.levelName;
        bestTime.text = FormatTime(levelData.bestTime);
        levelComplete.SetActive(levelData.levelComplete);
    }
    private string FormatTime(float seconds)
    {
        string hours = Mathf.Floor(seconds / 3600).ToString();
        seconds %= 3600;
        string minutes = TwoNums(Mathf.Floor(seconds / 60));
        seconds %= 60;
        string stringSeconds = TwoNums(Mathf.Floor(seconds));
        seconds %= 1;
        string milliseconds = Mathf.Floor(seconds * 1000).ToString();
        return hours + ":" + minutes + ":" + stringSeconds + "." + milliseconds;
    }

    private string TwoNums(float time)
    {
        if (time < 10)
        {
            return "0" + time.ToString();
        }

        return time.ToString();
    }

}
