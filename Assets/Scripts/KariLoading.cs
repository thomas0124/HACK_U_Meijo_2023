using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KariLoading : MonoBehaviour
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
    void Start()
    {
        StartCoroutine(enumerator());
    }

    void Update()
    {
        
    }

    IEnumerator enumerator()
    {
        getEnemyCard.SendSprite(cardInInventory.cardStatusList[cardInInventory.SelectCardId].creature);
        while (i <= 100)
        {
            loadingBar.GetComponent<Slider>().value = (float)(i / 100f);
            textText.SetText("{0}%", i);
            yield return new WaitForSeconds(0.02f);
            i++;
        }
        yield return new WaitForSeconds(2.0f);
        image.sprite = getEnemyCard.enemySprite;
        cardInInventory.enemySprite = getEnemyCard.enemySprite;
        loadScene.allowNextScene = true;
    }
}
