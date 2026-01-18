using UnityEngine;
using HCaptcha;

public class TestHCaptcha : MonoBehaviour
{
    private GUIStyle buttonStyle;
    private GUIStyle statusStyle;
    private GUIStyle titleStyle;
    private bool stylesInitialized = false;

    private string currentStatusMessage = "Ready! Tap button to test hCaptcha";
    private Color currentStatusColor = Color.white;

    void Start()
    {
        // Подписываемся на события HCaptchaManager
        if (HCaptchaManager.Instance != null)
        {
            HCaptchaManager.Instance.OnSuccess.AddListener(OnSuccess);
            HCaptchaManager.Instance.OnFailure.AddListener(OnFailure);
            Debug.Log("[TEST] Subscribed to HCaptchaManager events");
        }
        else
        {
            Debug.LogError("[TEST] HCaptchaManager not found! Add HCaptchaManager prefab to scene.");
            UpdateStatus("ERROR: HCaptchaManager prefab missing!", Color.red);
        }
    }

    void InitializeStyles()
    {
        if (stylesInitialized) return;

        buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 32;
        buttonStyle.fontStyle = FontStyle.Bold;
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.hover.textColor = Color.cyan;
        buttonStyle.alignment = TextAnchor.MiddleCenter;

        statusStyle = new GUIStyle(GUI.skin.label);
        statusStyle.fontSize = 24;
        statusStyle.normal.textColor = Color.white;
        statusStyle.alignment = TextAnchor.UpperLeft;
        statusStyle.wordWrap = true;
        statusStyle.padding = new RectOffset(20, 20, 20, 20);

        titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 36;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.normal.textColor = Color.cyan;
        titleStyle.alignment = TextAnchor.MiddleCenter;

        stylesInitialized = true;
    }

    void OnGUI()
    {
        InitializeStyles();

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        GUI.Box(new Rect(0, 0, screenWidth, screenHeight), "", GUI.skin.box);

        GUI.Label(new Rect(0, 50, screenWidth, 60), "hCaptcha Unity Test", titleStyle);

        float buttonWidth = Mathf.Min(screenWidth * 0.8f, 600);
        float buttonHeight = 120;
        float buttonX = (screenWidth - buttonWidth) / 2;
        float buttonY = (screenHeight - buttonHeight) / 2 - 50;

        if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), "🔒 Test hCaptcha", buttonStyle))
        {
            if (HCaptchaManager.Instance != null)
            {
                UpdateStatus("⏳ Starting hCaptcha verification...", Color.yellow);
                HCaptchaManager.Instance.Verify();
            }
            else
            {
                UpdateStatus("ERROR: HCaptchaManager not found!", Color.red);
            }
        }

        float statusHeight = 200;
        GUI.Label(new Rect(0, screenHeight - statusHeight - 50, screenWidth, statusHeight),
                  currentStatusMessage, statusStyle);

        GUIStyle infoStyle = new GUIStyle(GUI.skin.label);
        infoStyle.fontSize = 18;
        infoStyle.normal.textColor = Color.gray;
        infoStyle.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(0, screenHeight - 30, screenWidth, 30),
                  "Built with hCaptcha SDK 4.4.0", infoStyle);
    }

    void OnSuccess(string token)
    {
        string shortToken = token.Length > 50 ? token.Substring(0, 50) + "..." : token;
        string message = $"✅ SUCCESS!\n\nToken received:\n{shortToken}\n\nFull token length: {token.Length} characters";
        UpdateStatus(message, Color.green);
        Debug.Log($"[TEST] hCaptcha Success! Full token: {token}");
    }

    void OnFailure(string error)
    {
        string message = $"❌ FAILED!\n\nError:\n{error}";
        UpdateStatus(message, Color.red);
        Debug.LogError($"[TEST] hCaptcha Failed: {error}");
    }

    void UpdateStatus(string message, Color color)
    {
        currentStatusMessage = message;
        currentStatusColor = color;
        if (statusStyle != null)
        {
            statusStyle.normal.textColor = color;
        }
    }
}