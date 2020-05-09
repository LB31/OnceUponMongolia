using System;
using System.Collections;
using Helper.SharedStates;
using UnityEngine;

namespace Layouter
{
    public class PageLayout : MonoBehaviour
    {
        [SerializeField] private SharedTrigger trigger = null;

        private CanvasGroup _contentParent;
        
        private void Awake()
        {
            _contentParent = transform.GetComponentInChildren<CanvasGroup>();
        }

        private void Start()
        {
            if (trigger.Enabled)
                return;

            _contentParent.alpha = 0;

            if (trigger.Triggered)
            {
                StartCoroutine(FadeInAnimation());
            }
        }

        private IEnumerator FadeInAnimation()
        {
            while (_contentParent.alpha < 1f)
            {
                _contentParent.alpha += 0.01f;
                yield return null;
            }
        }
    }
}