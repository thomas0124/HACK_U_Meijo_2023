using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveButton : MonoBehaviour
{
    [SerializeField]
    private bool active = true;
    public List<Transform> gameObjects = new List<Transform>();
    public void ControlActive()
    {
        if (active)
        {
            foreach (Transform item in gameObjects)
            {
                item.gameObject.SetActive(true);
            }
        }
        if (!active)
        {
            foreach (Transform item in gameObjects)
            {
                item.gameObject.SetActive(false);
            }
        }
    }
}
