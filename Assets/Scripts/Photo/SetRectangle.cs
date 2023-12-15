using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SetRectangle : MonoBehaviour
{
    [SerializeField] private RawImage handleRangeImage;
    [SerializeField] private RectTransform originalImageRect;
    [SerializeField] private RawImage unMaskImage;
    [SerializeField] private GameObject leftUpper;
    [SerializeField] private Image leftUpperUnMask;
    [SerializeField] private Image rightUpper;
    [SerializeField] private Image rightUpperUnMask;
    [SerializeField] private Image leftLower;
    [SerializeField] private Image leftLowerUnMask;
    [SerializeField] private Image rightLower;
    [SerializeField] private Image rightLowerUnMask;
    [SerializeField] private Image rectImage;
    [SerializeField] private Image rectImageUnMask;

    private Vector2[] leftUpperPoints = new Vector2[2];
    private Vector2[] rightUpperPoints = new Vector2[2];
    private Vector2[] leftLowerPoints = new Vector2[2];
    private Vector2[] rightLowerPoints = new Vector2[2];

    private RectTransform unMaskRect;
    private RectTransform leftUpperRect;
    private RectTransform leftUpperUnMaskRect;
    private RectTransform rightUpperRect;
    private RectTransform rightUpperUnMaskRect;
    private RectTransform leftLowerRect;
    private RectTransform leftLowerUnMaskRect;
    private RectTransform rightLowerRect;
    private RectTransform rightLowerUnMaskRect;
    private RectTransform rectImageRect;
    private RectTransform rectImageUnMaskRect;
    public RectTransform showImageRect;

    private Vector2[] unMaskRectInitial = new Vector2[2];
    private Vector2[] leftUpperRectInitial = new Vector2[2];
    private Vector2[] leftUpperUnMaskRectInitial = new Vector2[2];
    private Vector2[] rightUpperRectInitial = new Vector2[2];
    private Vector2[] rightUpperUnMaskRectInitial = new Vector2[2];
    private Vector2[] leftLowerRectInitial = new Vector2[2];
    private Vector2[] leftLowerUnMaskRectInitial = new Vector2[2];
    private Vector2[] rightLowerRectInitial = new Vector2[2];
    private Vector2[] rightLowerUnMaskRectInitial = new Vector2[2];
    private Vector2[] rectImageRectInitial = new Vector2[2];
    private Vector2[] rectImageUnMaskRectInitial = new Vector2[2];

    private bool touchBigin = true;
    private Vector2 offset = Vector2.zero;
    public float handleSpaceThereshold; //ハンドル間の最小距離

    //アンカー変更後の初期位置
    public Vector2[] initialHandlePos = new Vector2[4] { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero }; //ハンドルの初期位置(左上、右上、左下、右下)
    public Vector2[] initialHandleUnMaskPos = new Vector2[4] { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero }; //ハンドルUnMaskの初期位置(左上、右上、左下、右下)

    public Vector2 MaxRectSize;
    private Vector2 MaxRectSize_LeftUpper;
    private Vector2 MaxRectSize_RightUpper;
    private Vector2 MaxRectSize_LeftLower;
    private Vector2 MaxRectSize_RightLower;
    private Vector2 MaxRectSize_LeftUpper_Mask;
    private Vector2 MaxRectSize_RightUpper_Mask;
    private Vector2 MaxRectSize_LeftLower_Mask;
    private Vector2 MaxRectSize_RightLower_Mask;

    private void Start()
    {
        unMaskRect = unMaskImage.GetComponent<RectTransform>();
        leftUpperRect = leftUpper.GetComponent<RectTransform>();
        leftUpperUnMaskRect = leftUpperUnMask.GetComponent<RectTransform>();
        rightUpperRect = rightUpper.GetComponent<RectTransform>();
        rightUpperUnMaskRect = rightUpperUnMask.GetComponent<RectTransform>();
        leftLowerRect = leftLower.GetComponent<RectTransform>();
        leftLowerUnMaskRect = leftLowerUnMask.GetComponent<RectTransform>();
        rightLowerRect = rightLower.GetComponent<RectTransform>();
        rightLowerUnMaskRect = rightLowerUnMask.GetComponent<RectTransform>();

        rectImageRect = rectImage.GetComponent<RectTransform>();
        rectImageUnMaskRect = rectImageUnMask.GetComponent<RectTransform>();

        //各RectTransformの初期値を保存
        SaveInitialRectTransformValue();

    }

    public void Init()
    {

        //初期化処理
        unMaskRect.offsetMin = unMaskRectInitial[0];
        unMaskRect.offsetMax = unMaskRectInitial[1];

        leftUpperRect.SetAnchorWithKeepingPosition(0f, 1.0f);
        leftUpperRect.anchoredPosition = leftUpperRectInitial[0];
        leftUpperRect.sizeDelta = leftUpperRectInitial[1];
        leftUpperUnMaskRect.SetAnchorWithKeepingPosition(0f, 1.0f);
        leftUpperUnMaskRect.anchoredPosition = leftUpperUnMaskRectInitial[0];
        leftUpperUnMaskRect.sizeDelta = leftUpperUnMaskRectInitial[1];

        rightUpperRect.SetAnchorWithKeepingPosition(1.0f, 1.0f);
        rightUpperRect.anchoredPosition = rightUpperRectInitial[0];
        rightUpperRect.sizeDelta = rightUpperRectInitial[1];
        rightUpperUnMaskRect.SetAnchorWithKeepingPosition(1.0f, 1.0f);
        rightUpperUnMaskRect.anchoredPosition = rightUpperUnMaskRectInitial[0];
        rightUpperUnMaskRect.sizeDelta = rightUpperUnMaskRectInitial[1];

        leftLowerRect.SetAnchorWithKeepingPosition(0f, 0f);
        leftLowerRect.anchoredPosition = leftLowerRectInitial[0];
        leftLowerRect.sizeDelta = leftLowerRectInitial[1];
        leftLowerUnMaskRect.SetAnchorWithKeepingPosition(0f, 0f);
        leftLowerUnMaskRect.anchoredPosition = leftLowerUnMaskRectInitial[0];
        leftLowerUnMaskRect.sizeDelta = leftLowerUnMaskRectInitial[1];

        rightLowerRect.SetAnchorWithKeepingPosition(1.0f, 0f);
        rightLowerRect.anchoredPosition = rightLowerRectInitial[0];
        rightLowerRect.sizeDelta = rightLowerRectInitial[1];
        rightLowerUnMaskRect.SetAnchorWithKeepingPosition(1.0f, 0f);
        rightLowerUnMaskRect.anchoredPosition = rightLowerUnMaskRectInitial[0];
        rightLowerUnMaskRect.sizeDelta = rightLowerUnMaskRectInitial[1];

        rectImageRect.offsetMin = rectImageRectInitial[0];
        rectImageRect.offsetMax = rectImageRectInitial[1];
        rectImageUnMaskRect.offsetMin = rectImageUnMaskRectInitial[0];
        rectImageUnMaskRect.offsetMax = rectImageUnMaskRectInitial[1];

        for(int i = 0; i < 4; i++)
        {
            initialHandlePos[i] = Vector2.zero;
            initialHandleUnMaskPos[i] = Vector2.zero;
        }

        // 画像の縦と横の長さを取得
        float width = handleRangeImage.texture.width;
        float height = handleRangeImage.texture.height;
        Debug.Log(width + " : " + height);
        // 画像の短い方を基準として、横:縦 = 9 : 16の比率としたときの、横と縦の長さを取得
        if(width > height)
        {
            // 右上、右下のハンドルの位置をアスペクト比9:16の位置に移動
            rightUpperRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
            rightUpperUnMaskRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
            var rectWidth = rightUpperRect.anchoredPosition.x * 2.0f;
            var unMaskRectWidth = rightUpperUnMaskRect.anchoredPosition.x * 2.0f;
            var RectWidthPerPixelWidth = rectWidth / width;
            var unMaskRectWidthPerPixelWidth = unMaskRectWidth / width;
            width = height * 9f / 16f;

            rightUpperRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);     
            rightUpperRect.anchoredPosition = new Vector2(-rightUpperRect.anchoredPosition.x + (width * RectWidthPerPixelWidth) , rightUpperRect.anchoredPosition.y);
            
            rightUpperUnMaskRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
            rightUpperUnMaskRect.anchoredPosition = new Vector2(-rightUpperUnMaskRect.anchoredPosition.x + (width * unMaskRectWidthPerPixelWidth) - 7f, rightUpperUnMaskRect.anchoredPosition.y);

            rightLowerRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
            rightLowerRect.anchoredPosition = new Vector2(-rightLowerRect.anchoredPosition.x + (width * RectWidthPerPixelWidth), rightLowerRect.anchoredPosition.y);
            
            rightLowerUnMaskRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
            rightLowerUnMaskRect.anchoredPosition = new Vector2(-rightLowerUnMaskRect.anchoredPosition.x + (width * unMaskRectWidthPerPixelWidth) - 7f, rightLowerUnMaskRect.anchoredPosition.y);

            unMaskRect.offsetMax = new Vector2(unMaskRect.offsetMax.x - (rectWidth - (width * RectWidthPerPixelWidth)), unMaskRect.offsetMax.y);
            
            rectImageRect.offsetMax = new Vector2(rectImageRect.offsetMax.x - (rectWidth - (width * RectWidthPerPixelWidth)), rectImageRect.offsetMax.y);
            rectImageUnMaskRect.offsetMax = new Vector2(rectImageUnMaskRect.offsetMax.x - (rectWidth - (width * RectWidthPerPixelWidth)), rectImageUnMaskRect.offsetMax.y);
        
            while(rightUpperRect.anchoredPosition.x >= (rectWidth / 2.0f))
            {
                rightUpperRect.anchoredPosition = new Vector2(rightUpperRect.anchoredPosition.x - 0.1f, rightUpperRect.anchoredPosition.y);
                rightUpperUnMaskRect.anchoredPosition = new Vector2(rightUpperUnMaskRect.anchoredPosition.x - 0.1f, rightUpperUnMaskRect.anchoredPosition.y);
                rightLowerRect.anchoredPosition = new Vector2(rightLowerRect.anchoredPosition.x - 0.1f, rightLowerRect.anchoredPosition.y + 0.1f);
                rightLowerUnMaskRect.anchoredPosition = new Vector2(rightLowerUnMaskRect.anchoredPosition.x - 0.1f, rightLowerUnMaskRect.anchoredPosition.y + 0.1f);

                leftLowerRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                leftLowerRect.anchoredPosition = new Vector2(leftLowerRect.anchoredPosition.x, leftLowerRect.anchoredPosition.y + 0.1f);
                leftLowerUnMaskRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                leftLowerUnMaskRect.anchoredPosition = new Vector2(leftLowerUnMaskRect.anchoredPosition.x, leftLowerUnMaskRect.anchoredPosition.y + 0.1f);

                unMaskRect.offsetMin = new Vector2(unMaskRect.offsetMin.x, unMaskRect.offsetMin.y + 0.1f);
                unMaskRect.offsetMax = new Vector2(unMaskRect.offsetMax.x - 0.1f, unMaskRect.offsetMax.y);
                rectImageRect.offsetMin = new Vector2(rectImageRect.offsetMin.x, rectImageRect.offsetMin.y + 0.1f);
                rectImageRect.offsetMax = new Vector2(rectImageRect.offsetMax.x - 0.1f, rectImageRect.offsetMax.y);
                rectImageUnMaskRect.offsetMin = new Vector2(rectImageUnMaskRect.offsetMin.x, rectImageUnMaskRect.offsetMin.y + 0.1f);
                rectImageUnMaskRect.offsetMax = new Vector2(rectImageUnMaskRect.offsetMax.x - 0.1f, rectImageUnMaskRect.offsetMax.y);   
            }

            rightUpperRect.SetAnchorWithKeepingPosition(1.0f, 1.0f);
            rightUpperUnMaskRect.SetAnchorWithKeepingPosition(1.0f, 1.0f);
            leftLowerRect.SetAnchorWithKeepingPosition(0.0f, 0.0f);
            leftLowerUnMaskRect.SetAnchorWithKeepingPosition(0.0f, 0.0f);
            rightLowerRect.SetAnchorWithKeepingPosition(1.0f, 0.0f);
            rightLowerUnMaskRect.SetAnchorWithKeepingPosition(1.0f, 0.0f);
        }
        else
        {
            // 左下、右下のハンドルの位置をアスペクト比9:16の位置に移動
            rightUpperRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
            var rectHeight = rightUpperRect.anchoredPosition.y * 2.0f;
            var RectHeightPerPixelHeight = rightUpperRect.anchoredPosition.y * 2.0f / height;
            height = width * 16 / 9;

            leftLowerRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
            leftLowerRect.anchoredPosition = new Vector2(leftLowerRect.anchoredPosition.x, -leftLowerRect.anchoredPosition.y - (height * RectHeightPerPixelHeight));
            
            leftLowerUnMaskRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
            leftLowerUnMaskRect.anchoredPosition = new Vector2(leftLowerUnMaskRect.anchoredPosition.x, -leftLowerUnMaskRect.anchoredPosition.y - (height * RectHeightPerPixelHeight) + 10f);
            

            rightLowerRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
            rightLowerRect.anchoredPosition = new Vector2(rightLowerRect.anchoredPosition.x, -rightLowerRect.anchoredPosition.y - (height * RectHeightPerPixelHeight));
            
            rightLowerUnMaskRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
            rightLowerUnMaskRect.anchoredPosition = new Vector2(rightLowerUnMaskRect.anchoredPosition.x, -rightLowerUnMaskRect.anchoredPosition.y - (height * RectHeightPerPixelHeight) + 10f);
            

            unMaskRect.offsetMin = new Vector2(unMaskRect.offsetMin.x, unMaskRect.offsetMin.y + (rectHeight - (height * RectHeightPerPixelHeight)));

            rectImageRect.offsetMin = new Vector2(rectImageRect.offsetMin.x, rectImageRect.offsetMin.y + (rectHeight - (height * RectHeightPerPixelHeight)));
            rectImageUnMaskRect.offsetMin = new Vector2(rectImageUnMaskRect.offsetMin.x, rectImageUnMaskRect.offsetMin.y + (rectHeight - (height * RectHeightPerPixelHeight)));

            // 調整した結果、ハンドルが画像の外に出てしまった場合(正方形に近い形の時)は、アスペクト比を維持したままハンドルを画像の内側に戻す
            while(leftLowerRect.anchoredPosition.y <= -(rectHeight / 2.0f))
            {
                rightUpperRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                rightUpperRect.anchoredPosition = new Vector2(rightUpperRect.anchoredPosition.x - 0.1f, rightUpperRect.anchoredPosition.y);
                rightUpperUnMaskRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                rightUpperUnMaskRect.anchoredPosition = new Vector2(rightUpperUnMaskRect.anchoredPosition.x - 0.1f, rightUpperUnMaskRect.anchoredPosition.y);

                leftLowerRect.anchoredPosition = new Vector2(leftLowerRect.anchoredPosition.x, leftLowerRect.anchoredPosition.y + 0.1f);
                leftLowerUnMaskRect.anchoredPosition = new Vector2(leftLowerUnMaskRect.anchoredPosition.x, leftLowerUnMaskRect.anchoredPosition.y + 0.1f);
                rightLowerRect.anchoredPosition = new Vector2(rightLowerRect.anchoredPosition.x - 0.1f, rightLowerRect.anchoredPosition.y + 0.1f);
                rightLowerUnMaskRect.anchoredPosition = new Vector2(rightLowerUnMaskRect.anchoredPosition.x - 0.1f, rightLowerUnMaskRect.anchoredPosition.y + 0.1f);

                unMaskRect.offsetMin = new Vector2(unMaskRect.offsetMin.x, unMaskRect.offsetMin.y + 0.1f);
                unMaskRect.offsetMax = new Vector2(unMaskRect.offsetMax.x - 0.1f, unMaskRect.offsetMax.y);
                rectImageRect.offsetMin = new Vector2(rectImageRect.offsetMin.x, rectImageRect.offsetMin.y + 0.1f);
                rectImageRect.offsetMax = new Vector2(rectImageRect.offsetMax.x - 0.1f, rectImageRect.offsetMax.y);
                rectImageUnMaskRect.offsetMin = new Vector2(rectImageUnMaskRect.offsetMin.x, rectImageUnMaskRect.offsetMin.y + 0.1f);
                rectImageUnMaskRect.offsetMax = new Vector2(rectImageUnMaskRect.offsetMax.x - 0.1f, rectImageUnMaskRect.offsetMax.y);
            }

            rightUpperRect.SetAnchorWithKeepingPosition(1.0f, 1.0f);
            rightUpperUnMaskRect.SetAnchorWithKeepingPosition(1.0f, 1.0f);
            leftLowerRect.SetAnchorWithKeepingPosition(0.0f, 0.0f);
            leftLowerUnMaskRect.SetAnchorWithKeepingPosition(0.0f, 0.0f);
            rightLowerRect.SetAnchorWithKeepingPosition(1.0f, 0.0f);
            rightLowerUnMaskRect.SetAnchorWithKeepingPosition(1.0f, 0.0f);

        }


        touchBigin = true;
    }

    public void OnAdjustmentObjectDrag(string position)
    {
        Vector3 clickPos = Vector3.zero;
        Vector2 localPoint = Vector2.zero;
        Vector2 touchPos = Vector2.zero;
        float amountX = 0f;
        float amountY = 0f;

        bool isReachingLimitX = false;
        bool isReachingLimitY = false;

        switch(position)
        {
            case "leftUpper":
            {
                if(touchBigin)
                {
                    //ドラッグ開始時の処理

                    //UIを移動させるため、アンカーを中央に設定
                    leftUpperRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    leftUpperUnMaskRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    leftLowerRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    leftLowerUnMaskRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    rightUpperRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    rightUpperUnMaskRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);

                    //現在の持ち手の位置を取得
                    leftUpperPoints[0] = leftUpperRect.anchoredPosition;

                        

                    //originalImageRect.sizeDeltaこいつのサイズを持ってくる。最大値をこいつで判定するようにしてしまえばいける
                    originalImageRect.anchorMin = new Vector2(0, 0);
                    originalImageRect.anchorMax = new Vector2(1, 1);

                    // 親オブジェクトにStretchするようにサイズを変更
                    originalImageRect.offsetMin = new Vector2(0, 0);
                    originalImageRect.offsetMax = new Vector2(0, 0);

                    originalImageRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    MaxRectSize = originalImageRect.sizeDelta;
                    Debug.Log(MaxRectSize);

                    MaxRectSize_LeftUpper = new Vector2(-Mathf.Abs(MaxRectSize.x) / 2 +21f, Mathf.Abs(MaxRectSize.y) / 2 - 21f);
                    MaxRectSize_RightUpper = new Vector2(Mathf.Abs(MaxRectSize.x) / 2 -21f, Mathf.Abs(MaxRectSize.y) / 2 - 21f);
                    MaxRectSize_LeftLower = new Vector2(-Mathf.Abs(MaxRectSize.x) / 2 + 21f, -Mathf.Abs(MaxRectSize.y) / 2 + 21f);
                    MaxRectSize_RightLower = new Vector2(Mathf.Abs(MaxRectSize.x) / 2 -21f, -Mathf.Abs(MaxRectSize.y) / 2 + 21f);


                    MaxRectSize_LeftUpper_Mask = new Vector2(-Mathf.Abs(MaxRectSize.x) / 2 +26f, Mathf.Abs(MaxRectSize.y) / 2 -26f);
                    MaxRectSize_RightUpper_Mask = new Vector2(Mathf.Abs(MaxRectSize.x) / 2 + 26f, Mathf.Abs(MaxRectSize.y) / 2 - 26f);
                    MaxRectSize_LeftLower_Mask = new Vector2(-Mathf.Abs(MaxRectSize.x) / 2 + 26f, -Mathf.Abs(MaxRectSize.y) / 2 + 26f);
                    MaxRectSize_RightLower_Mask = new Vector2(Mathf.Abs(MaxRectSize.x) / 2 - 26f, -Mathf.Abs(MaxRectSize.y) / 2 + 26f);
                    Debug.Log(MaxRectSize_LeftUpper);
                    Debug.Log(MaxRectSize_LeftLower);
                }
                
                #if UNITY_EDITOR
 
                clickPos = Input.mousePosition;  
                RectTransformUtility.ScreenPointToLocalPointInRectangle(showImageRect, clickPos, null, out localPoint);
                
                if(touchBigin)
                {
                    //クリック/タッチした位置と持ち手の位置の差分を取得
                    touchBigin = false;
                    offset = leftUpperRect.anchoredPosition - localPoint;
                }

                //差分を考慮して移動先の位置を取得(差分を含めないとドラッグし始めに差分の分だけ瞬間移動してしまう)
                leftUpperPoints[1] = localPoint + offset;

                    //Debug.Log(initialHandlePos[0] + " :: " + initialHandlePos[1] + " : : " + initialHandlePos[2] + " : : " + initialHandlePos[3]);
                if (leftUpperPoints[1].x < MaxRectSize_LeftUpper.x)
                {
                    //持ち手が初期値の位置より左に行かないようにする
                    leftUpperRect.anchoredPosition = new Vector2(MaxRectSize_LeftUpper.x, leftUpperRect.anchoredPosition.y);
                    leftUpperUnMaskRect.anchoredPosition = new Vector2(MaxRectSize_LeftUpper_Mask.x, leftUpperUnMaskRect.anchoredPosition.y);
                    rectImageRect.offsetMin = new Vector2(rectImageRectInitial[0].x, rectImageRect.offsetMin.y);
                    rectImageUnMaskRect.offsetMin = new Vector2(rectImageUnMaskRectInitial[0].x, rectImageUnMaskRect.offsetMin.y);
                    unMaskRect.offsetMin = new Vector2(unMaskRectInitial[0].x, unMaskRect.offsetMin.y);
                    leftLowerRect.anchoredPosition = new Vector2(MaxRectSize_LeftLower.x, leftLowerRect.anchoredPosition.y);
                    leftLowerUnMaskRect.anchoredPosition = new Vector2(MaxRectSize_LeftLower_Mask.x, leftLowerUnMaskRect.anchoredPosition.y);

                    leftUpperPoints[1].x = initialHandlePos[0].x;
                    leftUpperPoints[0].x = leftUpperPoints[1].x;
                    isReachingLimitX = true;
                }

                if(leftUpperPoints[1].y > MaxRectSize_LeftUpper.y)
                {
                    //持ち手が初期値の位置より上に行かないようにする
                    leftUpperRect.anchoredPosition = new Vector2(leftUpperRect.anchoredPosition.x, MaxRectSize_LeftUpper.y);
                    leftUpperUnMaskRect.anchoredPosition = new Vector2(leftUpperUnMaskRect.anchoredPosition.x, MaxRectSize_LeftUpper_Mask.y);
                    rectImageRect.offsetMax = new Vector2(rectImageRect.offsetMax.x, rectImageRectInitial[1].y);
                    rectImageUnMaskRect.offsetMax = new Vector2(rectImageUnMaskRect.offsetMax.x, rectImageUnMaskRectInitial[1].y);
                    unMaskRect.offsetMax = new Vector2(unMaskRect.offsetMax.x, unMaskRectInitial[1].y);
                    rightUpperRect.anchoredPosition = new Vector2(rightUpperRect.anchoredPosition.x, MaxRectSize_RightUpper.y);
                    rightUpperUnMaskRect.anchoredPosition = new Vector2(rightUpperUnMaskRect.anchoredPosition.x, MaxRectSize_RightUpper_Mask.y);

                    leftUpperPoints[1].y = initialHandlePos[0].y;
                    leftUpperPoints[0].y = leftUpperPoints[1].y;
                    isReachingLimitY = true;
                }
                

                if((rightUpperRect.anchoredPosition.x - leftUpperPoints[1].x) <= handleSpaceThereshold)
                {
                    //右上の持ち手より右に行かないようにする
                    leftUpperPoints[1].x = rightUpperRect.anchoredPosition.x - handleSpaceThereshold;
                    leftUpperPoints[0].x = leftUpperPoints[1].x;
                    isReachingLimitX = true;
                }

                if((leftUpperPoints[1].y - leftLowerRect.anchoredPosition.y) <= handleSpaceThereshold) 
                {
                    //左下の持ち手より下に行かないようにする
                    leftUpperPoints[1].y = leftLowerRect.anchoredPosition.y - handleSpaceThereshold;
                    leftUpperPoints[0].y = leftUpperPoints[1].y;
                    isReachingLimitY = true;
                }

                //持ち手の移動量取得
                amountX = (leftUpperPoints[1].x - leftUpperPoints[0].x);
                amountY = (leftUpperPoints[1].y - leftUpperPoints[0].y);

                //各オブジェクトの位置を移動
                unMaskRect.offsetMin = new Vector2(unMaskRect.offsetMin.x + amountX, unMaskRect.offsetMin.y);
                unMaskRect.offsetMax = new Vector2(unMaskRect.offsetMax.x, unMaskRect.offsetMax.y + amountY);
                leftUpperUnMaskRect.anchoredPosition += new Vector2(amountX, amountY);

                rectImageRect.offsetMin = new Vector2(rectImageRect.offsetMin.x + amountX, rectImageRect.offsetMin.y);
                rectImageRect.offsetMax = new Vector2(rectImageRect.offsetMax.x, rectImageRect.offsetMax.y + amountY);
                rectImageUnMaskRect.offsetMin = new Vector2(rectImageUnMaskRect.offsetMin.x + amountX, rectImageUnMaskRect.offsetMin.y);
                rectImageUnMaskRect.offsetMax = new Vector2(rectImageUnMaskRect.offsetMax.x, rectImageUnMaskRect.offsetMax.y + amountY);
                
                rightUpperRect.anchoredPosition += new Vector2(0f, amountY);
                rightUpperUnMaskRect.anchoredPosition += new Vector2(0f, amountY);
                
                leftLowerRect.anchoredPosition += new Vector2(amountX, 0f);
                leftLowerUnMaskRect.anchoredPosition += new Vector2(amountX, 0f);


#else

                if (Input.touchCount > 0)  
                {
                    var touch = Input.GetTouch(0);  
                    
                    touchPos = touch.position;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(showImageRect, touchPos, null, out localPoint);

                    if(touchBigin)
                    {
                        touchBigin = false;
                        offset = leftUpperRect.anchoredPosition - localPoint;
                    } 
                
                    leftUpperPoints[1] = localPoint + offset;

                    
                    if (leftUpperPoints[1].x < MaxRectSize_LeftUpper.x)
                    {
                        //持ち手が初期値の位置より左に行かないようにする
                        leftUpperRect.anchoredPosition = new Vector2(MaxRectSize_LeftUpper.x, leftUpperRect.anchoredPosition.y);
                        leftUpperUnMaskRect.anchoredPosition = new Vector2(MaxRectSize_LeftUpper_Mask.x, leftUpperUnMaskRect.anchoredPosition.y);
                        rectImageRect.offsetMin = new Vector2(rectImageRectInitial[0].x, rectImageRect.offsetMin.y);
                        rectImageUnMaskRect.offsetMin = new Vector2(rectImageUnMaskRectInitial[0].x, rectImageUnMaskRect.offsetMin.y);
                        unMaskRect.offsetMin = new Vector2(unMaskRectInitial[0].x, unMaskRect.offsetMin.y);
                        leftLowerRect.anchoredPosition = new Vector2(MaxRectSize_LeftLower.x, leftLowerRect.anchoredPosition.y);
                        leftLowerUnMaskRect.anchoredPosition = new Vector2(MaxRectSize_LeftLower_Mask.x, leftLowerUnMaskRect.anchoredPosition.y);

                        leftUpperPoints[1].x = initialHandlePos[0].x;
                        leftUpperPoints[0].x = leftUpperPoints[1].x;
                        isReachingLimitX = true;
                    }

                    if(leftUpperPoints[1].y > MaxRectSize_LeftUpper.y)
                    {
                        //持ち手が初期値の位置より上に行かないようにする
                        leftUpperRect.anchoredPosition = new Vector2(leftUpperRect.anchoredPosition.x, MaxRectSize_LeftUpper.y);
                        leftUpperUnMaskRect.anchoredPosition = new Vector2(leftUpperUnMaskRect.anchoredPosition.x, MaxRectSize_LeftUpper_Mask.y);
                        rectImageRect.offsetMax = new Vector2(rectImageRect.offsetMax.x, rectImageRectInitial[1].y);
                        rectImageUnMaskRect.offsetMax = new Vector2(rectImageUnMaskRect.offsetMax.x, rectImageUnMaskRectInitial[1].y);
                        unMaskRect.offsetMax = new Vector2(unMaskRect.offsetMax.x, unMaskRectInitial[1].y);
                        rightUpperRect.anchoredPosition = new Vector2(rightUpperRect.anchoredPosition.x, MaxRectSize_RightUpper.y);
                        rightUpperUnMaskRect.anchoredPosition = new Vector2(rightUpperUnMaskRect.anchoredPosition.x, MaxRectSize_RightUpper_Mask.y);

                        leftUpperPoints[1].y = initialHandlePos[0].y;
                        leftUpperPoints[0].y = leftUpperPoints[1].y;
                        isReachingLimitY = true;
                    }
                    

                    if((rightUpperRect.anchoredPosition.x - leftUpperPoints[1].x) <= handleSpaceThereshold )
                    {
                        leftUpperPoints[1].x = rightUpperRect.anchoredPosition.x - handleSpaceThereshold;
                        leftUpperPoints[0].x = leftUpperPoints[1].x;
                        isReachingLimitX = true;
                    }

                    if((leftUpperPoints[1].y - leftLowerRect.anchoredPosition.y) <= handleSpaceThereshold) 
                    {
                        leftUpperPoints[1].y = leftLowerRect.anchoredPosition.y - handleSpaceThereshold;
                        leftUpperPoints[0].y = leftUpperPoints[1].y;
                        isReachingLimitY = true;
                    }

                    amountX = (leftUpperPoints[1].x - leftUpperPoints[0].x);
                    amountY = (leftUpperPoints[1].y - leftUpperPoints[0].y);

                    unMaskRect.offsetMin = new Vector2(unMaskRect.offsetMin.x + amountX, unMaskRect.offsetMin.y);
                    unMaskRect.offsetMax = new Vector2(unMaskRect.offsetMax.x, unMaskRect.offsetMax.y + amountY);
                    leftUpperUnMaskRect.anchoredPosition += new Vector2(amountX, amountY);

                    rectImageRect.offsetMin = new Vector2(rectImageRect.offsetMin.x + amountX, rectImageRect.offsetMin.y);
                    rectImageRect.offsetMax = new Vector2(rectImageRect.offsetMax.x, rectImageRect.offsetMax.y + amountY);
                    rectImageUnMaskRect.offsetMin = new Vector2(rectImageUnMaskRect.offsetMin.x + amountX, rectImageUnMaskRect.offsetMin.y);
                    rectImageUnMaskRect.offsetMax = new Vector2(rectImageUnMaskRect.offsetMax.x, rectImageUnMaskRect.offsetMax.y + amountY);

                    rightUpperRect.anchoredPosition += new Vector2(0f, amountY);
                    rightUpperUnMaskRect.anchoredPosition += new Vector2(0f, amountY);

                    leftLowerRect.anchoredPosition += new Vector2(amountX, 0f);
                    leftLowerUnMaskRect.anchoredPosition += new Vector2(amountX, 0f); 
                }
                
#endif

                    //持ち手の位置を更新
                    if (isReachingLimitX)
                {
                    leftUpperRect.anchoredPosition += new Vector2(0f, amountY);
                }
                else if(isReachingLimitY)
                {
                    leftUpperRect.anchoredPosition += new Vector2(amountX, 0f);
                }
                else
                {
                    leftUpperRect.anchoredPosition += new Vector2(amountX, amountY);
                }
                
                //更新した持ち手の位置を次の現在の持ち手の位置として保存
                leftUpperPoints[0] = leftUpperRect.anchoredPosition;
                break;
            }
            case "rightUpper":
            {
                if(touchBigin)
                {
                    rightUpperRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    rightUpperUnMaskRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    leftUpperRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    leftUpperUnMaskRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    rightLowerRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    rightLowerUnMaskRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    
                    rightUpperPoints[0] = rightUpperRect.anchoredPosition;

                    
/*                    if(initialHandlePos[1].x == 0f) initialHandlePos[1].x = rightUpperRect.anchoredPosition.x;
                    if(initialHandlePos[1].y == 0f) initialHandlePos[1].y = rightUpperRect.anchoredPosition.y;
                    if(initialHandleUnMaskPos[1].x == 0f) initialHandleUnMaskPos[1].x = rightUpperUnMaskRect.anchoredPosition.x;
                    if(initialHandleUnMaskPos[1].y == 0f) initialHandleUnMaskPos[1].y = rightUpperUnMaskRect.anchoredPosition.y;

                    if(initialHandlePos[0].y == 0f) initialHandlePos[0].y = rightUpperRect.anchoredPosition.y;
                    if(initialHandleUnMaskPos[0].y == 0f) initialHandleUnMaskPos[0].y = rightUpperUnMaskRect.anchoredPosition.y;
                    if(initialHandlePos[3].x == 0f) initialHandlePos[3].x = rightUpperRect.anchoredPosition.x;
                    if(initialHandleUnMaskPos[3].x == 0f) initialHandleUnMaskPos[3].x = rightUpperUnMaskRect.anchoredPosition.x;*/

/*
                    initialHandleUnMaskPos[2] = Vector2.zero;
                    initialHandlePos[1] = -initialHandlePos[1];*/


                                              
                    originalImageRect.anchorMin = new Vector2(0, 0);
                    originalImageRect.anchorMax = new Vector2(1, 1);

                    // 親オブジェクトにStretchするようにサイズを変更
                    originalImageRect.offsetMin = new Vector2(0, 0);
                    originalImageRect.offsetMax = new Vector2(0, 0);

                    originalImageRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    MaxRectSize = originalImageRect.sizeDelta;
                    Debug.Log(MaxRectSize);

                    MaxRectSize_LeftUpper = new Vector2(-Mathf.Abs(MaxRectSize.x) / 2 +21f, Mathf.Abs(MaxRectSize.y) / 2 - 21f);
                    MaxRectSize_RightUpper = new Vector2(Mathf.Abs(MaxRectSize.x) / 2 -21f, Mathf.Abs(MaxRectSize.y) / 2 - 21f);
                    MaxRectSize_LeftLower = new Vector2(-Mathf.Abs(MaxRectSize.x) / 2 + 21f, -Mathf.Abs(MaxRectSize.y) / 2 + 21f);
                    MaxRectSize_RightLower = new Vector2(Mathf.Abs(MaxRectSize.x) / 2 -21f, -Mathf.Abs(MaxRectSize.y) / 2 + 21f);


                    MaxRectSize_LeftUpper_Mask = new Vector2(-Mathf.Abs(MaxRectSize.x) / 2 +26f, Mathf.Abs(MaxRectSize.y) / 2 -26f);
                    MaxRectSize_RightUpper_Mask = new Vector2(Mathf.Abs(MaxRectSize.x) / 2 - 26f, Mathf.Abs(MaxRectSize.y) / 2 - 26f);
                    MaxRectSize_LeftLower_Mask = new Vector2(-Mathf.Abs(MaxRectSize.x) / 2 + 26f, -Mathf.Abs(MaxRectSize.y) / 2 + 26f);
                    MaxRectSize_RightLower_Mask = new Vector2(Mathf.Abs(MaxRectSize.x) / 2 - 26f, -Mathf.Abs(MaxRectSize.y) / 2 + 26f);


                    }

                #if UNITY_EDITOR
 
                clickPos = Input.mousePosition;  
                RectTransformUtility.ScreenPointToLocalPointInRectangle(showImageRect, clickPos, null, out localPoint);

                if(touchBigin)
                {
                    touchBigin = false;
                    offset = rightUpperRect.anchoredPosition - localPoint;
                }

                rightUpperPoints[1] = localPoint + offset;

                    //Debug.Log(initialHandlePos[0] + " :: " + initialHandlePos[1] + " : : " + initialHandlePos[2] + " : : " + initialHandlePos[3]);


               if (rightUpperPoints[1].x > MaxRectSize_RightUpper.x)
                {
                    rightUpperRect.anchoredPosition = new Vector2(MaxRectSize_RightUpper.x, rightUpperRect.anchoredPosition.y);
                    rightUpperUnMaskRect.anchoredPosition = new Vector2(MaxRectSize_RightUpper_Mask.x, rightUpperUnMaskRect.anchoredPosition.y);
                    rectImageRect.offsetMax = new Vector2(rectImageRectInitial[1].x, rectImageRect.offsetMax.y);
                    rectImageUnMaskRect.offsetMax = new Vector2(rectImageUnMaskRectInitial[1].x, rectImageUnMaskRect.offsetMax.y);
                    unMaskRect.offsetMax = new Vector2(unMaskRectInitial[1].x, unMaskRect.offsetMax.y);
                    rightLowerRect.anchoredPosition = new Vector2(MaxRectSize_RightLower.x, rightLowerRect.anchoredPosition.y);
                    rightLowerUnMaskRect.anchoredPosition = new Vector2(MaxRectSize_RightLower_Mask.x, rightLowerUnMaskRect.anchoredPosition.y);

                    rightUpperPoints[1].x = initialHandlePos[1].x;
                    rightUpperPoints[0].x = rightUpperPoints[1].x;
                    isReachingLimitX = true;
                }
                
                if( rightUpperPoints[1].y > MaxRectSize_RightUpper.y)
                {
                    rightUpperRect.anchoredPosition = new Vector2(rightUpperRect.anchoredPosition.x, MaxRectSize_RightUpper.y);
                    rightUpperUnMaskRect.anchoredPosition = new Vector2(rightUpperUnMaskRect.anchoredPosition.x, MaxRectSize_RightUpper_Mask.y);
                    rectImageRect.offsetMax = new Vector2(rectImageRect.offsetMax.x, rectImageRectInitial[1].y);
                    rectImageUnMaskRect.offsetMax = new Vector2(rectImageUnMaskRect.offsetMax.x, rectImageUnMaskRectInitial[1].y);
                    unMaskRect.offsetMax = new Vector2(unMaskRect.offsetMax.x, unMaskRectInitial[1].y);
                    leftUpperRect.anchoredPosition = new Vector2(leftUpperRect.anchoredPosition.x, MaxRectSize_LeftUpper.y);
                    leftUpperUnMaskRect.anchoredPosition = new Vector2(leftUpperUnMaskRect.anchoredPosition.x, MaxRectSize_LeftUpper_Mask.y);

                    rightUpperPoints[1].y = initialHandlePos[1].y;
                    rightUpperPoints[0].y = rightUpperPoints[1].y;
                    isReachingLimitY = true;
                }
                

                if((rightUpperPoints[1].x - leftUpperRect.anchoredPosition.x) <= handleSpaceThereshold)
                {
                    rightUpperPoints[1].x = leftUpperRect.anchoredPosition.x + handleSpaceThereshold;
                    rightUpperPoints[0].x = rightUpperPoints[1].x;
                    isReachingLimitX = true;
                }

                if((rightUpperPoints[1].y - rightLowerRect.anchoredPosition.y) <= handleSpaceThereshold)
                {
                    rightUpperPoints[1].y = rightLowerRect.anchoredPosition.y - handleSpaceThereshold;
                    rightUpperPoints[0].y = rightUpperPoints[1].y;
                    isReachingLimitY = true;
                }

                amountX = (rightUpperPoints[1].x - rightUpperPoints[0].x);
                amountY = (rightUpperPoints[1].y - rightUpperPoints[0].y);

                unMaskRect.offsetMax = new Vector2(unMaskRect.offsetMax.x + amountX, unMaskRect.offsetMax.y + amountY);
                rightUpperUnMaskRect.anchoredPosition += new Vector2(amountX, amountY);

                rectImageRect.offsetMax = new Vector2(rectImageRect.offsetMax.x + amountX, rectImageRect.offsetMax.y + amountY);
                rectImageUnMaskRect.offsetMax = new Vector2(rectImageUnMaskRect.offsetMax.x + amountX, rectImageUnMaskRect.offsetMax.y + amountY);

                leftUpperRect.anchoredPosition += new Vector2(0f, amountY);
                leftUpperUnMaskRect.anchoredPosition += new Vector2(0f, amountY);
                
                rightLowerRect.anchoredPosition += new Vector2(amountX, 0f);
                rightLowerUnMaskRect.anchoredPosition += new Vector2(amountX, 0f);

#else

                if (Input.touchCount > 0)  
                {
                    var touch = Input.GetTouch(0);  
                    
                    touchPos = touch.position;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(showImageRect, touchPos, null, out localPoint);

                    if(touchBigin)
                    {
                        touchBigin = false;
                        offset = rightUpperRect.anchoredPosition - localPoint;
                    }

                    rightUpperPoints[1] = localPoint + offset;

                    
                   if (rightUpperPoints[1].x > MaxRectSize_RightUpper.x)
                    {
                        rightUpperRect.anchoredPosition = new Vector2(MaxRectSize_RightUpper.x, rightUpperRect.anchoredPosition.y);
                        rightUpperUnMaskRect.anchoredPosition = new Vector2(MaxRectSize_RightUpper_Mask.x, rightUpperUnMaskRect.anchoredPosition.y);
                        rectImageRect.offsetMax = new Vector2(rectImageRectInitial[1].x, rectImageRect.offsetMax.y);
                        rectImageUnMaskRect.offsetMax = new Vector2(rectImageUnMaskRectInitial[1].x, rectImageUnMaskRect.offsetMax.y);
                        unMaskRect.offsetMax = new Vector2(unMaskRectInitial[1].x, unMaskRect.offsetMax.y);
                        rightLowerRect.anchoredPosition = new Vector2(MaxRectSize_RightLower.x, rightLowerRect.anchoredPosition.y);
                        rightLowerUnMaskRect.anchoredPosition = new Vector2(MaxRectSize_RightLower_Mask.x, rightLowerUnMaskRect.anchoredPosition.y);

                        rightUpperPoints[1].x = initialHandlePos[1].x;
                        rightUpperPoints[0].x = rightUpperPoints[1].x;
                        isReachingLimitX = true;
                    }
                
                    if( rightUpperPoints[1].y > MaxRectSize_RightUpper.y)
                    {
                        rightUpperRect.anchoredPosition = new Vector2(rightUpperRect.anchoredPosition.x, MaxRectSize_RightUpper.y);
                        rightUpperUnMaskRect.anchoredPosition = new Vector2(rightUpperUnMaskRect.anchoredPosition.x, MaxRectSize_RightUpper_Mask.y);
                        rectImageRect.offsetMax = new Vector2(rectImageRect.offsetMax.x, rectImageRectInitial[1].y);
                        rectImageUnMaskRect.offsetMax = new Vector2(rectImageUnMaskRect.offsetMax.x, rectImageUnMaskRectInitial[1].y);
                        unMaskRect.offsetMax = new Vector2(unMaskRect.offsetMax.x, unMaskRectInitial[1].y);
                        leftUpperRect.anchoredPosition = new Vector2(leftUpperRect.anchoredPosition.x, MaxRectSize_LeftUpper.y);
                        leftUpperUnMaskRect.anchoredPosition = new Vector2(leftUpperUnMaskRect.anchoredPosition.x, MaxRectSize_LeftUpper_Mask.y);

                        rightUpperPoints[1].y = initialHandlePos[1].y;
                        rightUpperPoints[0].y = rightUpperPoints[1].y;
                        isReachingLimitY = true;
                    }

                    if((rightUpperPoints[1].x - leftUpperRect.anchoredPosition.x) <= handleSpaceThereshold)
                    {
                        rightUpperPoints[1].x = leftUpperRect.anchoredPosition.x + handleSpaceThereshold;
                        rightUpperPoints[0].x = rightUpperPoints[1].x;
                        isReachingLimitX = true;
                    }
                    
                    if((rightUpperPoints[1].y - rightLowerRect.anchoredPosition.y) <= handleSpaceThereshold)
                    {
                        rightUpperPoints[1].y = rightLowerRect.anchoredPosition.y - handleSpaceThereshold;
                        rightUpperPoints[0].y = rightUpperPoints[1].y;
                        isReachingLimitY = true;
                    }

                    amountX = (rightUpperPoints[1].x - rightUpperPoints[0].x);
                    amountY = (rightUpperPoints[1].y - rightUpperPoints[0].y);

                    unMaskRect.offsetMax = new Vector2(unMaskRect.offsetMax.x + amountX, unMaskRect.offsetMax.y + amountY);
                    rightUpperUnMaskRect.anchoredPosition += new Vector2(amountX, amountY);

                    rectImageRect.offsetMax = new Vector2(rectImageRect.offsetMax.x + amountX, rectImageRect.offsetMax.y + amountY);
                    rectImageUnMaskRect.offsetMax = new Vector2(rectImageUnMaskRect.offsetMax.x + amountX, rectImageUnMaskRect.offsetMax.y + amountY);

                    
                    leftUpperRect.anchoredPosition += new Vector2(0f, amountY);
                    leftUpperUnMaskRect.anchoredPosition += new Vector2(0f, amountY);
                    
                    rightLowerRect.anchoredPosition += new Vector2(amountX, 0f);
                    rightLowerUnMaskRect.anchoredPosition += new Vector2(amountX, 0f);
                }

#endif

                    if (isReachingLimitX)
                {
                    rightUpperRect.anchoredPosition += new Vector2(0f, amountY);
                }
                else if(isReachingLimitY)
                {
                    rightUpperRect.anchoredPosition += new Vector2(amountX, 0f);
                }
                else
                {
                    rightUpperRect.anchoredPosition += new Vector2(amountX, amountY);
                }

                rightUpperPoints[0] = rightUpperRect.anchoredPosition;

                break;
            }
            case "leftLower":
            {
                if(touchBigin)
                {
                    leftLowerRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    leftLowerUnMaskRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    leftUpperRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    leftUpperUnMaskRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    rightLowerRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    rightLowerUnMaskRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    
                    leftLowerPoints[0] = leftLowerRect.anchoredPosition;


                    /*                    
                     if(initialHandlePos[2].x == 0f) initialHandlePos[2].x = leftLowerRect.anchoredPosition.x;
                    if(initialHandlePos[2].y == 0f) initialHandlePos[2].y = leftLowerRect.anchoredPosition.y;
                    if(initialHandleUnMaskPos[2].x == 0f) initialHandleUnMaskPos[2].x = leftLowerUnMaskRect.anchoredPosition.x;
                    if(initialHandleUnMaskPos[2].y == 0f) initialHandleUnMaskPos[2].y = leftLowerUnMaskRect.anchoredPosition.y;

                    if(initialHandlePos[0].x == 0f) initialHandlePos[0].x = leftLowerRect.anchoredPosition.x;
                    if(initialHandleUnMaskPos[0].x == 0f) initialHandleUnMaskPos[0].x = leftLowerUnMaskRect.anchoredPosition.x;
                    if(initialHandlePos[3].y == 0f) initialHandlePos[3].y = leftLowerRect.anchoredPosition.y;
                    if(initialHandleUnMaskPos[3].y == 0f) initialHandleUnMaskPos[3].y = leftLowerUnMaskRect.anchoredPosition.y;
                    */

/*
                    initialHandleUnMaskPos[1] = Vector2.zero;
                    initialHandlePos[1] = Vector2.zero;*/

                    originalImageRect.anchorMin = new Vector2(0, 0);
                    originalImageRect.anchorMax = new Vector2(1, 1);

                    // 親オブジェクトにStretchするようにサイズを変更
                    originalImageRect.offsetMin = new Vector2(0, 0);
                    originalImageRect.offsetMax = new Vector2(0, 0);

                    originalImageRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    MaxRectSize = originalImageRect.sizeDelta;
                    Debug.Log(MaxRectSize);

                    MaxRectSize_LeftUpper = new Vector2(-Mathf.Abs(MaxRectSize.x) / 2 +21f, Mathf.Abs(MaxRectSize.y) / 2 - 21f);
                    MaxRectSize_RightUpper = new Vector2(Mathf.Abs(MaxRectSize.x) / 2 -21f, Mathf.Abs(MaxRectSize.y) / 2 - 21f);
                    MaxRectSize_LeftLower = new Vector2(-Mathf.Abs(MaxRectSize.x) / 2 + 21f, -Mathf.Abs(MaxRectSize.y) / 2 + 21f);
                    MaxRectSize_RightLower = new Vector2(Mathf.Abs(MaxRectSize.x) / 2 -21f, -Mathf.Abs(MaxRectSize.y) / 2 + 21f);


                    MaxRectSize_LeftUpper_Mask = new Vector2(-Mathf.Abs(MaxRectSize.x) / 2 +26f, Mathf.Abs(MaxRectSize.y) / 2 -26f);
                    MaxRectSize_RightUpper_Mask = new Vector2(Mathf.Abs(MaxRectSize.x) / 2 - 26f, Mathf.Abs(MaxRectSize.y) / 2 - 26f);
                    MaxRectSize_LeftLower_Mask = new Vector2(-Mathf.Abs(MaxRectSize.x) / 2 + 26f, -Mathf.Abs(MaxRectSize.y) / 2 + 26f);
                    MaxRectSize_RightLower_Mask = new Vector2(Mathf.Abs(MaxRectSize.x) / 2 - 26f, -Mathf.Abs(MaxRectSize.y) / 2 + 26f);

                }
                
                #if UNITY_EDITOR
 
                clickPos = Input.mousePosition;  
                RectTransformUtility.ScreenPointToLocalPointInRectangle(showImageRect, clickPos, null, out localPoint);


                if(touchBigin)
                {
                    touchBigin = false;
                    offset = leftLowerRect.anchoredPosition - localPoint;
                }

                leftLowerPoints[1] = localPoint + offset;

                //Debug.Log(initialHandlePos[0] + " :: " +initialHandlePos[1] + " : : " + initialHandlePos[2] + " : : " + initialHandlePos[3]);

                if (leftLowerPoints[1].x <= MaxRectSize_LeftLower.x)
                {
                    leftLowerRect.anchoredPosition = new Vector2(MaxRectSize_LeftLower.x, leftLowerRect.anchoredPosition.y);
                    leftLowerUnMaskRect.anchoredPosition = new Vector2(MaxRectSize_LeftLower_Mask.x, leftLowerUnMaskRect.anchoredPosition.y);
                    rectImageRect.offsetMin = new Vector2(rectImageRectInitial[0].x, rectImageRect.offsetMin.y);
                    rectImageUnMaskRect.offsetMin = new Vector2(rectImageUnMaskRectInitial[0].x, rectImageUnMaskRect.offsetMin.y);
                    unMaskRect.offsetMin = new Vector2(unMaskRectInitial[0].x, unMaskRect.offsetMin.y);
                    leftUpperRect.anchoredPosition = new Vector2(MaxRectSize_LeftUpper.x, leftUpperRect.anchoredPosition.y);
                    leftUpperUnMaskRect.anchoredPosition = new Vector2(MaxRectSize_LeftUpper_Mask.x, leftUpperUnMaskRect.anchoredPosition.y);

                    leftLowerPoints[1].x = initialHandlePos[2].x;
                    leftLowerPoints[0].x = leftLowerPoints[1].x;
                    isReachingLimitX = true;
                }
                
                if(leftLowerPoints[1].y <= MaxRectSize_LeftLower.y)
                {
                    leftLowerRect.anchoredPosition = new Vector2(leftLowerRect.anchoredPosition.x, MaxRectSize_LeftLower.y);
                    leftLowerUnMaskRect.anchoredPosition = new Vector2(leftLowerUnMaskRect.anchoredPosition.x, MaxRectSize_LeftLower_Mask.y);
                    rectImageRect.offsetMin = new Vector2(rectImageRect.offsetMin.x, rectImageRectInitial[0].y);
                    rectImageUnMaskRect.offsetMin = new Vector2(rectImageUnMaskRect.offsetMin.x, rectImageUnMaskRectInitial[0].y);
                    unMaskRect.offsetMin = new Vector2(unMaskRect.offsetMin.x, unMaskRectInitial[0].y);
                    rightLowerRect.anchoredPosition = new Vector2(rightLowerRect.anchoredPosition.x, MaxRectSize_LeftLower.y);
                    rightLowerUnMaskRect.anchoredPosition = new Vector2(rightLowerUnMaskRect.anchoredPosition.x, MaxRectSize_LeftLower_Mask.y);

                    leftLowerPoints[1].y = initialHandlePos[2].y;
                    leftLowerPoints[0].y = leftLowerPoints[1].y;
                    isReachingLimitY = true;
                }
                

                if((rightLowerRect.anchoredPosition.x - leftLowerPoints[1].x) <= handleSpaceThereshold)
                {
                    leftLowerPoints[1].x = rightLowerRect.anchoredPosition.x - handleSpaceThereshold;
                    leftLowerPoints[0].x = leftLowerPoints[1].x;
                    isReachingLimitX = true;
                }
                
                if((leftUpperRect.anchoredPosition.y - leftLowerPoints[1].y) <= handleSpaceThereshold)
                {
                    leftLowerPoints[1].y = leftUpperRect.anchoredPosition.y + handleSpaceThereshold;
                    leftLowerPoints[0].y = leftLowerPoints[1].y;
                    isReachingLimitY = true;
                }
                
                amountX = (leftLowerPoints[1].x - leftLowerPoints[0].x);
                amountY = (leftLowerPoints[1].y - leftLowerPoints[0].y);

                unMaskRect.offsetMin = new Vector2(unMaskRect.offsetMin.x + amountX, unMaskRect.offsetMin.y + amountY);
                leftLowerUnMaskRect.anchoredPosition += new Vector2(amountX, amountY);

                rectImageRect.offsetMin = new Vector2(rectImageRect.offsetMin.x + amountX, rectImageRect.offsetMin.y + amountY);
                rectImageUnMaskRect.offsetMin = new Vector2(rectImageUnMaskRect.offsetMin.x + amountX, rectImageUnMaskRect.offsetMin.y + amountY);
                
                rightLowerRect.anchoredPosition += new Vector2(0f, amountY);
                rightLowerUnMaskRect.anchoredPosition += new Vector2(0f, amountY);
                
                leftUpperRect.anchoredPosition += new Vector2(amountX, 0f);
                leftUpperUnMaskRect.anchoredPosition += new Vector2(amountX, 0f);


#else

                if (Input.touchCount > 0)  
                {
                    var touch = Input.GetTouch(0);  
                    
                    touchPos = touch.position;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(showImageRect, touchPos, null, out localPoint);

                    if(touchBigin)
                    {
                        touchBigin = false;
                        offset = leftLowerRect.anchoredPosition - localPoint;
                    }

                    leftLowerPoints[1] = localPoint + offset;

                    
                    if (leftLowerPoints[1].x <= MaxRectSize_LeftLower.x)
                    {
                        leftLowerRect.anchoredPosition = new Vector2(MaxRectSize_LeftLower.x, leftLowerRect.anchoredPosition.y);
                        leftLowerUnMaskRect.anchoredPosition = new Vector2(MaxRectSize_LeftLower_Mask.x, leftLowerUnMaskRect.anchoredPosition.y);
                        rectImageRect.offsetMin = new Vector2(rectImageRectInitial[0].x, rectImageRect.offsetMin.y);
                        rectImageUnMaskRect.offsetMin = new Vector2(rectImageUnMaskRectInitial[0].x, rectImageUnMaskRect.offsetMin.y);
                        unMaskRect.offsetMin = new Vector2(unMaskRectInitial[0].x, unMaskRect.offsetMin.y);
                        leftUpperRect.anchoredPosition = new Vector2(MaxRectSize_LeftUpper.x, leftUpperRect.anchoredPosition.y);
                        leftUpperUnMaskRect.anchoredPosition = new Vector2(MaxRectSize_LeftUpper_Mask.x, leftUpperUnMaskRect.anchoredPosition.y);

                        leftLowerPoints[1].x = initialHandlePos[2].x;
                        leftLowerPoints[0].x = leftLowerPoints[1].x;
                        isReachingLimitX = true;
                    }
                
                    if(leftLowerPoints[1].y <= MaxRectSize_LeftLower.y)
                    {
                        leftLowerRect.anchoredPosition = new Vector2(leftLowerRect.anchoredPosition.x, MaxRectSize_LeftLower.y);
                        leftLowerUnMaskRect.anchoredPosition = new Vector2(leftLowerUnMaskRect.anchoredPosition.x, MaxRectSize_LeftLower_Mask.y);
                        rectImageRect.offsetMin = new Vector2(rectImageRect.offsetMin.x, rectImageRectInitial[0].y);
                        rectImageUnMaskRect.offsetMin = new Vector2(rectImageUnMaskRect.offsetMin.x, rectImageUnMaskRectInitial[0].y);
                        unMaskRect.offsetMin = new Vector2(unMaskRect.offsetMin.x, unMaskRectInitial[0].y);
                        rightLowerRect.anchoredPosition = new Vector2(rightLowerRect.anchoredPosition.x, MaxRectSize_LeftLower.y);
                        rightLowerUnMaskRect.anchoredPosition = new Vector2(rightLowerUnMaskRect.anchoredPosition.x, MaxRectSize_LeftLower_Mask.y);

                        leftLowerPoints[1].y = initialHandlePos[2].y;
                        leftLowerPoints[0].y = leftLowerPoints[1].y;
                        isReachingLimitY = true;
                    }
                    

                    if((rightLowerRect.anchoredPosition.x - leftLowerPoints[1].x) <= handleSpaceThereshold)
                    {
                        leftLowerPoints[1].x = rightLowerRect.anchoredPosition.x - handleSpaceThereshold;
                        leftLowerPoints[0].x = leftLowerPoints[1].x;
                        isReachingLimitX = true;
                    }
                    
                    if((leftUpperRect.anchoredPosition.y - leftLowerPoints[1].y) <= handleSpaceThereshold)
                    {
                        leftLowerPoints[1].y = leftUpperRect.anchoredPosition.y + handleSpaceThereshold;
                        leftLowerPoints[0].y = leftLowerPoints[1].y;
                        isReachingLimitY = true;
                    }

                    amountX = (leftLowerPoints[1].x - leftLowerPoints[0].x);
                    amountY = (leftLowerPoints[1].y - leftLowerPoints[0].y);

                    unMaskRect.offsetMin = new Vector2(unMaskRect.offsetMin.x + amountX, unMaskRect.offsetMin.y + amountY);
                    leftLowerUnMaskRect.anchoredPosition += new Vector2(amountX, amountY);

                    rectImageRect.offsetMin = new Vector2(rectImageRect.offsetMin.x + amountX, rectImageRect.offsetMin.y + amountY);
                    rectImageUnMaskRect.offsetMin = new Vector2(rectImageUnMaskRect.offsetMin.x + amountX, rectImageUnMaskRect.offsetMin.y + amountY);

                    rightLowerRect.anchoredPosition += new Vector2(0f, amountY);
                    rightLowerUnMaskRect.anchoredPosition += new Vector2(0f, amountY);
                    
                    leftUpperRect.anchoredPosition += new Vector2(amountX, 0f);
                    leftUpperUnMaskRect.anchoredPosition += new Vector2(amountX, 0f);
                    
                }
                
#endif

                    if (isReachingLimitX)
                {
                    leftLowerRect.anchoredPosition += new Vector2(0f, amountY);
                }
                else if(isReachingLimitY)
                {
                    leftLowerRect.anchoredPosition += new Vector2(amountX, 0f);
                }
                else
                {
                    leftLowerRect.anchoredPosition += new Vector2(amountX, amountY);
                }

                leftLowerPoints[0] = leftLowerRect.anchoredPosition;
                break;
            }
            case "rightLower":
            {
                if(touchBigin)
                {
                    rightLowerRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    rightLowerUnMaskRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    rightUpperRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    rightUpperUnMaskRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    leftLowerRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    leftLowerUnMaskRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    
                    rightLowerPoints[0] = rightLowerRect.anchoredPosition;

                    
/*                    if(initialHandlePos[3].x == 0f) initialHandlePos[3].x = rightLowerRect.anchoredPosition.x;
                    if(initialHandlePos[3].y == 0f) initialHandlePos[3].y = rightLowerRect.anchoredPosition.y;
                    if(initialHandleUnMaskPos[3].x == 0f) initialHandleUnMaskPos[3].x = rightLowerUnMaskRect.anchoredPosition.x;
                    if(initialHandleUnMaskPos[3].y == 0f) initialHandleUnMaskPos[3].y = rightLowerUnMaskRect.anchoredPosition.y;

                    if(initialHandlePos[1].x == 0f) initialHandlePos[1].x = rightLowerRect.anchoredPosition.x;
                    if(initialHandleUnMaskPos[1].x == 0f) initialHandleUnMaskPos[1].x = rightLowerUnMaskRect.anchoredPosition.x;
                    if(initialHandlePos[2].y == 0f) initialHandlePos[2].y = rightLowerRect.anchoredPosition.y;
                    if(initialHandleUnMaskPos[2].y == 0f) initialHandleUnMaskPos[2].y = rightLowerUnMaskRect.anchoredPosition.y;*/


/*                        initialHandlePos[0] = Vector2.zero;
                        initialHandleUnMaskPos[0] = Vector2.zero;

                        initialHandlePos[3].x = -initialHandlePos[3].x;
                        initialHandleUnMaskPos[3].x = -initialHandleUnMaskPos[3].x;*/

                    originalImageRect.anchorMin = new Vector2(0, 0);
                    originalImageRect.anchorMax = new Vector2(1, 1);

                    // 親オブジェクトにStretchするようにサイズを変更
                    originalImageRect.offsetMin = new Vector2(0, 0);
                    originalImageRect.offsetMax = new Vector2(0, 0);

                    originalImageRect.SetAnchorWithKeepingPosition(0.5f, 0.5f);
                    MaxRectSize = originalImageRect.sizeDelta;
                    Debug.Log(MaxRectSize);

                    MaxRectSize_LeftUpper = new Vector2(-Mathf.Abs(MaxRectSize.x) / 2 +21f, Mathf.Abs(MaxRectSize.y) / 2 - 21f);
                    MaxRectSize_RightUpper = new Vector2(Mathf.Abs(MaxRectSize.x) / 2 -21f, Mathf.Abs(MaxRectSize.y) / 2 - 21f);
                    MaxRectSize_LeftLower = new Vector2(-Mathf.Abs(MaxRectSize.x) / 2 + 21f, -Mathf.Abs(MaxRectSize.y) / 2 + 21f);
                    MaxRectSize_RightLower = new Vector2(Mathf.Abs(MaxRectSize.x) / 2 -21f, -Mathf.Abs(MaxRectSize.y) / 2 + 21f);


                    MaxRectSize_LeftUpper_Mask = new Vector2(-Mathf.Abs(MaxRectSize.x) / 2 +26f, Mathf.Abs(MaxRectSize.y) / 2 -26f);
                    MaxRectSize_RightUpper_Mask = new Vector2(Mathf.Abs(MaxRectSize.x) / 2 - 26f, Mathf.Abs(MaxRectSize.y) / 2 - 26f);
                    MaxRectSize_LeftLower_Mask = new Vector2(-Mathf.Abs(MaxRectSize.x) / 2 + 26f, -Mathf.Abs(MaxRectSize.y) / 2 + 26f);
                    MaxRectSize_RightLower_Mask = new Vector2(Mathf.Abs(MaxRectSize.x) / 2 - 26f, -Mathf.Abs(MaxRectSize.y) / 2 + 26f);



                    }

#if UNITY_EDITOR

                    clickPos = Input.mousePosition;  
                RectTransformUtility.ScreenPointToLocalPointInRectangle(showImageRect, clickPos, null, out localPoint);

                if(touchBigin)
                {
                    touchBigin = false;
                    offset = rightLowerRect.anchoredPosition - localPoint;
                }

                rightLowerPoints[1] = localPoint + offset;

               // Debug.Log(initialHandlePos[0] + " :: " +initialHandlePos[1] + " : : " + initialHandlePos[2] + " : : " + initialHandlePos[3]);

                if ( rightLowerPoints[1].x >= MaxRectSize_RightLower.x)
                {
                    rightLowerRect.anchoredPosition = new Vector2(MaxRectSize_RightLower.x, rightLowerRect.anchoredPosition.y);
                    rightLowerUnMaskRect.anchoredPosition = new Vector2(MaxRectSize_RightLower_Mask.x, rightLowerUnMaskRect.anchoredPosition.y);
                    rectImageRect.offsetMax = new Vector2(rectImageRectInitial[1].x, rectImageRect.offsetMax.y);
                    rectImageUnMaskRect.offsetMax = new Vector2(rectImageUnMaskRectInitial[1].x, rectImageUnMaskRect.offsetMax.y);
                    unMaskRect.offsetMax = new Vector2(unMaskRectInitial[1].x, unMaskRect.offsetMax.y);
                    rightUpperRect.anchoredPosition = new Vector2(MaxRectSize_RightUpper.x, rightUpperRect.anchoredPosition.y);
                    rightUpperUnMaskRect.anchoredPosition = new Vector2(MaxRectSize_RightUpper_Mask.x, rightUpperUnMaskRect.anchoredPosition.y);

                    rightLowerPoints[1].x = initialHandlePos[3].x;
                    rightLowerPoints[0].x = rightLowerPoints[1].x;
                    isReachingLimitX = true;
                }
                
                if(rightLowerPoints[1].y <= MaxRectSize_RightLower.y)
                {
                    rightLowerRect.anchoredPosition = new Vector2(rightLowerRect.anchoredPosition.x, MaxRectSize_RightLower.y);
                    rightLowerUnMaskRect.anchoredPosition = new Vector2(rightLowerUnMaskRect.anchoredPosition.x, MaxRectSize_RightLower_Mask.y);
                    rectImageRect.offsetMin = new Vector2(rectImageRect.offsetMin.x, rectImageRectInitial[0].y);
                    rectImageUnMaskRect.offsetMin = new Vector2(rectImageUnMaskRect.offsetMin.x, rectImageUnMaskRectInitial[0].y);
                    unMaskRect.offsetMin = new Vector2(unMaskRect.offsetMin.x, unMaskRectInitial[0].y);
                    leftLowerRect.anchoredPosition = new Vector2(leftLowerRect.anchoredPosition.x, MaxRectSize_LeftLower.y);
                    leftLowerUnMaskRect.anchoredPosition = new Vector2(leftLowerUnMaskRect.anchoredPosition.x, MaxRectSize_LeftLower_Mask.y);

                    rightLowerPoints[1].y = initialHandlePos[3].y;
                    rightLowerPoints[0].y = rightLowerPoints[1].y;
                    isReachingLimitY = true;
                }
                

                if((rightLowerPoints[1].x - leftLowerRect.anchoredPosition.x) <= handleSpaceThereshold)
                {
                    rightLowerPoints[1].x = leftLowerRect.anchoredPosition.x + handleSpaceThereshold;
                    rightLowerPoints[0].x = rightLowerPoints[1].x;
                    isReachingLimitX = true;
                }
                
                if((rightUpperRect.anchoredPosition.y - rightLowerPoints[1].y) <= handleSpaceThereshold)
                {
                    rightLowerPoints[1].y = rightUpperRect.anchoredPosition.y + handleSpaceThereshold;
                    rightLowerPoints[0].y = rightLowerPoints[1].y;
                    isReachingLimitY = true;
                }
    
                amountX = (rightLowerPoints[1].x - rightLowerPoints[0].x);
                amountY = (rightLowerPoints[1].y - rightLowerPoints[0].y);

                unMaskRect.offsetMax = new Vector2(unMaskRect.offsetMax.x + amountX, unMaskRect.offsetMax.y);
                unMaskRect.offsetMin = new Vector2(unMaskRect.offsetMin.x, unMaskRect.offsetMin.y + amountY);
                rightLowerUnMaskRect.anchoredPosition += new Vector2(amountX, amountY);

                rectImageRect.offsetMax = new Vector2(rectImageRect.offsetMax.x + amountX, rectImageRect.offsetMax.y);
                rectImageRect.offsetMin = new Vector2(rectImageRect.offsetMin.x, rectImageRect.offsetMin.y + amountY);
                rectImageUnMaskRect.offsetMax = new Vector2(rectImageUnMaskRect.offsetMax.x + amountX, rectImageUnMaskRect.offsetMax.y);
                rectImageUnMaskRect.offsetMin = new Vector2(rectImageUnMaskRect.offsetMin.x, rectImageUnMaskRect.offsetMin.y + amountY);

                leftLowerRect.anchoredPosition += new Vector2(0f, amountY);
                leftLowerUnMaskRect.anchoredPosition += new Vector2(0f, amountY);

                rightUpperRect.anchoredPosition += new Vector2(amountX, 0f);
                rightUpperUnMaskRect.anchoredPosition += new Vector2(amountX, 0f);

#else

                if (Input.touchCount > 0)  
                {
                    var touch = Input.GetTouch(0);  
                    
                    touchPos = touch.position;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(showImageRect, touchPos, null, out localPoint);

                    if(touchBigin)
                    {
                        touchBigin = false;
                        offset = rightLowerRect.anchoredPosition - localPoint;
                    }

                    rightLowerPoints[1] = localPoint + offset;

                    
                    if ( rightLowerPoints[1].x >= MaxRectSize_RightLower.x)
                    {
                        rightLowerRect.anchoredPosition = new Vector2(MaxRectSize_RightLower.x, rightLowerRect.anchoredPosition.y);
                        rightLowerUnMaskRect.anchoredPosition = new Vector2(MaxRectSize_RightLower_Mask.x, rightLowerUnMaskRect.anchoredPosition.y);
                        rectImageRect.offsetMax = new Vector2(rectImageRectInitial[1].x, rectImageRect.offsetMax.y);
                        rectImageUnMaskRect.offsetMax = new Vector2(rectImageUnMaskRectInitial[1].x, rectImageUnMaskRect.offsetMax.y);
                        unMaskRect.offsetMax = new Vector2(unMaskRectInitial[1].x, unMaskRect.offsetMax.y);
                        rightUpperRect.anchoredPosition = new Vector2(MaxRectSize_RightUpper.x, rightUpperRect.anchoredPosition.y);
                        rightUpperUnMaskRect.anchoredPosition = new Vector2(MaxRectSize_RightUpper_Mask.x, rightUpperUnMaskRect.anchoredPosition.y);

                        rightLowerPoints[1].x = initialHandlePos[3].x;
                        rightLowerPoints[0].x = rightLowerPoints[1].x;
                        isReachingLimitX = true;
                    }
                
                    if(rightLowerPoints[1].y <= MaxRectSize_RightLower.y)
                    {
                        rightLowerRect.anchoredPosition = new Vector2(rightLowerRect.anchoredPosition.x, MaxRectSize_RightLower.y);
                        rightLowerUnMaskRect.anchoredPosition = new Vector2(rightLowerUnMaskRect.anchoredPosition.x, MaxRectSize_RightLower_Mask.y);
                        rectImageRect.offsetMin = new Vector2(rectImageRect.offsetMin.x, rectImageRectInitial[0].y);
                        rectImageUnMaskRect.offsetMin = new Vector2(rectImageUnMaskRect.offsetMin.x, rectImageUnMaskRectInitial[0].y);
                        unMaskRect.offsetMin = new Vector2(unMaskRect.offsetMin.x, unMaskRectInitial[0].y);
                        leftLowerRect.anchoredPosition = new Vector2(leftLowerRect.anchoredPosition.x, MaxRectSize_LeftLower.y);
                        leftLowerUnMaskRect.anchoredPosition = new Vector2(leftLowerUnMaskRect.anchoredPosition.x, MaxRectSize_LeftLower_Mask.y);

                        rightLowerPoints[1].y = initialHandlePos[3].y;
                        rightLowerPoints[0].y = rightLowerPoints[1].y;
                        isReachingLimitY = true;
                    }
                    

                    if((rightLowerPoints[1].x - leftLowerRect.anchoredPosition.x) <= handleSpaceThereshold)
                    {
                        rightLowerPoints[1].x = leftLowerRect.anchoredPosition.x + handleSpaceThereshold;
                        rightLowerPoints[0].x = rightLowerPoints[1].x;
                        isReachingLimitX = true;
                    }
                    
                    if((rightUpperRect.anchoredPosition.y - rightLowerPoints[1].y) <= handleSpaceThereshold)
                    {
                        rightLowerPoints[1].y = rightUpperRect.anchoredPosition.y + handleSpaceThereshold;
                        rightLowerPoints[0].y = rightLowerPoints[1].y;
                        isReachingLimitY = true;
                    }

                    amountX = (rightLowerPoints[1].x - rightLowerPoints[0].x);
                    amountY = (rightLowerPoints[1].y - rightLowerPoints[0].y);

                    unMaskRect.offsetMax = new Vector2(unMaskRect.offsetMax.x + amountX, unMaskRect.offsetMax.y);
                    unMaskRect.offsetMin = new Vector2(unMaskRect.offsetMin.x, unMaskRect.offsetMin.y + amountY);
                    rightLowerUnMaskRect.anchoredPosition += new Vector2(amountX, amountY);

                    rectImageRect.offsetMax = new Vector2(rectImageRect.offsetMax.x + amountX, rectImageRect.offsetMax.y);
                    rectImageRect.offsetMin = new Vector2(rectImageRect.offsetMin.x, rectImageRect.offsetMin.y + amountY);
                    rectImageUnMaskRect.offsetMax = new Vector2(rectImageUnMaskRect.offsetMax.x + amountX, rectImageUnMaskRect.offsetMax.y);
                    rectImageUnMaskRect.offsetMin = new Vector2(rectImageUnMaskRect.offsetMin.x, rectImageUnMaskRect.offsetMin.y + amountY);

                    leftLowerRect.anchoredPosition += new Vector2(0f, amountY);
                    leftLowerUnMaskRect.anchoredPosition += new Vector2(0f, amountY);

                    rightUpperRect.anchoredPosition += new Vector2(amountX, 0f);
                    rightUpperUnMaskRect.anchoredPosition += new Vector2(amountX, 0f);

                }
                
#endif

                    if (isReachingLimitX)
                {
                    rightLowerRect.anchoredPosition += new Vector2(0f, amountY);
                }
                else if(isReachingLimitY)
                {
                    rightLowerRect.anchoredPosition += new Vector2(amountX, 0f);
                }
                else
                {
                    rightLowerRect.anchoredPosition += new Vector2(amountX, amountY);
                }

                rightLowerPoints[0] = rightLowerRect.anchoredPosition;
                break;
            }
        }
    }

    public void OnAdjustmentObjectDragEnd()
    {
        touchBigin = true;
    }

    private void SaveInitialRectTransformValue()
    {
        unMaskRectInitial[0] = unMaskRect.offsetMin;
        unMaskRectInitial[1] = unMaskRect.offsetMax;

        leftUpperRectInitial[0] = leftUpperRect.anchoredPosition;
        leftUpperRectInitial[1] = leftUpperRect.sizeDelta;
        leftUpperUnMaskRectInitial[0] = leftUpperUnMaskRect.anchoredPosition;
        leftUpperUnMaskRectInitial[1] = leftUpperUnMaskRect.sizeDelta;

        rightUpperRectInitial[0] = rightUpperRect.anchoredPosition;
        rightUpperRectInitial[1] = rightUpperRect.sizeDelta;
        rightUpperUnMaskRectInitial[0] = rightUpperUnMaskRect.anchoredPosition;
        rightUpperUnMaskRectInitial[1] = rightUpperUnMaskRect.sizeDelta;

        leftLowerRectInitial[0] = leftLowerRect.anchoredPosition;
        leftLowerRectInitial[1] = leftLowerRect.sizeDelta;
        leftLowerUnMaskRectInitial[0] = leftLowerUnMaskRect.anchoredPosition;
        leftLowerUnMaskRectInitial[1] = leftLowerUnMaskRect.sizeDelta;

        rightLowerRectInitial[0] = rightLowerRect.anchoredPosition;
        rightLowerRectInitial[1] = rightLowerRect.sizeDelta;
        rightLowerUnMaskRectInitial[0] = rightLowerUnMaskRect.anchoredPosition;
        rightLowerUnMaskRectInitial[1] = rightLowerUnMaskRect.sizeDelta;

        rectImageRectInitial[0] = rectImageRect.offsetMin;
        rectImageRectInitial[1] = rectImageRect.offsetMax;
        rectImageUnMaskRectInitial[0] = rectImageUnMaskRect.offsetMin;
        rectImageUnMaskRectInitial[1] = rectImageUnMaskRect.offsetMax;
    }
}
