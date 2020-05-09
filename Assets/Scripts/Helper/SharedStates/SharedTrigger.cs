using System;
using UnityEngine;

namespace Helper.SharedStates
{
    [CreateAssetMenu]
    public class SharedTrigger : ScriptableObject
    {
        [SerializeField] private bool initialEnabled;
        public bool Enabled { get; private set; }

        [SerializeField] private bool initialTriggered;
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
            set
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
