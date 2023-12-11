using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AspectKeeper : MonoBehaviour
{
    public new Camera camera;
    public Vector2 aspect;

    void Update()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float targetAspect = aspect.x / aspect.y;
        float changeAspect = targetAspect / screenAspect;

        Rect viewportRect = new Rect(0, 0, 1, 1);
        if (changeAspect < 1)
        {
            viewportRect.width = changeAspect;
            viewportRect.x = 0.5f - viewportRect.width * 0.5f;
        }
        else
        {
            viewportRect.height = 1 / changeAspect;
            viewportRect.y = 0.5f - viewportRect.height * 0.5f;
        }

        camera.rect = viewportRect;
    }
}
