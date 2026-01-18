# 🔒 hCaptcha for Unity

[![Unity](https://img.shields.io/badge/Unity-2023.3%2B-black?logo=unity)](https://unity.com/)
[![License](https://img.shields.io/badge/License-MIT-green)](LICENSE)
[![Platform](https://img.shields.io/badge/Platform-Android-brightgreen)](https://developer.android.com/)
[![hCaptcha](https://img.shields.io/badge/hCaptcha-SDK%204.4.0-blue)](https://www.hcaptcha.com/)

Simple and powerful **hCaptcha** integration for **Unity Android** games and apps.

![hCaptcha Demo](docs/demo.gif)

## ✨ Features

- 🎮 **Drop-in Integration** - Import and use in minutes
- 👻 **Invisible Captcha** - Seamless user experience
- 🔧 **Automatic Setup** - Gradle configured automatically
- 📦 **Pre-built AAR** - No Android Studio required (Unity 6+)
- 🎯 **Simple API** - Easy C# interface
- 🎨 **Two UI Examples** - OnGUI test scene + Canvas-ready prefab
- 🆓 **MIT Licensed** - Free for commercial use
- 🌐 **Open Source** - Full source code available

## 🚀 Quick Start

### 1. Installation

Download the latest [HCaptcha-Unity.unitypackage](../../releases/latest) and import into Unity:
```
Assets → Import Package → Custom Package → Select downloaded file
```

### 2. Get hCaptcha Site Key

1. Sign up at [hCaptcha.com](https://www.hcaptcha.com/) (free)
2. Create a new site
3. Copy your **Site Key**

### 3. Setup in Unity

**⚠️ IMPORTANT: Custom Gradle Templates Required!**

After importing, Unity **must** use the included Gradle templates:

1. `Edit → Project Settings → Player → Publishing Settings → Build`
2. **Enable BOTH:**
   - ✓ Custom Main Gradle Template
   - ✓ Custom Gradle Settings Template

> **Without these, the build will fail!** These templates configure JitPack repository and hCaptcha dependencies.

**Then add to scene:**

1. Drag `Prefabs/HCaptchaManager` into your scene
2. Select the prefab in Inspector
3. Paste your **Site Key**
4. (Optional) Drag `Prefabs/TestUI` for instant test button

### 4. Use in Code
```csharp
using UnityEngine;
using HCaptcha;

public class LoginManager : MonoBehaviour
{
    void Start()
    {
        HCaptchaManager.Instance.OnSuccess.AddListener(OnCaptchaSuccess);
        HCaptchaManager.Instance.OnFailure.AddListener(OnCaptchaFailure);
    }

    public void OnLoginButtonClick()
    {
        HCaptchaManager.Instance.Verify();
    }

    void OnCaptchaSuccess(string token)
    {
        Debug.Log("✓ Captcha verified! Token: " + token);
        // TODO: Send token to your server for validation
        AllowLogin();
    }

    void OnCaptchaFailure(string error)
    {
        Debug.LogError("✗ Captcha failed: " + error);
        ShowErrorMessage("Verification failed. Please try again.");
    }
}
```

**Or connect directly in Unity UI:**
1. Select your Button
2. OnClick() → Drag `HCaptchaManager` prefab
3. Select function: `HCaptchaManager → Verify()`

## 📦 What's Included
```
HCaptcha/
├── Runtime/
│   ├── Prefabs/
│   │   ├── HCaptchaManager.prefab    # Main captcha handler
│   │   └── TestUI.prefab              # Ready-to-use test button
│   ├── Scripts/
│   │   ├── HCaptchaManager.cs         # High-level API
│   │   └── HCaptchaUnityBridge.cs     # Android bridge
│   └── Plugins/Android/
│       ├── unithhcapt-lib.aar         # Pre-compiled library
│       ├── mainTemplate.gradle        # hCaptcha dependencies
│       └── settingsTemplate.gradle    # JitPack repository
├── Editor/
│   └── HCaptchaSetup.cs               # First-time setup helper
└── Samples/
    └── TestHCaptcha.cs                # Example OnGUI test scene
```

## 📋 Requirements

| Requirement | Version |
|-------------|---------|
| **Unity** | 2023.3 LTS or newer |
| **Platform** | Android only |
| **Min SDK** | API 22 (Android 5.1) |
| **Target SDK** | API 34 (Android 14) |
| **Gradle Templates** | **REQUIRED** (Custom Main + Settings) |

## ⚠️ Unity Version Compatibility

### Unity 6.x (2023.3+) - ✅ Fully Supported
- Pre-built AAR works out of the box
- Enable Custom Gradle Templates and build

### Unity 2022.3 LTS - ⚠️ Requires Rebuild
The included AAR is compiled with Java 17. Unity 2022.3 uses Java 11, causing build errors.

**Solution:**
1. Clone the [Android library source](Android/)
2. In Android Studio, set Java 11:
```gradle
   compileOptions {
       sourceCompatibility JavaVersion.VERSION_11
       targetCompatibility JavaVersion.VERSION_11
   }
   kotlinOptions {
       jvmTarget = "11"
   }
```
3. Rebuild AAR and replace in `Plugins/Android/`

## 🧪 Testing

### In Unity Editor
Press Play → You'll see "Platform not supported" (expected - hCaptcha works only on Android)

### On Android Device
1. Build APK: `File → Build Settings → Android → Build`
2. Install on device
3. Run app and press verify button
4. Complete captcha
5. Check logcat for success token

## 🔧 Building from Source

Want to customize the Android library?

See [Android/README.md](Android/README.md) for build instructions.

## 🐛 Troubleshooting

**"Platform not supported"**
- Normal in Unity Editor - hCaptcha only works on Android devices

**"Site Key not configured"**
- Set your site key in HCaptchaManager Inspector

**Gradle build fails with "keepUnitySymbols.gradle does not exist"**
- Enable Custom Gradle Templates in Player Settings (see setup above)

**"D8: java.lang.NullPointerException" or class version errors**
- Unity 2022.3: AAR needs Java 11 rebuild (see compatibility section above)
- Unity 6+: Should work without issues

**"compileSdkVersion is not specified"**
- Ensure Custom Main Gradle Template is enabled
- Package auto-configures it - if overridden, re-import package

## 📖 API Reference

### HCaptchaManager

**Methods:**
- `Verify()` - Start captcha verification
- `Reset()` - Reset captcha state

**Events:**
- `OnSuccess(string token)` - Fired when user passes captcha
- `OnFailure(string error)` - Fired on error or user cancellation

**Properties:**
- `Instance` - Singleton instance (auto-created from prefab)

## 🤝 Contributing

Contributions welcome!

1. Fork the repo
2. Create feature branch: `git checkout -b feature/amazing`
3. Commit changes: `git commit -m 'Add amazing feature'`
4. Push to branch: `git push origin feature/amazing`
5. Open Pull Request

## 📝 License

MIT License - see [LICENSE](LICENSE)

This project uses [hCaptcha Android SDK](https://github.com/hCaptcha/hcaptcha-android-sdk) (also MIT licensed).

## 🙏 Credits

- [hCaptcha](https://www.hcaptcha.com/) - Bot protection service
- [hCaptcha Android SDK](https://github.com/hCaptcha/hcaptcha-android-sdk) - Native Android SDK

## 📧 Support

- 🐛 [Report Bug](../../issues/new?labels=bug)
- 💡 [Request Feature](../../issues/new?labels=enhancement)
- 💬 [Discussions](../../discussions)

---

⭐ **Star this repo** if you find it useful!

Made with ❤️ for Unity developers
