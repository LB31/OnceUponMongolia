using System;
using Helper.ClassTypeReference;
using UnityEditor;
using UnityEngine;

namespace Layouter
{
    public class SeperatorLayout : BaseLayout
    {
        private const string ChildNamePrefix = "Child ";
        private const int ChildCount = 2;
        
        [SerializeField] private bool verticalSeperation = false;
        [Range(0, 1)]
        [SerializeField] private float seperationAspectRatio = 0.5f;

        [ClassExtends(typeof(BaseLayout), Grouping = ClassGrouping.None)]
        [SerializeField] private ClassTypeReference child1Type = null;
        
        [ClassExtends(typeof(BaseLayout), Grouping = ClassGrouping.None)]
        [SerializeField] private ClassTypeReference child2Type = null;
        
        private RectTransform _childTransform1;
        private RectTransform _childTransform2;

        public override void Initialize()
        {
            base.Initialize();
            
            ConnectChilds();
        }

        public override void UpdateLayout()
        {
            base.UpdateLayout();
            
            ConnectChilds();
        }

        private void OnBecameVisible()
        {
            ConnectChilds();
        }

        private void OnValidate()
        {
            if (_childTransform1 == null || _childTransform2 == null)
                return;
            
            UpdateChildTransforms();
        }

        public void ConnectChilds()
        {
            for (var i = 1; i <= ChildCount; i++)
            {
                var child = GetChild(i);
                if (child == null)
                    child = CreateChild(ChildNamePrefix + i, GetChildType(i));
                
                SetChildTransform(i, child);
            }
            UpdateChildTransforms();
        }

        public void ResetChild(int index)
        {
            var child = GetChild(index);
            if(child != null) DestroyImmediate(child.gameObject);
            child = CreateChild(ChildNamePrefix + index, GetChildType(index));
            SetChildTransform(index , child);
            
            UpdateChildTransforms();
        }
        
        private void UpdateChildTransforms()
        {
            var invertedAspectRatio = 1 - seperationAspectRatio;

            _childTransform1.anchorMin = verticalSeperation
                ? Vector2.zero
                : new Vector2(0, invertedAspectRatio);
            _childTransform1.anchorMax = verticalSeperation
                ? new Vector2(seperationAspectRatio, 1)
                : Vector2.one;

            _childTransform2.anchorMin = verticalSeperation
                ? new Vector2(seperationAspectRatio, 0)
                : Vector2.zero;
            _childTransform2.anchorMax = verticalSeperation
                ? Vector2.one
                : new Vector2(1, invertedAspectRatio);
        }

        private RectTransform CreateChild(string childName, Type type)
        {
            var child = new GameObject(childName, typeof(RectTransform));
            var childTransform = child.transform as RectTransform;

            Debug.Assert(childTransform != null, nameof(childTransform) + " != null");
            childTransform.SetParent(transform);
            childTransform.offsetMin = Vector2.zero;
            childTransform.offsetMax = Vector2.zero;

            if (type != null)
            {
                child.AddComponent(type);
                var content = child.GetComponent<BaseLayout>();
                content.Initialize();
            }
        
            return childTransform;
        }

        private RectTransform GetChild(int index)
        {
            return (RectTransform) transform.Find(ChildNamePrefix + index);
        }

        private Type GetChildType(int index)
        {
            switch (index)
            {
                case 1: return child1Type.Type;
                case 2: return child2Type.Type;
                default: throw new IndexOutOfRangeException();
            }
        }

        private void SetChildTransform(int index, RectTransform newTransform)
        {
            switch (index)
            {
                case 1: _childTransform1 = newTransform;
                    break;
                case 2: _childTransform2 = newTransform;
                    break;
            }
        }

        public void JumpToChild(int index)
        {
            Selection.activeGameObject = GetChild(index).gameObject;
        }    
    }
}
