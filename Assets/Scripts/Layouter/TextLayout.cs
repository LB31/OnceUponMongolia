using TMPro;
using UnityEngine;

namespace Layouter
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextLayout : BaseLayout
    {
        private const string InitialText = "Example Text";
        private const float InitialTextSize = 1f;

        public override void Initialize()
        {
            base.Initialize();
            
            var textMesh = gameObject.GetComponent<TextMeshProUGUI>();
            textMesh.text = InitialText;
            textMesh.fontSize = InitialTextSize;
        }
    }
}
