using System;
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
        strBarcodeRead = null;
        camTexture = new WebCamTexture();
        camTexture.requestedHeight = Screen.height;
        camTexture.requestedWidth = Screen.width;

        if (camTexture != null)
        {
            camTexture.Play();
            rawImageDisplay.texture = camTexture;
            rawImageDisplay.material.mainTexture = camTexture;
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