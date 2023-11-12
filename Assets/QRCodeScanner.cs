using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ZXing;

public class QRCodeScanner : MonoBehaviour
{
    public RawImage rawImageDisplay; // RawImage UI 요소
    public TextMeshProUGUI tmpText; // TextMeshPro UI 요소
    private WebCamTexture camTexture;

    static string strBarcodeRead;

    void Start()
    {
        // WebCamTexture 설정
        camTexture = new WebCamTexture();
        camTexture.Play();

        // RawImage에 WebCamTexture 설정
        rawImageDisplay.texture = camTexture;

        // 비율에 맞게 RawImage의 RectTransform 크기 조정
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
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            var result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);

            if (result != null && strBarcodeRead != result.Text)
            {
                strBarcodeRead = result.Text;
                tmpText.text = strBarcodeRead; // TextMeshPro 텍스트 업데이트
                Debug.Log(result.Text);
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex.Message);
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