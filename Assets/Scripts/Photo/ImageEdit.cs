using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageEdit : MonoBehaviour
{
    private SetRectangle setRectangle;
    [SerializeField] private RawImage showImage; //表示する画像
    [SerializeField] private RawImage maskImage; //透明範囲を示すマスク画像
    [SerializeField] private RawImage handleRangeImage;
    [SerializeField] private Image rectImage; //矩形範囲を示す画像

    [SerializeField] private RawImage cardRangeImage; //矩形範囲を示す画像
    [SerializeField] private RawImage cardImage; //カード画像
    [SerializeField] private Text message;

    private RectTransform showImageRect;
    private float pixelWidthPerRectTransformWidth;
    private float pixelHeightPerRectTransformHeight;

    private int rectX;
    private int rectY;
    private int rectWidth;
    private int rectHeight;


    private void Start() 
    {
        setRectangle = GetComponent<SetRectangle>();

        handleRangeImage.gameObject.SetActive(false);
    }

    public void Init(Texture originalImageTexture)
    {
        //ロードした画像を取得
        showImage.texture = originalImageTexture;

        //画像の拡縮
        handleRangeImage.texture = originalImageTexture;
        handleRangeImage.gameObject.GetComponent<AspectRatioManager>().GetImage();
        //handleRangeの画像は不要であるためnullにする
        //handleRangeImage.texture = null;

        // //maskImageの位置を初期化
        maskImage.rectTransform.offsetMin = new Vector2(0f, 0f);
        maskImage.rectTransform.offsetMax = new Vector2(0f, 0f);

        //初期化処理
        setRectangle.Init();

        Texture2D shoImageTexture = showImage.texture as Texture2D;
        
        showImageRect = showImage.GetComponent<RectTransform>();
        showImageRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);

        pixelWidthPerRectTransformWidth = shoImageTexture.width / showImageRect.sizeDelta.x;
        pixelHeightPerRectTransformHeight = shoImageTexture.height / showImageRect.sizeDelta.y;

        StartCoroutine(ShowImageSize());
    }

    private IEnumerator ShowImageSize()
    {
        while(true)
        {
            rectX = (int)((rectImage.rectTransform.offsetMin.x + 3f) * pixelWidthPerRectTransformWidth);
            rectY = (int)((rectImage.rectTransform.offsetMax.y + 3f) * pixelHeightPerRectTransformHeight * (-1));
            rectWidth = (int)(((showImageRect.sizeDelta.x - 6f) - (rectImage.rectTransform.offsetMin.x + rectImage.rectTransform.offsetMax.x * (-1))) * pixelWidthPerRectTransformWidth);
            rectHeight = (int)(((showImageRect.sizeDelta.y - 6f) - (rectImage.rectTransform.offsetMin.y + rectImage.rectTransform.offsetMax.y * (-1))) * pixelHeightPerRectTransformHeight);

            // アスペクト比 横:縦 = 9:16 であれば、OKとする。ただし、誤差として30ピクセルまで許容する
            if(rectWidth - rectHeight * 9 / 16 < 10 && rectWidth - rectHeight * 9 / 16 > -10)
            {
                message.text = "カードを作成できます";
                var test = PhotoManager.Instance.CreateButton.GetComponent<EventTrigger>();
            }
            else if(rectWidth - rectHeight * 9 / 16 < -10)
            {
                message.text = "縦型に調整してください\n横を長くするか、縦を短くする必要があります";
            }
            else if(rectWidth - rectHeight * 9 / 16 > 10)
            {
                message.text = "縦型に調整してください\n縦を長くするか、横を短くする必要があります";
            }
            
            yield return null;
        }
    }

    public string CreateCardImage(string path)
    {
        StopCoroutine(ShowImageSize());

        // (rectX, rectY)から(rectWidth, rectangleHeight)の範囲の画像を切り取る
        Texture2D cardTexture = new Texture2D(rectWidth, rectHeight, TextureFormat.RGBA32, false);
        Texture2D showImageTexture = showImage.texture as Texture2D;
        Color[] pixels = showImageTexture.GetPixels(rectX, rectY, rectWidth, rectHeight);
        cardTexture.SetPixels(pixels);
        cardTexture.Apply();

        // 画像の拡縮
        cardRangeImage.texture = cardTexture;
        cardRangeImage.gameObject.GetComponent<AspectRatioManager>().GetImage();
        //handleRangeの画像は不要であるためnullにする
        cardRangeImage.texture = null;

        // 切り取った画像を表示
        cardImage.texture = cardTexture;

        return CreateCardImageToPNG(cardTexture, path);
    }

    private string CreateCardImageToPNG(Texture2D cardTexture, string path)
    {
        // PNGに変換して、保存
        byte[] bytes = cardTexture.EncodeToPNG();

        // 保存先は Application.persistentDataPath + 元画像名_card.png
        string cardPath = Path.Combine(Application.persistentDataPath, Path.GetFileNameWithoutExtension(path) + "_card.png");
        Debug.Log("cardPath: " + cardPath);

        File.WriteAllBytes(cardPath, bytes);
        return cardPath;
    }   
}
