using UnityEditor;
using UnityEngine;

namespace Layouter
{
    [CustomEditor(typeof(PageLayout))]
    public class PageLayoutEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (GUILayout.Button("Set Game View Size")) ((PageLayout) target).SetGameViewSize();
        }
    }
}