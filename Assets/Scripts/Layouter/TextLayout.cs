using System;
using TMPro;
using UnityEngine;

namespace Layouter
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextLayout : BaseLayout
    {
        private const string InitialText = "Example Text";

        [SerializeField] private float relativeFontSize = 0.15f;

        private TextMeshProUGUI _textMesh = null;
        private TextMeshProUGUI TextMesh
        {
            get
            {
                if (_textMesh == null)
                {
                    _textMesh = gameObject.GetComponent<TextMeshProUGUI>();
                }

                return _textMesh;
            }
        }

        private void OnValidate()
        {
            CalculateFontSize();
        }

        private void Start()
        {
            CalculateFontSize();
        }

        public override void Initialize()
        {
            base.Initialize();
            
            TextMesh.text = InitialText;
            CalculateFontSize();
        }

        public override void UpdateLayout()
        {
            base.UpdateLayout();
            
            CalculateFontSize();
        }

        private void CalculateFontSize()
        {
            var parentCanvas = GetComponentInParent<Canvas>();

            if (parentCanvas == null)
            {
                return;
            }

            var rect = parentCanvas.GetComponent<RectTransform>().rect;
            if (Math.Abs(rect.width) < 0.01f || Math.Abs(rect.height) < 0.01f)
            {
                return;
            }
            
            var fullSize = Mathf.Sqrt(rect.width * rect.height);

            TextMesh.fontSize = fullSize * relativeFontSize;
        }
    }
}
