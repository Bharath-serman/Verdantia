using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Extensions;
using Google;

public class FirebaseGoogleLogin : MonoBehaviour
{
    private FirebaseAuth auth;
    private GoogleSignInConfiguration configuration;

    [Header("Scene Settings")]
    [SerializeField] private string nextSceneName = "MainMenu";

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        configuration = new GoogleSignInConfiguration
        {
            WebClientId = "1054814683679-16j3dadot3m2vmis1np2j33h3eg3bqf0.apps.googleusercontent.com",
            RequestIdToken = true,
            RequestEmail = true
        };
    }

    public void SignIn()
    {
#if UNITY_ANDROID && !UNITY_EDITOR

        GoogleSignIn.Configuration = configuration;

        GoogleSignIn.DefaultInstance
            .SignIn()
            .ContinueWithOnMainThread(OnGoogleAuthenticated);

#else

        Debug.LogWarning("Google Sign-In only works on Android devices.");

#endif
    }

    private void OnGoogleAuthenticated(System.Threading.Tasks.Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            Debug.LogError("Google Sign-In Failed");

            foreach (var e in task.Exception.Flatten().InnerExceptions)
            {
                Debug.LogError(e);
            }

            return;
        }

        if (task.IsCanceled)
        {
            Debug.LogWarning("Google Sign-In Cancelled");
            return;
        }

        Credential credential = GoogleAuthProvider.GetCredential(
            task.Result.IdToken,
            null);

        auth.SignInWithCredentialAsync(credential)
            .ContinueWithOnMainThread(OnFirebaseAuthenticated);
    }

    private void OnFirebaseAuthenticated(System.Threading.Tasks.Task<FirebaseUser> task)
    {
        if (task.IsFaulted)
        {
            Debug.LogError("Firebase Authentication Failed");

            foreach (var e in task.Exception.Flatten().InnerExceptions)
            {
                Debug.LogError(e);
            }

            return;
        }

        if (task.IsCanceled)
        {
            Debug.LogWarning("Firebase Authentication Cancelled");
            return;
        }

        FirebaseUser user = task.Result;

        Debug.Log("Login Success");
        Debug.Log("Name: " + user.DisplayName);
        Debug.Log("Email: " + user.Email);
        Debug.Log("UID: " + user.UserId);

        PlayerPrefs.SetString("UserName", user.DisplayName ?? "");
        PlayerPrefs.SetString("UserEmail", user.Email ?? "");

        if (user.PhotoUrl != null)
            PlayerPrefs.SetString("UserPhoto", user.PhotoUrl.ToString());
        else
            PlayerPrefs.SetString("UserPhoto", "");

        PlayerPrefs.Save();

        SceneManager.LoadScene(nextSceneName);
    }
}