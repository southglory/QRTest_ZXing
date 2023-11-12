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
    
    public RawImage rawImageDisplay;
    public GameObject QRCompletedPanel; // QR 코드 스캔 완료 패널
    public TextMeshProUGUI scannedText; // 스캔된 텍스트를 표시할 TextMeshProUGUI
    public Button openUrlButton; // URL 열기 버튼

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
                    Debug.Log(result.Text);

                    // QR 코드 인식 후 처리
                    HandleScannedQRCode(result.Text);

                    // 촬영 중단
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
    
    private void HandleScannedQRCode(string scannedData)
    {
        // 패널과 텍스트 업데이트
        QRCompletedPanel.SetActive(true);
        scannedText.text = scannedData;

        openUrlButton.onClick.RemoveAllListeners(); // 기존 리스너 제거
        
        // URL 확인 버튼 활성화 여부 결정
        if (Uri.IsWellFormedUriString(scannedData, UriKind.Absolute))
        {
            openUrlButton.gameObject.SetActive(true);
            openUrlButton.onClick.AddListener(() => OpenUrl(scannedData));
        }
        else
        {
            openUrlButton.gameObject.SetActive(false);
        }
    }

    private void OpenUrl(string url)
    {
        Application.OpenURL(url);
    }

    public void RestartScanning()
    {
        strBarcodeRead = null; // 스캔된 데이터 초기화
        StartScanning(); // 웹캠 촬영 재시작
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