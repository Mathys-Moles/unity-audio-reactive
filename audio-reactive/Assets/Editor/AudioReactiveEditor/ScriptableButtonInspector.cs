using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(ScriptableObject), true)]
public class ScriptableButtonInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ScriptableObject so = (ScriptableObject)target;

        MethodInfo[] methods = so.GetType().GetMethods(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
        );

        foreach (MethodInfo method in methods)
        {
            if (method.GetCustomAttribute(typeof(OnEditButtonAttribute)) != null)
            {
                if (method.GetParameters().Length > 0)
                {
                    EditorGUILayout.HelpBox(
                        $"[OnEditButton] {method.Name} has parameters",
                        MessageType.Warning
                    );
                    continue;
                }

                if (GUILayout.Button(method.Name))
                {
                    method.Invoke(so, null);
                    EditorUtility.SetDirty(so);
                }
            }
        }
    }
}
