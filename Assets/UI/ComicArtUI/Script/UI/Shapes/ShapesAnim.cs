using DG.Tweening;
using Shapes2D;
using TMPro;
using UnityEngine;

namespace ComicUI
{
    public abstract class ShapesAnim : MonoBehaviour
    {
        public TextMeshProUGUI ButtonText;
        public Shape ButtonShape;

        public Color ActiveColor;
        public Color DisabledColor;

        public float MinValue = 30f;
        public float MaxValue = 40f;

        public float AnimationSpeed = 5f;

        [SerializeField] private bool _isHovering;

        [SerializeField] private float _defaultFontSize;

        private Sequence _sequence;


        private void Awake()
        {
            ButtonShape = GetComponent<Shape>();
            ButtonText = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            _defaultFontSize = ButtonText.fontSize;
        }

        private void Update()
        {
            if (ButtonShape.settings.fillType == FillType.None)
                return;
            if (_isHovering)
            {
                OnHoverAction();
            }
            else
            {
                OnHoverLostAction();
            }
        }


        public void PlayHoverAnimation()
        {
            _sequence = DOTween.Sequence();
            //_sequence.Join(ButtonText.DOFontSize(_defaultFontSize + 20f, 0.4f));
            _sequence.Join(ButtonText.rectTransform.DOScale(1.1f, 0.4f));
            _sequence.Join(ButtonText.DOFade(0.35f, 0.4f));
            _sequence.SetLoops(-1, LoopType.Yoyo);
            _sequence.Play();
        }

        public void ResetAnimation()
        {
            _sequence.Rewind();
            ButtonText.fontSize = _defaultFontSize;
            ButtonText.alpha = 1f;
        }


        public abstract void OnHoverAction();

        public abstract void OnHoverLostAction();

        public virtual void OnClickAction()
        {
            _isHovering = false;
            ResetAnimation();
        }

        public void OnHover()
        {
            _isHovering = true;
        }

        public void OnHoverLost()
        {
            _isHovering = false;
        }
    }
}
