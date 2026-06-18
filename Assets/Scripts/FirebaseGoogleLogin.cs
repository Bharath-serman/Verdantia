using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;
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
            RequestIdToken = true
        };
    }

    public void SignIn()
    {
#if UNITY_ANDROID && !UNITY_EDITOR

        GoogleSignIn.Configuration = configuration;

        GoogleSignIn.DefaultInstance
            .SignIn()
            .ContinueWith(OnGoogleAuthenticated);

#else

        Debug.LogWarning("Google Sign-In only works on Android devices.");

#endif
    }

    private void OnGoogleAuthenticated(System.Threading.Tasks.Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            Debug.LogError("Google Sign-In Failed: " + task.Exception);
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
            .ContinueWith(OnFirebaseAuthenticated);
    }

    private void OnFirebaseAuthenticated(System.Threading.Tasks.Task<FirebaseUser> task)
    {
        if (task.IsFaulted)
        {
            Debug.LogError("Firebase Authentication Failed: " + task.Exception);
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

        // Save User Details
        PlayerPrefs.SetString("UserName", user.DisplayName ?? "");
        PlayerPrefs.SetString("UserEmail", user.Email ?? "");

        if (user.PhotoUrl != null)
        {
            PlayerPrefs.SetString("UserPhoto", user.PhotoUrl.ToString());
        }
        else
        {
            PlayerPrefs.SetString("UserPhoto", "");
        }

        PlayerPrefs.Save();

        // Load Next Scene
        SceneManager.LoadScene(nextSceneName);
    }

    //Anonymous sign in.
    public void AnonymousSignIn()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Anonymous Sign-In Failed: " + task.Exception);
                return;
            }
            if (task.IsCanceled)
            {
                Debug.LogWarning("Anonymous Sign-In Cancelled");
                return;
            }
            FirebaseUser user = task.Result.User;
            Debug.Log("Anonymous Login Success");
            Debug.Log("UID: " + user.UserId);
            // Save User Details
            PlayerPrefs.SetString("UserName", "Anonymous");
            PlayerPrefs.SetString("UserEmail", "");
            PlayerPrefs.SetString("UserPhoto", "");
            PlayerPrefs.Save();
            // Load Next Scene
            SceneManager.LoadScene(nextSceneName);
        });
    }
}