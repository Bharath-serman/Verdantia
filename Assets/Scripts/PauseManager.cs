using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{

    [Header("Inputs")]
    public GameObject PausePanel;
    bool PanelStatus = false;
    private string[] SceneNames =
    {
        "Ocean_Scene",  //Index 0
        "Game_Scene",  // Index 1
    };

    void Start()
    {
        //Make sure the Pause Panel is hidden.
        PausePanel.SetActive(PanelStatus);  //False.
    }

    #region Pause and Resume Logic
    public void PauseGame()
    {
        //Toggle the Pause Panel.
        PausePanel.SetActive(!PanelStatus);  //True.
        Time.timeScale = 0f;  //Pause the Game.
    }

    public void ResumeGame()
    {
        //Resume the Game.
        Time.timeScale = 1f;
        //Toggle the Pause Panel.
        PausePanel.SetActive(PanelStatus);  //False.
    }
    #endregion

    #region MainMenu Button Logic
    public void MainSceneLoad(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
        //print("Scene Loaded");
    } 
    #endregion

    #region Scene_Switch Logic
    public void SwitchScene(int index)
    {
        switch (index)
        {
            case 0:
                //MainMenuLogic.Instance.StartCoroutine(MainMenuLogic.Instance.StartGame());
                SceneManager.LoadScene(SceneNames[0]);
                break;
            case 1:
                //MainMenuLogic.Instance.StartCoroutine(MainMenuLogic.Instance.StartGame());
                SceneManager.LoadScene(SceneNames[1]);
                break;
            default:
                print("Scene Index not Specified!");
                break;
        }
    } 
    #endregion


}
