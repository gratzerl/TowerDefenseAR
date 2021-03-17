using Assets.Scripts;
using UnityEditor;


/// <summary>
/// Editor for configuring mutiple enemies.
/// </summary>
[CustomEditor(typeof(EnemySpawner))]
public class EnemyStageConfigEditor : Editor
{
    SerializedProperty EnemyConfigs;

    void OnEnable()
    {
        EnemyConfigs = serializedObject.FindProperty(nameof(EnemyConfigs));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(EnemyConfigs);
        serializedObject.ApplyModifiedProperties();
    }
}
