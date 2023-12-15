using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComicUI
{
    public class GradientShapeAnim : ShapesAnim
    {
        public override void OnHoverAction()
        {
            ButtonShape.settings.gradientStart = Mathf.Lerp(ButtonShape.settings.gradientStart, MaxValue,
                Time.deltaTime * AnimationSpeed);
        }

        public override void OnHoverLostAction()
        {
            if (ButtonShape.settings.gradientStart == MinValue)
                return;
            ButtonShape.settings.gradientStart = Mathf.Lerp(ButtonShape.settings.gradientStart, MinValue,
                Time.deltaTime * AnimationSpeed);
        }

        public override void OnClickAction()
        {
            base.OnClickAction();
        }
    }
}
