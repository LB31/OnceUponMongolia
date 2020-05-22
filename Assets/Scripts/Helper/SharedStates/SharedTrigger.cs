using UnityEngine;

namespace Helper.SharedStates
{
    [CreateAssetMenu]
    public class SharedTrigger : ScriptableObject
    {
        [SerializeField] private bool initialEnabled = false;
        public bool Enabled { get; private set; }

        [SerializeField] private bool initialTriggered = false;
        private bool _triggered;
        public bool Triggered
        {
            get
            {
                if (_triggered)
                {
                    Enabled = true;
                }
                return _triggered;
            }
            private set
            {
                if (!_triggered)
                {
                    Enabled = false;
                }
                _triggered = value;
            }
        }

        private void OnEnable()
        {
            Enabled = initialEnabled;
            Triggered = initialTriggered;
        }
    }
}
