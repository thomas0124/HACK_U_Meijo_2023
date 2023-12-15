using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindButton : MonoBehaviour
{
    private GameObject presenter;
    private ButtonPresenter buttonPresenter;
    void Start()
    {
        presenter = GameObject.Find("Presenter");
        buttonPresenter = presenter.GetComponent<ButtonPresenter>();
        buttonPresenter.joinButton.Add(this.gameObject.GetComponent<Button>());
        buttonPresenter.makeJoinRoomButton();
        buttonPresenter.joinButton.Remove(this.gameObject.GetComponent<Button>());
    }
}
