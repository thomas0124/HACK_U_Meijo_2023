using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PhotoManager : MonoBehaviour
{
    public static PhotoManager Instance { get; private set; }
    [SerializeField] private ImageEdit imageEdit;
    [SerializeField] private RawImage handleRangeImage;
    [SerializeField] private RawImage originalImage;
    [SerializeField] private RectTransform showImageUnMask;
    [SerializeField] private RawImage rectangleImage;
    [SerializeField] private RectTransform unMaskRect;
    [SerializeField] private RectTransform rectImageRect;
    [SerializeField] private RectTransform rectImageUnMaskRect;
    [SerializeField] private List<RectTransform> rectTransform;
    [SerializeField] private List<RectTransform> rectTransform_mask;

    private Vector2 RectWH = new Vector2(0,0);
    private Vector2 anchorPosTopLeft;
    private Vector2 anchorPosTopRight;
    private Vector2 anchorPosBottomRight;
    private Vector2 anchorPosBottomLeft;


    [Header("Button And Text")]
    [SerializeField] private GameObject imageChoiceButton;
    [SerializeField] private Text messageText;
    [SerializeField] private GameObject createCard;
    [SerializeField] private GameObject createButton;
    [SerializeField] private GameObject clockwiseButton;
    [SerializeField] private GameObject counterClockwiseButton;
    [SerializeField] private GameObject redoButton;

    public GameObject CreateButton => createButton;
    public GameObject RedoButton => redoButton;

    private Texture2D texture;
    private string imagePath;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void OnClickPickButton()
    {
        // 別のメディア選択操作がすでに進行中の場合
        if(NativeGallery.IsMediaPickerBusy())
            return;

        // 画像の読み込み
        PickImage();
    }

    public void OnClickCreateButton()
    {
        StartCoroutine(CardCreate());
    }

    private IEnumerator CardCreate()
    {
        // 指定した範囲でカードの画像を作成
        string cardPath = imageEdit.CreateCardImage(imagePath);
        // 作成した画像のステータスを取得
        WebRequestManager.Instance.GetStatus(cardPath);
        yield return new WaitUntil(() => WebRequestManager.Instance.isRequesting == false);
        if(WebRequestManager.Instance.isError)
        {
            Debug.Log("エラーもしくは広告ではない");
            ReStart();
            yield break;
        }
        // ステータス等を表示
        ShowStatusManager.Instance.ShowLevel();
        StartCoroutine(ShowStatusManager.Instance.ShowStatuses());
    }

    // 画像の読み込み
    private void PickImage()
    {
        // 画像の読み込み
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            imagePath = path;

            if (path != null)
            {  
                // 画像パスからTexture2Dを生成
                texture = NativeGallery.LoadImageAtPath(path, 512);
                if( texture == null )
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                texture = createReadabeTexture2D(texture);

                handleRangeImage.gameObject.SetActive(true);
                createButton.SetActive(true);
                redoButton.SetActive(true);
                clockwiseButton.SetActive(true);
                counterClockwiseButton.SetActive(true);

                imageEdit.Init(texture);
            }
        } );

        Debug.Log( "Permission result: " + permission );

    }

    private Texture2D createReadabeTexture2D(Texture2D texture2d)
    {
        RenderTexture renderTexture = RenderTexture.GetTemporary(
                    texture2d.width,
                    texture2d.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(texture2d, renderTexture);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTexture;
        Texture2D readableTexture2D = new Texture2D(texture2d.width, texture2d.height);
        readableTexture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        readableTexture2D.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTexture);

        renderTexture = null;
        Destroy(renderTexture);

        previous = null;
        Destroy(previous);

        texture2d = null;
        Destroy(texture2d);

        return readableTexture2D;
    }

    public void TextureRotator(bool Clockwise)
    {

        //Debug.Log("TextureRotate");
        RotatePosition(texture, Clockwise);
        /*        
        texture = RotateTexture(texture, Clockwise);
        imageEdit.Rotate(texture);*/

    }

    private void RotatePosition(Texture2D rotatedTexture,bool clockwise)
    {
        Texture2D originalTexture = originalImage.texture as Texture2D;
        var originalImageRect = originalImage.GetComponent<RectTransform>();
        originalImageRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);


        foreach (RectTransform rect in rectTransform)
        {
            rect.SetAnchorWithKeepingPosition(0f, 1f);
        }
        foreach (RectTransform rect in rectTransform_mask)
        {
            //Debug.Log(rect.name);
            rect.SetAnchorWithKeepingPosition(0f, 1f);
        }
        if (RectWH == new Vector2(0,0)) 
        { 
            RectWH = rectTransform[3].anchoredPosition;
        }



        // 中心位置を計算
        float centerX = originalImageRect.sizeDelta.x / 2;
        float centerY = originalImageRect.sizeDelta.y / 2;

        //showImageのRectTransformの値1に対して、Texture2Dのピクセル数はどれだけかを計算
        var pixelWidthPerRectTransformWidth = originalTexture.width / originalImageRect.sizeDelta.x;
        var pixelHeightPerRectTransformHeight = originalTexture.height / originalImageRect.sizeDelta.y;

        //確定した矩形範囲にあたる位置、幅、高さを計算し、取得
        int rectangleX = (int)(showImageUnMask.offsetMin.x * pixelWidthPerRectTransformWidth);
        int rectangleY = (int)(showImageUnMask.offsetMax.y * pixelHeightPerRectTransformHeight * (-1));
        int rectangleWidth = (int)((originalImageRect.sizeDelta.x - (showImageUnMask.offsetMin.x + rectangleImage.rectTransform.offsetMax.x * (-1))) * pixelWidthPerRectTransformWidth);
        int rectangleHeight = (int)((originalImageRect.sizeDelta.y - (showImageUnMask.offsetMin.y + rectangleImage.rectTransform.offsetMax.y * (-1))) * pixelHeightPerRectTransformHeight);

        // 矩形の四つ角の値を計算
        int topLeftX = rectangleX;
        int topLeftY = rectangleY;
        int topRightX = rectangleX + rectangleWidth;
        int topRightY = rectangleY;
        int bottomLeftX = rectangleX;
        int bottomLeftY = rectangleY + rectangleHeight;
        int bottomRightX = rectangleX + rectangleWidth;
        int bottomRightY = rectangleY + rectangleHeight;


        int w = originalTexture.width;
        int h = originalTexture.height;

        // 回転前の座標*
        Vector2 originalCoord_topLeft = new Vector2(topLeftX, topLeftY);

        // 回転後の座標を計算
        Vector2 rotatedPoint_topLeft = RotatePoint(originalCoord_topLeft,w,h,clockwise);

        // 回転前の座標
        Vector2 originalCoord_topRight = new Vector2(topRightX, topRightY);

        // 回転後の座標を計算
        Vector2 rotatedPoint_topRight = RotatePoint(originalCoord_topRight, w, h, clockwise);

        // 回転前の座標
        Vector2 originalCoord_ButtomLeft = new Vector2(bottomLeftX, bottomLeftY);

        // 回転後の座標を計算
        Vector2 rotatedPoint_ButtomLeft = RotatePoint(originalCoord_ButtomLeft, w, h, clockwise);


        // 回転前の座標
        Vector2 originalCoord_ButtomRight = new Vector2(bottomRightX, bottomRightY);

        // 回転後の座標を計算
        Vector2 rotatedPoint_ButtomRight = RotatePoint(originalCoord_ButtomRight, w, h, clockwise);


        float aspectRatio = (float)rotatedTexture.height / (float)rotatedTexture.width;

        texture = RotateTexture(texture, clockwise);

        imageEdit.Rotate(texture);


        originalImageRect.anchorMin = new Vector2(0, 0);
        originalImageRect.anchorMax = new Vector2(1, 1);

        // 親オブジェクトにStretchするようにサイズを変更
        originalImageRect.offsetMin = new Vector2(0, 0);
        originalImageRect.offsetMax = new Vector2(0, 0);


        originalImageRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);

        //回転によって入れる座標を変える


        if (clockwise)
        {
            anchorPosTopLeft = new Vector2(rotatedPoint_ButtomLeft.x * Mathf.Abs(originalImageRect.sizeDelta.x), rotatedPoint_ButtomLeft.y * -Mathf.Abs(originalImageRect.sizeDelta.y));
            anchorPosTopRight = new Vector2(rotatedPoint_topLeft.x * Mathf.Abs(originalImageRect.sizeDelta.x), rotatedPoint_topLeft.y * -Mathf.Abs(originalImageRect.sizeDelta.y));
            anchorPosBottomRight = new Vector2(rotatedPoint_topRight.x * Mathf.Abs(originalImageRect.sizeDelta.x), rotatedPoint_topRight.y * -Mathf.Abs(originalImageRect.sizeDelta.y));
            anchorPosBottomLeft = new Vector2(rotatedPoint_ButtomRight.x * Mathf.Abs(originalImageRect.sizeDelta.x), rotatedPoint_ButtomRight.y * -Mathf.Abs(originalImageRect.sizeDelta.y));
        }
        else
        {
            anchorPosTopLeft = new Vector2(rotatedPoint_topRight.x * Mathf.Abs(originalImageRect.sizeDelta.x), rotatedPoint_topRight.y * -Mathf.Abs(originalImageRect.sizeDelta.y));
            anchorPosTopRight = new Vector2(rotatedPoint_ButtomRight.x * Mathf.Abs(originalImageRect.sizeDelta.x), rotatedPoint_ButtomRight.y * -Mathf.Abs(originalImageRect.sizeDelta.y));
            anchorPosBottomRight = new Vector2(rotatedPoint_ButtomLeft.x * Mathf.Abs(originalImageRect.sizeDelta.x), rotatedPoint_ButtomLeft.y * -Mathf.Abs(originalImageRect.sizeDelta.y));
            anchorPosBottomLeft = new Vector2(rotatedPoint_topLeft.x * Mathf.Abs(originalImageRect.sizeDelta.x), rotatedPoint_topLeft.y * -Mathf.Abs(originalImageRect.sizeDelta.y));
        }

        Debug.Log(anchorPosTopLeft);
        rectTransform[0].anchoredPosition = new Vector2(anchorPosBottomLeft.x + 21, anchorPosBottomLeft.y + 21);
        rectTransform[1].anchoredPosition = new Vector2(anchorPosTopLeft.x + 21, anchorPosTopLeft.y - 21);
        rectTransform[2].anchoredPosition = new Vector2(anchorPosTopRight.x - 21, anchorPosTopRight.y - 21);
        rectTransform[3].anchoredPosition = new Vector2(anchorPosBottomRight.x - 21, anchorPosBottomRight.y + 21);

        rectTransform_mask[0].anchoredPosition = new Vector2(anchorPosBottomLeft.x + 26, anchorPosBottomLeft.y + 26);
        rectTransform_mask[1].anchoredPosition = new Vector2(anchorPosTopLeft.x + 26, anchorPosTopLeft.y - 26);
        rectTransform_mask[2].anchoredPosition = new Vector2(anchorPosTopRight.x - 26, anchorPosTopRight.y - 26);
        rectTransform_mask[3].anchoredPosition = new Vector2(anchorPosBottomRight.x - 26, anchorPosBottomRight.y + 26);



        unMaskRect.offsetMin = new Vector2(anchorPosBottomLeft.x + 0.6f, originalImageRect.sizeDelta.y + anchorPosBottomLeft.y + 0.6f);
        unMaskRect.offsetMax = new Vector2(anchorPosTopRight.x - originalImageRect.sizeDelta.x - 0.6f, anchorPosTopRight.y - 0.6f);

        rectImageRect.offsetMin = new Vector2(anchorPosBottomLeft.x + 3f, originalImageRect.sizeDelta.y + anchorPosBottomLeft.y +3f);
        rectImageRect.offsetMax = new Vector2(anchorPosTopRight.x - originalImageRect.sizeDelta.x-3f, anchorPosTopRight.y -3f);
        rectImageUnMaskRect.offsetMin = new Vector2(anchorPosBottomLeft.x  + 5f, originalImageRect.sizeDelta.y + anchorPosBottomLeft.y + 5f);
        rectImageUnMaskRect.offsetMax = new Vector2(anchorPosTopRight.x - originalImageRect.sizeDelta.x -5f, anchorPosTopRight.y -5f);

        RectWH = new Vector2(RectWH.y, RectWH.x);

        originalImageRect.anchorMin = new Vector2(0, 0);
        originalImageRect.anchorMax = new Vector2(1, 1);

        // 親オブジェクトにStretchするようにサイズを変更
        originalImageRect.offsetMin = new Vector2(0, 0);
        originalImageRect.offsetMax = new Vector2(0, 0);

    }

    private Vector2 RotatePoint(Vector2 point,int width,int height, bool clockwise)
    {
        //Debug.Log(width + " : " + height);
        point = new Vector2(point.x / width, point.y / height);
        //Debug.Log(point);
        Vector2 rotatePoint;
        float rad = Mathf.PI / 2;

        if(clockwise)
        {
            rotatePoint.x = (point.x * Mathf.Cos(rad) - point.y * Mathf.Sin(rad)) + 1;
            rotatePoint.y = (point.x * Mathf.Sin(rad) + point.y * Mathf.Cos(rad));
            //Debug.Log("rotatePoint = " + rotatePoint);
        }
        else
        {
            rotatePoint.x = (point.x * Mathf.Cos(rad) + point.y * Mathf.Sin(rad));
            rotatePoint.y = (-point.x * Mathf.Sin(rad) + point.y * Mathf.Cos(rad)) + 1;
            //Debug.Log("rotatePoint = " + rotatePoint);

        }
        return rotatePoint;
    }

    private Texture2D RotateTexture(Texture2D originalTexture, bool clockwise)
    {
        Color32[] original = originalTexture.GetPixels32();
        Color32[] rotated = new Color32[original.Length];

        int h = originalTexture.height;
        int w = originalTexture.width;

        //Debug.Log(w + " :  " + h);
        int iRotated, iOriginal;

        //Debug.Log(original.Length);

        for (int j = 0; j < h; j++)
        {
            for (int i = 0; i < w; i++)
            {
                iRotated = (i + 1) * h - j - 1;
                iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                rotated[iRotated] = original[iOriginal];
            }
        }

        Texture2D rotatedTexture = new Texture2D(h, w);
        rotatedTexture.SetPixels32(rotated);
        rotatedTexture.Apply();

        return rotatedTexture;
    }

    public void ReStart()
    {
        // handleRangeImageの親オブジェクトを表示
        handleRangeImage.transform.parent.gameObject.SetActive(true);
        // 画像選択ボタンを表示
        imageChoiceButton.SetActive(true);

        handleRangeImage.gameObject.SetActive(false);
        createButton.SetActive(false);
        redoButton.SetActive(false);
        createCard.SetActive(false);
        clockwiseButton.SetActive(false);
        counterClockwiseButton.SetActive(false);

        // メッセージを消去
        messageText.text = "";
        
    }
}
