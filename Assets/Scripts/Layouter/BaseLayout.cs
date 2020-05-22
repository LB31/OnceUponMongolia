using UnityEngine;

namespace Layouter
{
    public class BaseLayout : MonoBehaviour
    {
        public virtual void Initialize()
        {
            FitTransform();
        }

        private void FitTransform()
        {
            var rectTransform = (RectTransform) transform;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }

        public virtual void UpdateLayout() { }
    }
}