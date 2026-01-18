# hCaptcha Unity Package

Unity integration for hCaptcha on Android.

<p align="center">
  <img src="demo.gif" alt="hCaptcha Demo" width="30%"/>
</p>

## Installation

**Download .unitypackage:**

[Latest Release](https://github.com/gtrqa/hcaptcha-unity/releases/latest)

Import: `Assets → Import Package → Custom Package`

## ⚠️ IMPORTANT: Gradle Setup (Required!)

After importing, Unity **must** use the included Gradle templates:

1. `Edit → Project Settings → Player → Publishing Settings → Build`
2. **Enable BOTH:**
   - ✓ Custom Main Gradle Template
   - ✓ Custom Gradle Settings Template

**The package will auto-configure these templates** with JitPack and hCaptcha dependencies.

> ⚡ **Without these, the build will fail!** The templates are required for AAR resolution.

## Unity Version Compatibility

### Unity 6.x (2023.3+) ✅
Works out of the box with included AAR.

### Unity 2022.3 LTS ⚠️
Included AAR uses Java 17. Unity 2022.3 uses Java 11.

**You'll need to rebuild the AAR:**
1. Clone [Android source](https://github.com/gtrqa/hcaptcha-unity/tree/main/Android)
2. Set Java 11 in `build.gradle`:
```gradle
   compileOptions {
       sourceCompatibility JavaVersion.VERSION_11
       targetCompatibility JavaVersion.VERSION_11
   }
```
3. Rebuild and replace AAR in `Plugins/Android/`

## Quick Start

### 1. Setup Gradle (see above ⬆️)

### 2. Add to Scene

- Drag `Prefabs/HCaptchaManager` into scene
- Set **Site Key** in Inspector ([get from hcaptcha.com](https://hcaptcha.com))
- (Optional) Add `Prefabs/TestUI` for instant test button

### 3. Use in Code
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
- **Plugins/Android/mainTemplate.gradle** - Auto-configured dependencies
- **Plugins/Android/settingsTemplate.gradle** - JitPack repository
- **Plugins/Android/unithhcapt-lib.aar** - Pre-compiled Android library

## Requirements

- Unity 2022.3 LTS or newer
- Android platform
- Min SDK 22 (Android 5.1)
- Target SDK 34 (Android 14)
- **Custom Gradle Templates enabled** (see setup above)

## Testing

### In Editor
Press Play → "Platform not supported" (expected - Android only)

### On Device
1. Build APK with Gradle templates enabled
2. Install on Android device
3. Press verify button
4. Complete captcha
5. Check logcat for success token

## Troubleshooting

**Gradle build fails with "keepUnitySymbols.gradle does not exist"**
- Enable Custom Gradle Templates in Player Settings (see setup above)

**"D8: NullPointerException" or Java class version errors**
- Unity 2022.3: Rebuild AAR with Java 11 (see compatibility section)
- Unity 6+: Should work without issues

**"compileSdkVersion is not specified"**
- Ensure Custom Main Gradle Template is enabled
- Package auto-configures it - if overridden, re-import package

**"Platform not supported" on device**
- Check that AAR is in `Assets/Plugins/Android/`
- Verify Gradle templates are enabled
- Re-import package with "Include dependencies" checked

## Links

- [GitHub Repository](https://github.com/gtrqa/hcaptcha-unity)
- [Report Issues](https://github.com/gtrqa/hcaptcha-unity/issues)
- [hCaptcha Documentation](https://docs.hcaptcha.com/)

## License

MIT License - See repository for details
