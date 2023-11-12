using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ZXing;

public class QRCodeScanner : MonoBehaviour
{
    private static bool _isAppPaused = false;
    private static bool _isAppQuit = false;
    
    public RawImage rawImageDisplay; // RawImage UI 요소
    public TextMeshProUGUI tmpText; // TextMeshPro UI 요소
    private WebCamTexture camTexture;

    static string strBarcodeRead;

    void Start()
    {
        // 웹캠 촬영 설정은 별도의 메서드로 분리
        SetupWebcam();
    }

    public void StartScanning() 
    {
        // 웹캠 촬영 시작
        if (camTexture != null)
        {
            camTexture.Play();
        }
    }

    public void StopScanning() 
    {
        print("StopScanning");
        // 웹캠 촬영 중단
        if (camTexture != null && camTexture.isPlaying)
        {
            camTexture.Stop();
        }
    }
    
    private void SetupWebcam()
    {
        camTexture = new WebCamTexture();
        camTexture.requestedFPS = 30; // 30 FPS 설정
        rawImageDisplay.texture = camTexture;
        StartCoroutine(AdjustRawImageSize());
    }


    private IEnumerator AdjustRawImageSize()
    {
        yield return new WaitUntil(() => camTexture.width > 100);

        float screenAspectRatio = (float)Screen.width / Screen.height;
        float camAspectRatio = (float)camTexture.width / camTexture.height;

        rawImageDisplay.rectTransform.localEulerAngles = new Vector3(0, 0, -camTexture.videoRotationAngle);

        if (camTexture.videoVerticallyMirrored)
        {
            rawImageDisplay.rectTransform.localScale = new Vector3(1, -1, 1);
        }

        if (screenAspectRatio > camAspectRatio)
        {
            float width = Screen.height * camAspectRatio;
            rawImageDisplay.rectTransform.sizeDelta = new Vector2(width, Screen.height);
        }
        else
        {
            float height = Screen.width / camAspectRatio;
            rawImageDisplay.rectTransform.sizeDelta = new Vector2(Screen.width, height);
        }
    }

    
    void Update()
    {
        // WebCamTexture가 실행 중인지 확인
        if (camTexture != null && camTexture.isPlaying)
        {
            try
            {
                IBarcodeReader barcodeReader = new BarcodeReader();
                var result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);

                if (result != null && strBarcodeRead != result.Text)
                {
                    strBarcodeRead = result.Text;
                    tmpText.text = strBarcodeRead; // TextMeshPro 텍스트 업데이트
                    Debug.Log(result.Text);

                    // QR 코드 인식 후 촬영 중단 (필요한 경우)
                    StopScanning();
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex.Message);
            }
        }
        
        // Check if the "Escape" key (Android "back" button) is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Handle the "back" button press here
            // You can perform any action you want, such as navigating back or showing a dialog.
            StopScanning();
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        _isAppPaused = !hasFocus;
        // Debug.Log("Application focus status: " + hasFocus);
            
        if (_isAppPaused)
        {
            // Do something when the app is paused
            StopScanning();
        }
    }
    
    
    void OnApplicationPause(bool pauseStatus)
    {
        _isAppPaused = pauseStatus;
        // Debug.Log("Application pause status: " + pauseStatus);
            
        if (_isAppPaused)
        {
            // Do something when the app is paused
            StopScanning();
        }
    }
    
    void OnApplicationQuit()
    {
        _isAppQuit = true;
        // Debug.Log("Application ending after " + Time.time + " seconds");
        if (_isAppQuit)
        {
            // Do something when the app is quit
            StopScanning();
        }
    }
    
    void OnDestroy()
    {
        if (camTexture != null)
        {
            camTexture.Stop();
        }
    }
}