using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject fileScreen;

    public string fileNum = "";

    private void HideAllMenus()
    {
        titleScreen.SetActive(false);
        fileScreen.SetActive(false);
    }

    public void GoToFileMenu()
    {
        GetMenuData();

        HideAllMenus();
        
        fileScreen.SetActive(true);
    }

    public void GoToMainMenu()
    {
        HideAllMenus();

        titleScreen.SetActive(true);
    }

    private void GetMenuData()
    {
        for (int i = 0; i < 3; i++)
        {
            MenuDataStatic.files[i] = SerializationManager.Load(i.ToString());

        }
    }

    public void DeleteFile()
    {
        string path = "C:/Users/cayde/AppData/LocalLow/DefaultCompany/Candle Game" + "/saves/" + fileNum + ".save";

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("File deleted at: " + path);
        }

        GoToMainMenu();
    }

    public void SetTargetSaveName()
    {
        MenuDataStatic.targetSaveName = fileNum;
    }

    public void NewGame()
    {
        SaveData saveData = new SaveData();
        saveData.playerX = -20f;
        saveData.playerY = 17.11f;
        saveData.scene = "Tutorial";
        saveData.playTime = 0f;
        //saveData.levelArray[0][0] = new LevelData(false, , "tutorial");
        SerializationManager.Save(MenuDataStatic.targetSaveName, saveData);
        //SceneManager.LoadScene("Tutorial"); 
        LoadGame();
    }
    public void LoadGame()
    {
        if (SerializationManager.Load(MenuDataStatic.targetSaveName) == null)
        {
            NewGame();
        }
        else
        {
            SaveData saveData = SerializationManager.Load(MenuDataStatic.targetSaveName);
            /*SaveDataStatic.playerX = saveData.playerX;
            SaveDataStatic.playerY = saveData.playerY;
            SaveDataStatic.scene = saveData.scene;
            SaveDataStatic.playTime = saveData.playTime;*/
            SaveDataStatic.saveData = saveData;
            SceneManager.LoadScene(SaveDataStatic.saveData.scene);

        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
