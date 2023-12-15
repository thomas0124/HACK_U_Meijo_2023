using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AspectRatioManager : MonoBehaviour
{
    private RawImage _image;

    public void GetImage()
    {
        _image = this.GetComponent<RawImage>();
        AspectRatio(_image);
    }
    
    public void AspectRatio(RawImage _image)
    {
        float aspectRatio = (float)_image.texture.width / (float)_image.texture.height;
        _image.GetComponent<AspectRatioFitter>().aspectRatio = aspectRatio;

    }
}
