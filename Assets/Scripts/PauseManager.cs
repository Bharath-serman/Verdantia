using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{

    #region Inputs
    [Header("Inputs")]
    public GameObject PausePanel;
    bool PanelStatus = false;
    [SerializeField] private string[] SceneNames;

    private float OriginalTime = 1f;
    #endregion

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
        Time.timeScale = OriginalTime;  //0f.
        //print("Scene Loaded");
    }
    #endregion

    #region Scene_Switch Logic
    public void SwitchScene(int index)
    {
        if(index < 0 || index >= SceneNames.Length)
        {
            Debug.LogError("Invalid Scene Index : {index}");
            return;
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneNames[index]);
    } 
    #endregion


}
