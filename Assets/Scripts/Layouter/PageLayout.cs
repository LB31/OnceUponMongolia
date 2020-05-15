using System;
using System.Collections.Generic;
using UnityEngine;

namespace Layouter
{
    public class PageLayout : MonoBehaviour
    {
        private List<TriggeredFadein> _triggeredFadeins = new List<TriggeredFadein>();

        public void RegisterTriggeredFadein(TriggeredFadein fadein)
        {
            if (fadein == null || _triggeredFadeins.Contains(fadein))
                return;
            
            _triggeredFadeins.Add(fadein);
        }
        
        public void TriggerAll()
        {
            foreach (var layout in _triggeredFadeins)
            {
                layout.Trigger();
            }
        }
    }
}