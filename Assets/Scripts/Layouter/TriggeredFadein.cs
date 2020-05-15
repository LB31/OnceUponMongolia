using Helper.SharedStates;
using UnityEngine;
using UnityEngine.Events;

namespace Layouter
{
    [System.Serializable]
    public class AnimatedValueEvent : UnityEvent<int>
    {
    }
    
    public class TriggeredFadein : MonoBehaviour
    {
        [SerializeField] private SharedTrigger trigger = null;
        [SerializeField] private AnimatedValueEvent animatedValue;

        private void Awake()
        {
            if (trigger == null)
            {
                Debug.LogWarning($"SharedTrigger has not been assigned for {name}");
                return;
            }
            
            var pageLayout = transform.GetComponentInParent<PageLayout>();

            if (pageLayout != null)
            {
                pageLayout.RegisterTriggeredFadein(this);
            }
        }

        public void Trigger()
        {
            if (trigger.Enabled)
                return;

            if (trigger.Triggered)
            {
                //TODO: Animation
            }
        }
    }
}