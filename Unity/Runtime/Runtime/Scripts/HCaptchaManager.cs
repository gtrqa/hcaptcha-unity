using UnityEngine;
using UnityEngine.Events;

namespace HCaptcha
{
    public class HCaptchaManager : MonoBehaviour
    {
        [Header("Configuration")]
        [Tooltip("Get your site key from hcaptcha.com")]
        public string siteKey = "";

        [Header("Events")]
        public UnityEvent<string> OnSuccess;
        public UnityEvent<string> OnFailure;

        [Header("Debug")]
        [SerializeField] private bool enableLogs = true;

        private HCaptchaUnityBridge bridge;
        private static HCaptchaManager instance;

        void Awake()
        {
            if (instance != null && instance != this)
            {
                if (enableLogs)
                    Debug.LogWarning("[HCaptcha] Multiple instances. Destroying duplicate.");
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
            SetupBridge();
        }

        void SetupBridge()
        {
            if (gameObject.name != "check")
            {
                if (enableLogs)
                    Debug.Log($"[HCaptcha] Renaming '{gameObject.name}' to 'check'");
                gameObject.name = "check";
            }

            bridge = GetComponent<HCaptchaUnityBridge>();
            if (bridge == null)
            {
                bridge = gameObject.AddComponent<HCaptchaUnityBridge>();
            }

            var field = typeof(HCaptchaUnityBridge).GetField("siteKey", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (field != null)
            {
                field.SetValue(bridge, siteKey);
            }

            bridge.OnSuccess += HandleSuccess;
            bridge.OnFailure += HandleFailure;

            if (enableLogs)
                Debug.Log("[HCaptcha] Manager initialized");
        }

        public void Verify()
        {
            if (string.IsNullOrEmpty(siteKey))
            {
                string error = "Site Key not configured! Get one from hcaptcha.com";
                Debug.LogError($"[HCaptcha] {error}");
                OnFailure?.Invoke(error);
                return;
            }

            if (bridge == null)
            {
                Debug.LogError("[HCaptcha] Bridge not initialized!");
                OnFailure?.Invoke("Bridge not initialized");
                return;
            }

            if (enableLogs)
                Debug.Log("[HCaptcha] Starting verification...");

            bridge.StartHCaptcha();
        }

        public void Reset()
        {
            if (bridge != null)
            {
                bridge.ResetHCaptcha();
                if (enableLogs)
                    Debug.Log("[HCaptcha] Reset completed");
            }
        }

        private void HandleSuccess(string token)
        {
            if (enableLogs)
            {
                string shortToken = token.Length > 50 ? token.Substring(0, 50) + "..." : token;
                Debug.Log($"[HCaptcha] ✓ Success! Token: {shortToken}");
            }

            OnSuccess?.Invoke(token);
        }

        private void HandleFailure(string error)
        {
            if (enableLogs)
                Debug.LogError($"[HCaptcha] ✗ Failed: {error}");

            OnFailure?.Invoke(error);
        }

        void OnDestroy()
        {
            if (bridge != null)
            {
                bridge.OnSuccess -= HandleSuccess;
                bridge.OnFailure -= HandleFailure;
            }

            if (instance == this)
                instance = null;
        }

        public static HCaptchaManager Instance => instance;

#if UNITY_EDITOR
        void OnValidate()
        {
            if (gameObject.name != "check")
            {
                gameObject.name = "check";
            }
        }
#endif
    }
}