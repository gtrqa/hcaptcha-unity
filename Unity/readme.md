# hCaptcha Unity Package

Unity integration for hCaptcha on Android.

## Installation

**UPM:**
```
https://github.com/gtrqa/hcaptcha-unity.git?path=/Unity
```

**Or download** `.unitypackage` from [Releases](https://github.com/gtrqa/hcaptcha-unity/releases)

## Quick Start

1. Drag `Prefabs/HCaptchaManager` into scene
2. Set Site Key in Inspector (get from hcaptcha.com)
3. Call `HCaptchaManager.Instance.Verify()`

## Example

```csharp
using HCaptcha;

public class LoginManager : MonoBehaviour
{
    void Start()
    {
        var captcha = FindObjectOfType<HCaptchaManager>();
        captcha.OnSuccess.AddListener(token => {
            Debug.Log("✓ Verified! Token: " + token);
            // Send token to your server
        });
        captcha.OnFailure.AddListener(error => {
            Debug.LogError("✗ Failed: " + error);
        });
    }

    public void Login()
    {
        HCaptchaManager.Instance.Verify();
    }
}
```

## Requirements

- Unity 2022.3+
- Android platform
- Min SDK 22

## Links

- [GitHub](https://github.com/gtrqa/hcaptcha-unity)
- [Report Issues](https://github.com/gtrqa/hcaptcha-unity/issues)
- [hCaptcha Docs](https://docs.hcaptcha.com/)