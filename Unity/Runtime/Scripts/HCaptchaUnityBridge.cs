using UnityEngine;
using System;

/// <summary>
/// Bridge between Unity and Android hCaptcha SDK
/// Attach this script to a GameObject named "check"
/// </summary>
public class HCaptchaUnityBridge : MonoBehaviour
{
    [Header("hCaptcha Settings")]
    [SerializeField]
    private string siteKey = ""; // Replace with your key

    // Events (subscribe to these for results)
    public event Action<string> OnSuccess;
    public event Action<string> OnFailure;

    private const string ANDROID_CLASS = "com.gtrq.unithhcapt.HCaptchaWrapper";

    private void Awake()
    {
        // Make sure GameObject is named "check" for UnitySendMessage to work
        if (gameObject.name != "check")
        {
            Debug.LogWarning($"GameObject should be named 'check' but is named '{gameObject.name}'. Renaming...");
            gameObject.name = "check";
        }
    }

    /// <summary>
    /// Start hCaptcha verification
    /// </summary>
    public void StartHCaptcha()
    {
        Debug.Log("[HCaptcha] Starting verification...");

#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (AndroidJavaClass hCaptchaClass = new AndroidJavaClass(ANDROID_CLASS))
            {
                Debug.Log("[HCaptcha] Calling startInvisibleHCaptcha with siteKey: " + siteKey);
                hCaptchaClass.CallStatic("startInvisibleHCaptcha", siteKey);
                Debug.Log("[HCaptcha] Method called successfully");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("[HCaptcha] Failed to invoke hCaptcha: " + ex.Message);
            Debug.LogError("[HCaptcha] Stack trace: " + ex.StackTrace);
            OnFailure?.Invoke("Unity exception: " + ex.Message);
        }
#else
        Debug.LogWarning("[HCaptcha] hCaptcha is only supported on Android devices.");
        OnFailure?.Invoke("Platform not supported");
#endif
    }

    /// <summary>
    /// Reset hCaptcha state (called from Android)
    /// </summary>
    public void ResetHCaptcha()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (AndroidJavaClass hCaptchaClass = new AndroidJavaClass(ANDROID_CLASS))
            {
                hCaptchaClass.CallStatic("resetHCaptcha");
                Debug.Log("[HCaptcha] Reset called");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("[HCaptcha] Failed to reset: " + ex.Message);
        }
#endif
    }

    /// <summary>
    /// Callback from Android on successful verification
    /// NOTE: Method must be public for UnitySendMessage
    /// </summary>
    public void OnHCaptchaSuccess(string token)
    {
        Debug.Log("[HCaptcha] ✓ Success! Received token: " + token);
        OnSuccess?.Invoke(token);

        // Add your token handling logic here
        // e.g. send to server for validation
    }

    /// <summary>
    /// Callback from Android on error
    /// NOTE: Method must be public for UnitySendMessage
    /// </summary>
    public void OnHCaptchaFailure(string message)
    {
        Debug.LogError("[HCaptcha] ✗ Failure: " + message);
        OnFailure?.Invoke(message);

        // Add your error handling logic here
        // e.g. show UI message to user
    }

    private void OnDestroy()
    {
        // Cleanup resources on destroy
        ResetHCaptcha();
    }

    /// <summary>
    /// Helper method to test Android connection
    /// </summary>
    public void TestConnection()
    {
        Debug.Log("[HCaptcha] Testing Android connection...");

#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                Debug.Log("[HCaptcha] ✓ Current Activity: " + activity.Call<string>("toString"));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("[HCaptcha] ✗ Connection test failed: " + ex.Message);
        }
#else
        Debug.LogWarning("[HCaptcha] Test only works on Android device");
#endif
    }
}