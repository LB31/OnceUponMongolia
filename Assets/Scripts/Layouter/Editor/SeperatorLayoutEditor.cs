using NUnit.Framework.Internal;
using UnityEditor;
using UnityEngine;

namespace Layouter
{
    [CustomEditor(typeof(SeperatorLayout))]
    public class SeperatorLayoutEditor : Editor
    {
        private SerializedProperty _verticalSeperationProperty;
        private SerializedProperty _seperationAspectRatioProperty;
        
        private SerializedProperty _child1TypeProperty;
        private SerializedProperty _child2TypeProperty;
        
        private GUIStyle _headerStyle;
        private GUIStyle _childStyle;

        private void OnEnable()
        {
            _verticalSeperationProperty = serializedObject.FindProperty("verticalSeperation");
            _seperationAspectRatioProperty = serializedObject.FindProperty("seperationAspectRatio");
            
            _child1TypeProperty = serializedObject.FindProperty("child1Type");
            _child2TypeProperty = serializedObject.FindProperty("child2Type");
            
            
            _headerStyle = new GUIStyle
            {
                fontStyle = FontStyle.Bold
            };

            _childStyle = new GUIStyle
            {
                normal = new GUIStyleState
                {
                    background = Texture2D.grayTexture
                },
                margin = new RectOffset(5, 5, 5, 5)
            };
        }
        
        public override void OnInspectorGUI()
        {
            GUILayout.Label("General", _headerStyle);
            EditorGUILayout.PropertyField(_verticalSeperationProperty, new GUIContent("Vertical Split"));
            EditorGUILayout.PropertyField(_seperationAspectRatioProperty, new GUIContent("Aspect Ratio"));
            
            GUILayout.Space(20f);
            
            if (GUILayout.Button("Update Connections")) ((SeperatorLayout) target).ConnectChilds();

            BeginControlGroup();
            
            ChildDrawer(1);
            ChildDrawer(2);
            
            EndControlGroup();

            serializedObject.ApplyModifiedProperties();
        }

        private void ChildDrawer(int index)
        {
            GUILayout.BeginVertical(_childStyle);
            GUILayout.Label("Child " + index, _headerStyle);
            EditorGUILayout.PropertyField(GetChildTypeProperty(index), new GUIContent("Type"));
            if (GUILayout.Button("Reset")) ((SeperatorLayout) target).ResetChild(index);
            if (GUILayout.Button("Jump To")) ((SeperatorLayout) target).JumpToChild(index);
            GUILayout.EndVertical();
        }

        private SerializedProperty GetChildTypeProperty(int index)
        {
            switch (index)
            {
                case 1: return _child1TypeProperty;
                case 2: return _child2TypeProperty;
                default: return null;
            }
        }

        private void EndControlGroup()
        {
            if (_verticalSeperationProperty.boolValue)
            {
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.EndVertical();
            }
        }

        private void BeginControlGroup()
        {
            if (_verticalSeperationProperty.boolValue)
            {
                GUILayout.BeginHorizontal();
            }
            else
            {
                GUILayout.BeginVertical();
            }
        }
    }
}