using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(CityChanger))]
public class CityChangerEditor : Editor {

    SerializedProperty cityVariables;
    private ReorderableList reorderableList;
    void OnEnable() {
        cityVariables = serializedObject.FindProperty("cityVariables");
        reorderableList = new ReorderableList(serializedObject, cityVariables, true, true, true, true);
        reorderableList.drawElementCallback = DrawElementCallback;
        reorderableList.elementHeightCallback = ElementHeightCallback;
        reorderableList.drawHeaderCallback = DrawHeaderCallback;
    }

    private void DrawHeaderCallback(Rect rect) {
        EditorGUI.LabelField(rect, "City Variable Objects");
    }

    private void DrawElementCallback(Rect rect, int index, bool isactive, bool isfocused) {
        //Get the element we want to draw from the list.
        SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;

        //We get the name property of our element so we can display this in our list.
        SerializedProperty elementName = element.FindPropertyRelative("_name");
        string elementTitle = string.IsNullOrEmpty(elementName.stringValue)
            ? "New"
            : elementName.stringValue;

        

        //Draw the list item as a property field, just like Unity does internally.
        EditorGUI.PropertyField(position:
            new Rect(rect.x += 10, rect.y, Screen.width * .8f, height: EditorGUIUtility.singleLineHeight), property:
            element, label: new GUIContent(elementTitle), includeChildren: true);
    }

    private float ElementHeightCallback(int index) {
        //Gets the height of the element. This also accounts for properties that can be expanded, like structs.
        float propertyHeight = EditorGUI.GetPropertyHeight(reorderableList.serializedProperty.GetArrayElementAtIndex(index), true);

        float spacing = EditorGUIUtility.singleLineHeight / 2;

        return propertyHeight + spacing;
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}

