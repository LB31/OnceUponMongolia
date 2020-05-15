using UnityEngine;

namespace Layouter.TriggeredAnimations
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TriggeredFadeIn : TriggeredAnimation
    {
        private CanvasGroup _canvasGroup;

        protected override void PreAnimation()
        {
            base.PreAnimation();
            
            _canvasGroup = GetComponent<CanvasGroup>();

            _canvasGroup.alpha = 0;
        }

        protected override void PlayAnimation()
        {
            base.PlayAnimation();
            
            LeanTween.alphaCanvas(_canvasGroup, 1f, 1f);
        }
    }
}