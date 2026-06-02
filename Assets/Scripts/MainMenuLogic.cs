using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuLogic : MonoBehaviour
{
    #region Inputs
    [Header("Inputs")]
    [SerializeField] private Animator Fade_BG;
    [SerializeField] private Animator Load_Fade;
    [SerializeField] private AudioSource Button_Click_Sound;
    [SerializeField] private AudioSource MainMenuTheme;

    private string[] TriggerNames = {
        "Fade",
        "Load"
    };

    string SceneName = "Game_Scene";

    float DelayTime = 1.5f;
    float AudioFadeDuration = 1.5f;

    bool Status = false;
    [SerializeField] private GameObject LoadText;
    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject AboutPanel;

    #endregion

    #region StartGameLogic
    IEnumerator StartGame()
    {
        //Play the Button Click Sound.
        Button_Click_Sound.Play();

        //Trigger the Fade Animation.
        Fade_BG.SetTrigger(TriggerNames[0]);

        //Wait for Some time.
        yield return new WaitForSeconds(DelayTime);

        //Show the Load text.
        LoadText.SetActive(!Status);  //True.

        //Trigger the Load Fade Animation.
        Load_Fade.SetTrigger(TriggerNames[1]);

        //Wait for some time.
        yield return new WaitForSeconds(DelayTime + 1.5f);  //Total -> 3 seconds.

        //Load the Next Scene.
        SceneManager.LoadScene(SceneName);

        yield return null;
    }

    #endregion

    void Start()
    {
        //Hide the Load Text at start.
        LoadText.SetActive(Status);  //False.

        //Hide the About Panel at start.
        AboutPanel.SetActive(Status);  //False.
    }

    public void BeginGame()
    {
        StartCoroutine(FadeAudioVolume());
        StartCoroutine(StartGame());
    }

    #region About Button Logic
    public void AboutLogic()
    {
        MainPanel.SetActive(Status);  //False.
        AboutPanel.SetActive(!Status);  //True.
    }
    #endregion

    #region About Back Button Logic
    public void AboutBackLogic()
    {
        MainPanel.SetActive(!Status);  //True.
        AboutPanel.SetActive(Status);  //False.
    }
    #endregion

    #region Quit Button Logic
    public void ExitButtonLogic()
    {
        //Quit the Application.
        Application.Quit();
    }
    #endregion

    #region AudioFadeLogic
    IEnumerator FadeAudioVolume()
    {
        //Get the Actual Volume of the Audio Source.
        float ActualVolume = MainMenuTheme.volume;

        while (ActualVolume > 0f)
        {
            MainMenuTheme.volume -= Time.deltaTime / AudioFadeDuration;
            yield return null;
        }

        MainMenuTheme.Stop();
        MainMenuTheme.volume = ActualVolume;

    } 
    #endregion

}
