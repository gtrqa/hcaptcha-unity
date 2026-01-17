# Android Library (AAR)

Android library module for hCaptcha Unity integration.

## 📦 Pre-built AAR

Don't want to build? Download pre-built AAR from [Releases](../../releases).

## 🔧 Building from Source

### Prerequisites

- **Android Studio** Arctic Fox (2020.3.1) or newer
- **JDK** 11 or newer
- **Gradle** 8.0+ (included via wrapper)
- **Unity classes.jar** from your Unity installation

### Step 1: Get Unity classes.jar

#### Windows:
```powershell
Copy-Item "C:\Program Files\Unity\Hub\Editor\6000.3.4f1\Editor\Data\PlaybackEngines\AndroidPlayer\Variations\mono\Release\classes.jar" "unithhcapt-lib\src\main\libs\"
```

> **Note:** Adjust path according to your Unity version.

### Step 2: Build AAR

#### Windows:
```powershell
.\gradlew.bat :unithhcapt-lib:assembleRelease
```

### Step 3: Get Output
```
unithhcapt-lib/build/outputs/aar/unithhcapt-lib-release.aar
```

## 📂 Project Structure
```
unithhcapt-lib/
├── build.gradle                    # Module configuration
├── proguard-rules.pro              # ProGuard rules
└── src/main/
    ├── java/com/gtrq/unithhcapt/
    │   └── HCaptchaWrapper.java    # Main integration code
    ├── AndroidManifest.xml         # Library manifest
    └── libs/
        ├── .gitkeep
        └── classes.jar             # (You provide this)
```

## 🔗 Dependencies

Automatically resolved via Gradle:

- **hCaptcha SDK:** 4.4.0 (from JitPack)
- **AndroidX Fragment:** 1.6.2
- **AndroidX Activity:** 1.8.2
- **AndroidX AppCompat:** 1.6.1

## 📝 License

MIT License - see [../LICENSE](../LICENSE)
