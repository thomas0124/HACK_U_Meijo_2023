using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Photon.Pun;

public class AfterKariLoading : MonoBehaviour
{
    public Image vs;
    public Image fill;
    public Image background;
    public TextMeshProUGUI text;
    public Image loadingScreen;
    public TextMeshProUGUI waitingText;
    [SerializeField]
    public bool eComp;
    int i = 0;
    bool flag = true;
    SendPhotonMessage sendPhotonMessage;
    private void Awake()
    {
        sendPhotonMessage = GetComponent<SendPhotonMessage>();
    }
    private void Start()
    {
        StartCoroutine(enumerator());
    }
    private void OnEnable()
    {
        StartCoroutine(enumerator());
    }
    IEnumerator enumerator()
    {
        fill.DOFade(endValue: 0f, duration: 1f);
        background.DOFade(endValue: 0f, duration: 1f);
        text.DOFade(endValue: 0f, duration: 1f);
        yield return new WaitForSeconds(1.2f);
        vs.gameObject.SetActive(true);
        vs.transform.DOScale(new Vector3(1.8f, 1.8f, 0), 0.5f).SetEase(Ease.OutBounce);
        sendPhotonMessage.SendComp();
        while (!eComp)
        {
            ++i;
            yield return new WaitForSeconds(0.1f);
            if (i > 50 && flag)
            {
                flag = false;
                Debug.Log("a");
                sendPhotonMessage.reconnect();
                waitingText.DOFade(endValue: 1f, duration: 0.5f);
            }
        }
        yield return new WaitUntil(() => eComp);
        if (!flag)
        {
            waitingText.DOFade(endValue: 0f, duration: 0.2f);
        }
        yield return new WaitForSeconds(0.5f);
        loadingScreen.DOFade(endValue: 0f, duration: 1f);
        yield return new WaitForSeconds(0.2f);
        vs.DOFade(endValue: 0f, duration: 1.1f);
        gameObject.SetActive(false);
    }
}
