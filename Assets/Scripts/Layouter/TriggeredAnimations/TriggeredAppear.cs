using UnityEngine;

namespace Layouter.TriggeredAnimations
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TriggeredAppear : TriggeredAnimation
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
            
            _canvasGroup.alpha = 1;
        }
    }
}