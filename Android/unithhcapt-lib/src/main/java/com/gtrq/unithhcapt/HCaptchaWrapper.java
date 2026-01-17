package com.gtrq.unithhcapt;

import android.app.Activity;
import android.util.Log;
import androidx.fragment.app.FragmentActivity;
import com.hcaptcha.sdk.*;
import com.hcaptcha.sdk.tasks.*;

/**
 * All hCaptcha stuff runs on UI thread now
 */
public class HCaptchaWrapper {
    private static final String TAG = "HCaptchaWrapper";
    private static final String UNITY_OBJECT_NAME = "check";
    private static HCaptcha hCaptchaInstance = null;

    /**
     * Start invisible hCaptcha verification
     * Running on UI thread for WebView compatibility
     */
    public static void startInvisibleHCaptcha(final String siteKey) {
        Log.d(TAG, "startInvisibleHCaptcha called with siteKey: " + siteKey);

        try {
            // Get current activity via reflection
            Class<?> unityPlayerClass = Class.forName("com.unity3d.player.UnityPlayer");
            java.lang.reflect.Field currentActivityField = unityPlayerClass.getDeclaredField("currentActivity");
            currentActivityField.setAccessible(true);
            final Activity activity = (Activity) currentActivityField.get(null);

            if (activity == null) {
                Log.e(TAG, "UnityPlayer.currentActivity is null!");
                sendFailureToUnity("Activity is null");
                return;
            }

            Log.d(TAG, "Got activity: " + activity.getClass().getName());

            // Make sure it's a FragmentActivity
            if (!(activity instanceof FragmentActivity)) {
                Log.e(TAG, "Activity is not FragmentActivity! Actual: " + activity.getClass().getName());
                sendFailureToUnity("Activity must be FragmentActivity");
                return;
            }

            // Switch to UI thread - this is important!
            activity.runOnUiThread(new Runnable() {
                @Override
                public void run() {
                    try {
                        FragmentActivity fragmentActivity = (FragmentActivity) activity;

                        Log.d(TAG, "Running on UI thread, initializing hCaptcha...");

                        // Create or reuse hCaptcha client
                        if (hCaptchaInstance == null) {
                            hCaptchaInstance = HCaptcha.getClient(fragmentActivity);
                            Log.d(TAG, "HCaptcha client created on UI thread");
                        }

                        // Config for invisible captcha
                        HCaptchaConfig config = HCaptchaConfig.builder()
                                .siteKey(siteKey)
                                .size(HCaptchaSize.INVISIBLE)
                                .theme(HCaptchaTheme.LIGHT)
                                .loading(true)
                                .build();

                        Log.d(TAG, "Starting hCaptcha verification on UI thread...");

                        // Setup and start verification
                        hCaptchaInstance.setup(config)
                                .verifyWithHCaptcha()
                                .addOnSuccessListener(new OnSuccessListener<HCaptchaTokenResponse>() {
                                    @Override
                                    public void onSuccess(HCaptchaTokenResponse response) {
                                        String token = response.getTokenResult();
                                        Log.d(TAG, "hCaptcha success! Token: " + token);
                                        sendSuccessToUnity(token);
                                    }
                                })
                                .addOnFailureListener(new OnFailureListener() {
                                    @Override
                                    public void onFailure(HCaptchaException e) {
                                        Log.e(TAG, "hCaptcha failed: " + e.getMessage() +
                                                " (code: " + e.getStatusCode() + ")");
                                        sendFailureToUnity(e.getMessage() + " (" + e.getStatusCode() + ")");
                                    }
                                })
                                .addOnOpenListener(new OnOpenListener() {
                                    @Override
                                    public void onOpen() {
                                        Log.d(TAG, "hCaptcha dialog opened");
                                    }
                                });

                    } catch (Exception e) {
                        Log.e(TAG, "Exception during hCaptcha setup on UI thread: " + e.getMessage());
                        e.printStackTrace();
                        sendFailureToUnity("UI thread exception: " + e.getMessage());
                    }
                }
            });

        } catch (Exception e) {
            Log.e(TAG, "Exception getting activity: " + e.getMessage());
            e.printStackTrace();
            sendFailureToUnity("Activity exception: " + e.getMessage());
        }
    }

    /**
     * Reset hCaptcha state - also on UI thread
     */
    public static void resetHCaptcha() {
        if (hCaptchaInstance != null) {
            Log.d(TAG, "Resetting hCaptcha instance");

            try {
                Class<?> unityPlayerClass = Class.forName("com.unity3d.player.UnityPlayer");
                java.lang.reflect.Field currentActivityField = unityPlayerClass.getDeclaredField("currentActivity");
                currentActivityField.setAccessible(true);
                final Activity activity = (Activity) currentActivityField.get(null);

                if (activity != null) {
                    activity.runOnUiThread(new Runnable() {
                        @Override
                        public void run() {
                            if (hCaptchaInstance != null) {
                                hCaptchaInstance.reset();
                                Log.d(TAG, "hCaptcha reset completed on UI thread");
                            }
                        }
                    });
                }
            } catch (Exception e) {
                Log.e(TAG, "Exception during reset: " + e.getMessage());
            }
        }
    }

    /**
     * Full cleanup - also on UI thread
     */
    public static void destroyHCaptcha() {
        if (hCaptchaInstance != null) {
            Log.d(TAG, "Destroying hCaptcha instance");

            try {
                Class<?> unityPlayerClass = Class.forName("com.unity3d.player.UnityPlayer");
                java.lang.reflect.Field currentActivityField = unityPlayerClass.getDeclaredField("currentActivity");
                currentActivityField.setAccessible(true);
                final Activity activity = (Activity) currentActivityField.get(null);

                if (activity != null) {
                    activity.runOnUiThread(new Runnable() {
                        @Override
                        public void run() {
                            if (hCaptchaInstance != null) {
                                hCaptchaInstance.destroy();
                                hCaptchaInstance = null;
                                Log.d(TAG, "hCaptcha destroyed on UI thread");
                            }
                        }
                    });
                }
            } catch (Exception e) {
                Log.e(TAG, "Exception during destroy: " + e.getMessage());
            }
        }
    }

    /**
     * Send success token to Unity
     */
    private static void sendSuccessToUnity(String token) {
        try {
            Class<?> unityPlayerClass = Class.forName("com.unity3d.player.UnityPlayer");
            java.lang.reflect.Method sendMessageMethod = unityPlayerClass.getMethod(
                    "UnitySendMessage", String.class, String.class, String.class);
            sendMessageMethod.invoke(null, UNITY_OBJECT_NAME, "OnHCaptchaSuccess", token);
        } catch (Exception e) {
            Log.e(TAG, "Failed to send success to Unity: " + e.getMessage());
        }
    }

    /**
     * Send error to Unity
     */
    private static void sendFailureToUnity(String message) {
        try {
            Class<?> unityPlayerClass = Class.forName("com.unity3d.player.UnityPlayer");
            java.lang.reflect.Method sendMessageMethod = unityPlayerClass.getMethod(
                    "UnitySendMessage", String.class, String.class, String.class);
            sendMessageMethod.invoke(null, UNITY_OBJECT_NAME, "OnHCaptchaFailure", message);
        } catch (Exception e) {
            Log.e(TAG, "Failed to send failure to Unity: " + e.getMessage());
        }
    }
}