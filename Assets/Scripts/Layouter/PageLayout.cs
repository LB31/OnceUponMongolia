using System;
using System.Collections.Generic;
using Layouter.TriggeredAnimations;
using UnityEngine;

namespace Layouter
{
    public class PageLayout : MonoBehaviour
    {
        private readonly List<TriggeredAnimation> _triggeredAnimations = new List<TriggeredAnimation>();

        public void RegisterTriggeredFadein(TriggeredAnimation triggeredAnimation)
        {
            if (triggeredAnimation == null || _triggeredAnimations.Contains(triggeredAnimation))
                return;
            
            _triggeredAnimations.Add(triggeredAnimation);
        }
        
        public void TriggerAll()
        {
            foreach (var layout in _triggeredAnimations)
            {
                layout.Trigger();
            }
        }
    }
}