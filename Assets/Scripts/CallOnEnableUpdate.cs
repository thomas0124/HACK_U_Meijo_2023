using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallOnEnableUpdate : MonoBehaviour
{
    public ContentCard contentCard;
    private void Start()
    {
        contentCard.CardUpdate();
    }
    private void OnEnable()
    {
        contentCard.CardUpdate();
    }
}
