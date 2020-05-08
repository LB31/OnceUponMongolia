using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraTargetAspect))]
public class CameraTargetAspectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var buttonPress = GUILayout.Button("Update Transform Size");
        if (buttonPress)
        {
            ((CameraTargetAspect) target).Recalculate();
        }
    }
}