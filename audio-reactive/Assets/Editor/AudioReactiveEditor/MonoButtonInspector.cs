using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonoBehaviour), true)]
public class MonoButtonInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MonoBehaviour mono = (MonoBehaviour)target;
        MethodInfo[] methods = mono.GetType().GetMethods(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
        );

        foreach (MethodInfo method in methods)
        {
            if (method.GetCustomAttribute(typeof(OnEditButtonAttribute)) != null)
            {
                if (method.GetParameters().Length > 0)
                {
                    EditorGUILayout.HelpBox(
                        $"[OnEditButton] : {method.Name} Contain Parameter you can not use OnEditButton",
                        MessageType.Warning
                    );
                    continue;
                }

                if (GUILayout.Button(method.Name))
                {
                    method.Invoke(mono, null);

#if UNITY_EDITOR
                    EditorUtility.SetDirty(mono);
                    if (!Application.isPlaying)
                        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(mono.gameObject.scene);
#endif
                }
            }
        }
    }
}
