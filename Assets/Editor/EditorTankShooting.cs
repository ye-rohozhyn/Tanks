using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TankShooting))]
public class EditorTankShooting : Editor
{
    TankShooting tankShooting;

    private void OnEnable()
    {
        tankShooting = (TankShooting) target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();

        tankShooting.shootingType = (ShootingType) EditorGUILayout.EnumPopup("Shooting type", tankShooting.shootingType);
        tankShooting.lifeTime = EditorGUILayout.FloatField("Lifetime", tankShooting.lifeTime);

        if (tankShooting.shootingType == ShootingType.OneGun) 
        {
            tankShooting.shell = (Rigidbody) EditorGUILayout.ObjectField("Shell", tankShooting.shell, typeof(Rigidbody), true);
            tankShooting.startPosition = (Transform) EditorGUILayout.ObjectField("Start position", tankShooting.startPosition, typeof(Transform), true);
            tankShooting.power = EditorGUILayout.FloatField("Power", tankShooting.power);
        }
        else if (tankShooting.shootingType == ShootingType.ManyGuns)
        {
            //List shells
            var serializedObject = new SerializedObject(target);
            var property = serializedObject.FindProperty("shells");
            serializedObject.Update();
            EditorGUILayout.PropertyField(property, true);
            serializedObject.ApplyModifiedProperties();

            //List start positions
            serializedObject = new SerializedObject(target);
            property = serializedObject.FindProperty("startPositions");
            serializedObject.Update();
            EditorGUILayout.PropertyField(property, true);
            serializedObject.ApplyModifiedProperties();

            tankShooting.power = EditorGUILayout.FloatField("Power", tankShooting.power);
        }
        else if (tankShooting.shootingType == ShootingType.BallisticGun)
        {
            tankShooting.target = (Transform) EditorGUILayout.ObjectField("Target", tankShooting.target, typeof(Transform), true);
            tankShooting.shotAngle = EditorGUILayout.FloatField("Shot angle", tankShooting.shotAngle);
            tankShooting.atIntervals = EditorGUILayout.Toggle("At intervals", tankShooting.atIntervals);

            if (tankShooting.atIntervals)
            {
                tankShooting.interval = EditorGUILayout.FloatField("Interval", tankShooting.interval);
            }

            //List shells
            var serializedObject = new SerializedObject(target);
            var property = serializedObject.FindProperty("shells");
            serializedObject.Update();
            EditorGUILayout.PropertyField(property, true);
            serializedObject.ApplyModifiedProperties();

            //List start positions
            serializedObject = new SerializedObject(target);
            property = serializedObject.FindProperty("startPositions");
            serializedObject.Update();
            EditorGUILayout.PropertyField(property, true);
            serializedObject.ApplyModifiedProperties();
        }
        else if (tankShooting.shootingType == ShootingType.SplashGun)
        {
            tankShooting.splashEffect = (ParticleSystem) EditorGUILayout.ObjectField("Splash effect", tankShooting.splashEffect, typeof(ParticleSystem), true);
            tankShooting.length = EditorGUILayout.FloatField("Length", tankShooting.length);
            tankShooting.timeReload = EditorGUILayout.FloatField("Time reload", tankShooting.timeReload);
        }

        EditorGUILayout.EndVertical();
    }
}
