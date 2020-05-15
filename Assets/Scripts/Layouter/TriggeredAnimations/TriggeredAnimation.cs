using Helper.SharedStates;
using UnityEngine;

namespace Layouter.TriggeredAnimations
{
    public class TriggeredAnimation : MonoBehaviour
    {
        [SerializeField] private SharedTrigger trigger = null;

        private void Awake()
        {
            if (trigger == null)
            {
                Debug.LogWarning($"SharedTrigger has not been assigned for {name}");
                return;
            }
            
            Register();

            if (trigger.Enabled)
                return;
            
            PreAnimation();
        }

        public void Trigger()
        {
            if (trigger.Enabled)
                return;

            if (trigger.Triggered)
            {
                PlayAnimation();
            }
        }

        protected virtual void PreAnimation()
        {
        }

        protected virtual void PlayAnimation()
        {
        }

        private void Register()
        {
            var pageLayout = transform.GetComponentInParent<PageLayout>();

            if (pageLayout != null)
            {
                pageLayout.RegisterTriggeredFadein(this);
            }
        }
    }
}