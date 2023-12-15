using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShowStatusManager : MonoBehaviour
{
    public static ShowStatusManager Instance {get ; private set;}
    [SerializeField] private GameObject lineRendererCanvas;
    [SerializeField] private GameObject levelShows;
    [SerializeField] private GameObject statusShows;
    [SerializeField] private RawImage cardImage_level;
    [SerializeField] private RawImage cardRangeImage;
    [SerializeField] private RawImage cardImage_status;
    [SerializeField] private RawImage whiteOutImage;

    public string[] levelTextStrings;
    [SerializeField] private Text[] statusTexts;
    [SerializeField] private Text[] levelTexts = new Text[3];
    [SerializeField] private Text[] levelPercentTexts = new Text[3];

    private int[] chosenIndex= new int[3];
    private bool isLevelShowing {get; set;} = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void ShowLevel()
    {
        isLevelShowing = true;

        StartCoroutine(ShowLevelCoroutine());

    }

    private IEnumerator ShowLevelCoroutine()
    {
        for(int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(2.0f);
            // 表示するレベル文字列をランダムで決定
            int random = Random.Range(0, levelTextStrings.Length);
            // chosenIndexと重複していたらやり直し
            while(System.Array.IndexOf(chosenIndex, random) != -1)
            {
                random = Random.Range(0, levelTextStrings.Length);
            }

            levelTexts[i].text = levelTextStrings[random];

            int count = 0;
            while(count < 20)
            {
                int randomPercent = Random.Range(30, 100);
                levelPercentTexts[i].text = randomPercent.ToString() + "%";
                count++;

                yield return new WaitForSeconds(0.1f);
            }

            levelPercentTexts[i].text = Random.Range(30, 100).ToString() + "%";
        }

        yield return new WaitForSeconds(2.0f);

        isLevelShowing = false;
        
    }

    public IEnumerator ShowStatuses()
    {
        yield return new WaitUntil(() => isLevelShowing == false);

        // ステータスを取得
        var statusInfo = WebRequestManager.Instance.statusInfo;
        string type = statusInfo.type;
        int[] nums = {statusInfo.hp, statusInfo.attack, statusInfo.defense, statusInfo.specialAttack, statusInfo.specialDefense, statusInfo.speed};
        //ホワイトアウトする
        whiteOutImage.gameObject.SetActive(true);
        whiteOutImage.DOFade(1f, 3.0f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(5.0f);
        levelShows.SetActive(false);
        statusShows.SetActive(true);
        cardRangeImage.texture = cardImage_level.texture;
        cardRangeImage.gameObject.GetComponent<AspectRatioManager>().GetImage();
        cardRangeImage.texture = null;
        cardImage_status.texture = cardImage_level.texture;

        whiteOutImage.DOFade(0f, 3.0f).SetEase(Ease.Linear);
        
        for(int i = 0; i < statusTexts.Length; i++)
        {
            if(i == 0) statusTexts[i].text = type;
            else statusTexts[i].text = nums[i - 1].ToString();
        }
    }
}
