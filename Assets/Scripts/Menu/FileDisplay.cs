using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FileDisplay : MonoBehaviour
{
    public int fileNum = 0;

    public TextMeshProUGUI playTimeDisplay;
    public TextMeshProUGUI areaDisplay;
    private SaveData saveData;

    void OnEnable()
    {
        saveData = MenuDataStatic.files[fileNum];

        if (saveData == null)
        {
            playTimeDisplay.text = "0:00:00.000";
            areaDisplay.text = "New Game";
        }
        else
        {
            playTimeDisplay.text = FormatTime();
            areaDisplay.text = saveData.scene;
        }
    }

    private string FormatTime()
    {
        float seconds = saveData.playTime;
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
