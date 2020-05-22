using System.Collections.Generic;
using Helper;
using Layouter.TriggeredAnimations;
using UnityEditor;
using UnityEngine;

namespace Layouter
{
    public class PageLayout : MonoBehaviour
    {
        [Header("Bookmark")] 
        [SerializeField] private bool isbookmarked = false;
        [SerializeField] private string bookmarkText = "";
        
        [Header("Page aspect ratio")]
        [SerializeField] private int x = 2;
        [SerializeField] private int y = 3;

        private readonly List<TriggeredAnimation> _triggeredAnimations = new List<TriggeredAnimation>();

        public void SetGameViewSize()
        {
            var sizeIndex = GameViewUtils.FindSize(GameViewSizeGroupType.Standalone, 2, 3);
            if (sizeIndex == -1)
            {
                GameViewUtils.AddCustomSize(GameViewUtils.GameViewSizeType.AspectRatio, GameViewSizeGroupType.Standalone, x, y, x + ":" + y);
                sizeIndex = GameViewUtils.FindSize(GameViewSizeGroupType.Standalone, 2, 3);
            }
            
            GameViewUtils.SetSize(sizeIndex);

            UpdateChildLayouts();
        }

        private void UpdateChildLayouts()
        {
            var layouts = GetComponentsInChildren<BaseLayout>();
            foreach (var layout in layouts)
            {
                layout.UpdateLayout();
            }
        }

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