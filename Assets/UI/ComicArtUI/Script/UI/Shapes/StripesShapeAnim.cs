using UnityEngine;

namespace ComicUI
{
    public class StripesShapeAnim : ShapesAnim
    {
        public override void OnHoverAction()
        {
            ButtonShape.settings.gridSize =
                Mathf.Lerp(ButtonShape.settings.gridSize, MaxValue, Time.deltaTime * AnimationSpeed);
        }

        public override void OnHoverLostAction()
        {
            ButtonShape.settings.gridSize =
                Mathf.Lerp(ButtonShape.settings.gridSize, MinValue, Time.deltaTime * AnimationSpeed);
        }

        public override void OnClickAction()
        {
            base.OnClickAction();
        }


    }
}
