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
    [SerializeField] private GameObject imageChoiceButton;
    [SerializeField] private Text messageText;
    [SerializeField] private GameObject createCard;
    [SerializeField] private GameObject createButton;
    [SerializeField] private GameObject redoButton;

    public GameObject CreateButton => createButton;
    public GameObject RedoButton => redoButton;

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
                Texture2D texture = NativeGallery.LoadImageAtPath(path, 512);
                if( texture == null )
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                texture = createReadabeTexture2D(texture);

                handleRangeImage.gameObject.SetActive(true);
                createButton.SetActive(true);
                redoButton.SetActive(true);

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

        // メッセージを消去
        messageText.text = "";
        
    }
}
