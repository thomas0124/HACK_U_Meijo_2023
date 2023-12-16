using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class KariLoading : MonoBehaviourPunCallbacks
{
    int i = 0;
    public Transform loadingObject;
    [SerializeField]
    private TextMeshProUGUI textText;
    public GameObject loadingBar;
    public LoadScene loadScene;

    public CardInInventory cardInInventory;
    public GetEnemyCard getEnemyCard;
    public Image image;
    private bool flag = false;
    void Start()
    {
        StartCoroutine(enumerator());
    }

    void Update()
    {
        
    }

    IEnumerator enumerator()
    {
        float a = Time.time;
        getEnemyCard.SendSprite(cardInInventory.cardStatusList[cardInInventory.SelectCardId].creature);
        while (i <= 100)
        {
            loadingBar.GetComponent<Slider>().value = (float)(i / 100f);
            textText.SetText("{0}%", i);
            yield return new WaitForSeconds(0.02f);
            i++;
        }
        yield return new WaitForSeconds(5.0f);
        while (Time.time - a >= 10)
        {
            yield return null;
        }
        while (image.sprite == null)
        {
            image.sprite = getEnemyCard.enemySprite;
        }
        Debug.Log("a");
        photonView.RPC(nameof(CompLoad), RpcTarget.Others);
        cardInInventory.enemySprite = image.sprite;
        loadScene.allowNextScene = true;
    }
    [PunRPC]
    void CompLoad()
    {
        flag = true;
    }
}
