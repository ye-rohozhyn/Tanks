using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TankBaseInfo))]
public class EditorTankBaseInfo : Editor
{
    private TankBaseInfo _tankBaseInfo;

    private void OnEnable()
    {
        _tankBaseInfo = (TankBaseInfo)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();

        _tankBaseInfo.tankBaseSpeed = EditorGUILayout.FloatField("Movement speed", _tankBaseInfo.tankBaseSpeed);
        _tankBaseInfo.tankBaseRotationSpeed = EditorGUILayout.FloatField("Rotation speed", _tankBaseInfo.tankBaseRotationSpeed);
        _tankBaseInfo.tankBaseType = (TankBaseType)EditorGUILayout.EnumPopup("Tank base type", _tankBaseInfo.tankBaseType);

        if (_tankBaseInfo.tankBaseType == TankBaseType.Track)
        {
            _tankBaseInfo.animator = (Animator)EditorGUILayout.ObjectField("Animator", _tankBaseInfo.animator, typeof(Animator), true);
        }
        else if (_tankBaseInfo.tankBaseType == TankBaseType.Whells)
        {
            _tankBaseInfo.rotateAngle = EditorGUILayout.FloatField("Rotate angle", _tankBaseInfo.rotateAngle);

            var serializedObject = new SerializedObject(target);
            var property = serializedObject.FindProperty("frontWhells");
            serializedObject.Update();
            EditorGUILayout.PropertyField(property, true);
            serializedObject.ApplyModifiedProperties();
        }

        EditorGUILayout.EndVertical();
    }
}
