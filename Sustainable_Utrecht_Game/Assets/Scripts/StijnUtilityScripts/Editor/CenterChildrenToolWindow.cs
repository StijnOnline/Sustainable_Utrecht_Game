using System;
using UnityEngine;
using UnityEditor;


class CenterChildrenToolWindow : EditorWindow {

    private Transform parentObject;
    private Transform childObject;
    private Method method;

    enum Method { Center, Child }

    [MenuItem("Window/Center Children")]
    static void Init() {
        CenterChildrenToolWindow window = (CenterChildrenToolWindow)EditorWindow.GetWindow(typeof(CenterChildrenToolWindow));
    }

    private void OnEnable() {

    }

    void OnGUI() {
        //Parent Object Field
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Parent Object");
        parentObject = (Transform)EditorGUILayout.ObjectField(parentObject, typeof(Transform));
        if(GUILayout.Button("Selected")) {
            parentObject = Selection.activeTransform;
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Target Position");
        method = (Method)EditorGUILayout.EnumPopup(method);
        EditorGUILayout.EndHorizontal();

        EditorSeparator(5, 2);


        switch(method) {
            case Method.Center:

                GUILayout.Label("Moves parent to center of children");
                if(GUILayout.Button("Center")) {
                    //Calculate Center
                    Vector3 referencePosition = Vector3.zero;
                    foreach(Transform child in parentObject.transform) {
                        referencePosition += child.localPosition;
                    }
                    referencePosition /= parentObject.childCount;
                    Center(referencePosition);
                }
                break;
            case Method.Child:
                GUILayout.Label("Moves parent to child");
                //Child Object Field
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Child Object");
                childObject = (Transform)EditorGUILayout.ObjectField(childObject, typeof(Transform));
                if(GUILayout.Button("Selected")) {
                    childObject = Selection.activeTransform;
                }
                EditorGUILayout.EndHorizontal();

                if(GUILayout.Button("Center")) {
                    Center(childObject.localPosition);
                }
                break;
        }

    }

    void Center(Vector3 referencePosition) {
        for(int i = 0; i < parentObject.childCount; i++) {
            Transform child = parentObject.GetChild(i);
            Undo.RecordObject(child, "Centered");
            child.localPosition -= referencePosition;
        }
        Undo.RecordObject(parentObject, "Centered");
        parentObject.position += referencePosition;

    }

    public static void EditorSeparator(float padding, float height) {
        GUILayout.Space(padding);
        EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, height), Color.grey);
        GUILayout.Space(padding);
    }
}
