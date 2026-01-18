# hCaptcha Unity Package

Unity integration for hCaptcha on Android.

<p align="left">
  <img src="../demo.gif" alt="hCaptcha Demo" width="15%"/>
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
3. **Replace Unity's generated templates with package templates:**
   - Copy `Assets/HCaptcha/Runtime/Plugins/Android/mainTemplate.gradle` → `Assets/Plugins/Android/mainTemplate.gradle`
   - Copy `Assets/HCaptcha/Runtime/Plugins/Android/settingsTemplate.gradle` → `Assets/Plugins/Android/settingsTemplate.gradle`
   - Overwrite when prompted

> ⚡ **Without these exact templates, the build will fail!** Unity's auto-generated templates don't include hCaptcha dependencies.

## Unity Version Compatibility

### Unity 6.x (2023.3+) ✅
Works out of the box with included AAR (don't forget to replace templates above!).

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
- **Plugins/Android/mainTemplate.gradle** - Pre-configured dependencies (must replace Unity's!)
- **Plugins/Android/settingsTemplate.gradle** - JitPack repository (must replace Unity's!)
- **Plugins/Android/unithhcapt-lib.aar** - Pre-compiled Android library

## Requirements

- Unity 2022.3 LTS or newer
- Android platform
- Min SDK 22 (Android 5.1)
- Target SDK 34 (Android 14)
- **Custom Gradle Templates enabled AND replaced** (see setup above)

## Testing

### In Editor
Press Play → "Platform not supported" (expected - Android only)

### On Device
1. Build APK with Gradle templates enabled and replaced
2. Install on Android device
3. Press verify button
4. Complete captcha
5. Check logcat for success token

## Troubleshooting

**Gradle build fails with "keepUnitySymbols.gradle does not exist"**
- Enable Custom Gradle Templates in Player Settings
- Replace Unity's templates with package templates (see setup step 3)

**"D8: NullPointerException" or Java class version errors**
- Unity 2022.3: Rebuild AAR with Java 11 (see compatibility section)
- Unity 6+: Make sure you replaced the gradle templates

**"compileSdkVersion is not specified"**
- You must replace Unity's generated templates with package templates (setup step 3)
- Unity's auto-generated templates don't have the required configuration

**"Platform not supported" on device**
- Check that AAR is in `Assets/Plugins/Android/`
- Verify Gradle templates were replaced correctly
- Re-import package with "Include dependencies" checked

## Links

- [GitHub Repository](https://github.com/gtrqa/hcaptcha-unity)
- [Report Issues](https://github.com/gtrqa/hcaptcha-unity/issues)
- [hCaptcha Documentation](https://docs.hcaptcha.com/)

## License

MIT License - See repository for details
