# 🔒 hCaptcha for Unity

[![Unity](https://img.shields.io/badge/Unity-2022.3%2B-black?logo=unity)](https://unity.com/)
[![License](https://img.shields.io/badge/License-MIT-green)](LICENSE)
[![Platform](https://img.shields.io/badge/Platform-Android-brightgreen)](https://developer.android.com/)
[![hCaptcha](https://img.shields.io/badge/hCaptcha-SDK%204.4.0-blue)](https://www.hcaptcha.com/)

Simple and powerful **hCaptcha** integration for **Unity Android** games and apps.

![hCaptcha Demo](https://via.placeholder.com/800x400?text=Demo+GIF+Coming+Soon)

## ✨ Features

- 🎮 **Drop-in Integration** - Import and use in minutes
- 👻 **Invisible Captcha** - Seamless user experience
- 🔧 **Automatic Setup** - Gradle configured automatically
- 📦 **Pre-built AAR** - No Android Studio required
- 🎯 **Simple API** - Easy C# interface
- 📚 **Examples Included** - OnGUI and Canvas UI samples
- 🆓 **MIT Licensed** - Free for commercial use
- 🌐 **Open Source** - Full source code available

## 🚀 Quick Start

### 1. Installation

**Option A: Asset Store (Coming Soon)**
> One-click install with automatic updates and support

**Option B: Manual Install**
1. Download the latest [Release](../../releases)
2. Import `.unitypackage` into your Unity project
3. Done! Gradle templates are configured automatically

### 2. Get hCaptcha Site Key

1. Sign up at [hCaptcha.com](https://www.hcaptcha.com/) (free)
2. Create a new site
3. Copy your **Site Key**

### 3. Setup in Unity

1. Drag `Prefabs/HCaptchaManager` into your scene
2. Select the prefab in Inspector
3. Paste your **Site Key**

### 4. Use in Code
```csharp
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    void Start()
    {
        var captcha = FindObjectOfType<HCaptchaManager>();
        captcha.OnSuccess += OnCaptchaSuccess;
        captcha.OnFailure += OnCaptchaFailure;
    }

    public void VerifyUser()
    {
        FindObjectOfType<HCaptchaManager>().Verify();
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
        ShowError("Please try again");
    }
}
```

## 📋 Requirements

| Requirement | Version |
|-------------|---------|
| **Unity** | 2022.3 LTS or newer |
| **Platform** | Android only |
| **Min SDK** | API 22 (Android 5.1) |
| **Target SDK** | API 34 (Android 14) |

## 📁 Project Structure
```
hcaptcha-unity/
├── Android/          # Android library source (AAR)
└── Unity/            # Unity package (coming soon)
```

## 🔧 Building from Source

Want to customize the Android library?

See [Android/README.md](Android/README.md) for build instructions.

## 📖 Documentation

- [Quick Start Guide](docs/QUICKSTART.md) _(coming soon)_
- [API Reference](docs/API.md) _(coming soon)_
- [Troubleshooting](docs/TROUBLESHOOTING.md) _(coming soon)_

## 🤝 Contributing

Contributions are welcome!

1. Fork the repo
2. Create a feature branch (`git checkout -b feature/amazing`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing`)
5. Open a Pull Request

## 📝 License

MIT License - see [LICENSE](LICENSE) for details.

This project uses [hCaptcha Android SDK](https://github.com/hCaptcha/hcaptcha-android-sdk) which is also MIT licensed.

## 🙏 Credits

- [hCaptcha](https://www.hcaptcha.com/) - Bot protection service
- [hCaptcha Android SDK](https://github.com/hCaptcha/hcaptcha-android-sdk) - Native Android SDK

## 📧 Support

- 🐛 [Report a Bug](../../issues/new?labels=bug)
- 💡 [Request a Feature](../../issues/new?labels=enhancement)
- 💬 [Ask a Question](../../discussions)

---

⭐ **Star this repo** if you find it useful!

Made with ❤️ for Unity developers
