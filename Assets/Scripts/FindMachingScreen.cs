using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMachingScreen : MonoBehaviour
{
    private GameObject main;
    private GameObject matchingScreen;
    [SerializeField]
    private ActiveButton activeButton;
    void Start()
    {
        main = GameObject.Find("5");
        matchingScreen = main.transform.Find("Matching Screen").gameObject;
        activeButton.gameObjects.Add(matchingScreen.transform);
    }
}
