using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Google;

public class ProfileManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text userNameText;
    [SerializeField] private TMP_Text userEmailText;
    [SerializeField] private RawImage profileImage;

    [Header("Default Profile Image")]
    [SerializeField] private Texture defaultProfileImage;

    bool IsActived = false;
    public GameObject MainMenuPanel;
    public GameObject ProfilePanel;

    private void Start()
    {
        LoadProfile();
        ProfilePanel.SetActive(IsActived);  //False.
    }

    private void LoadProfile()
    {
        // Load saved data
        string userName = PlayerPrefs.GetString("UserName", "Player");
        string userEmail = PlayerPrefs.GetString("UserEmail", "");
        string photoUrl = PlayerPrefs.GetString("UserPhoto", "");

        // Display name and email
        userNameText.text = userName;
        userEmailText.text = userEmail;

        // Load profile image
        if (!string.IsNullOrEmpty(photoUrl))
        {
            StartCoroutine(LoadProfileImage(photoUrl));
        }
        else
        {
            profileImage.texture = defaultProfileImage;
        }
    }

    #region ProfileButtonLogic
    public void OpenProfilePage()
    {
        MainMenuPanel.SetActive(IsActived);  //False.
        ProfilePanel.SetActive(!IsActived);  //True.
    } 
    #endregion

    #region BackButtonLogic
    public void ProfilePageBackLogic()
    {
        ProfilePanel.SetActive(IsActived);  //False.
        MainMenuPanel.SetActive(!IsActived);  //True.
    } 
    #endregion

    #region ProfileImageLogic
    private IEnumerator LoadProfileImage(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (request.result != UnityWebRequest.Result.Success)
#else
            if (request.isNetworkError || request.isHttpError)
#endif
            {
                Debug.LogWarning("Failed to load profile image.");

                if (defaultProfileImage != null)
                    profileImage.texture = defaultProfileImage;

                yield break;
            }

            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            profileImage.texture = texture;
        }
    }
    #endregion

    #region SignOutLogic
    public void SignOut(string AuthSceneName)
    {
        FirebaseAuth.DefaultInstance.SignOut();
        GoogleSignIn.DefaultInstance.SignOut();

        //Delete the Saved Keys
        PlayerPrefs.DeleteKey("UserName");
        PlayerPrefs.DeleteKey("UserEmail");
        PlayerPrefs.DeleteKey("UserPhoto");
        PlayerPrefs.Save();

        //Load the Authentication Scene.
        SceneManager.LoadScene(AuthSceneName);
    }
    #endregion
}