# hCaptcha Unity Package

Unity integration for hCaptcha on Android.

## Installation

**Download .unitypackage:**

[Latest Release](https://github.com/gtrqa/hcaptcha-unity/releases/latest)

Import: `Assets → Import Package → Custom Package`

## Quick Start

1. Drag `Prefabs/HCaptchaManager` into scene
2. Set Site Key in Inspector (get from [hcaptcha.com](https://hcaptcha.com))
3. Optional: Add `Prefabs/TestUI` for instant test button
4. Call `HCaptchaManager.Instance.Verify()`

## Example
```csharp
using HCaptcha;

public class LoginManager : MonoBehaviour
{
    void Start()
    {
        HCaptchaManager.Instance.OnSuccess.AddListener(token => {
            Debug.Log("✓ Verified! Token: " + token);
            // Send token to your server
        });
        
        HCaptchaManager.Instance.OnFailure.AddListener(error => {
            Debug.LogError("✗ Failed: " + error);
        });
    }

    public void Login()
    {
        HCaptchaManager.Instance.Verify();
    }
}
```

## Package Contents

- **Prefabs/HCaptchaManager** - Main captcha component
- **Prefabs/TestUI** - Ready-to-use test button
- **Samples/TestHCaptcha.cs** - OnGUI example script

## Requirements

- Unity 2022.3+
- Android platform
- Min SDK 22

## Links

- [GitHub Repository](https://github.com/gtrqa/hcaptcha-unity)
- [Report Issues](https://github.com/gtrqa/hcaptcha-unity/issues)
- [hCaptcha Documentation](https://docs.hcaptcha.com/)
