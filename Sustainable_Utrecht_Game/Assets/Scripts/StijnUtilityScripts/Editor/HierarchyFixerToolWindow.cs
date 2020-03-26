using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HierarchyFixerToolWindow : EditorWindow {
    [MenuItem("Window/Hierarchy Fixer")]
    static void Init() {
        CenterChildrenToolWindow window = (CenterChildrenToolWindow)EditorWindow.GetWindow(typeof(CenterChildrenToolWindow));
        Debug.Log(StringStuff.findstem(new string[] {"test Boom","Boom 3 (Clone)","boomding"}));
    }

    void OnGUI() {

    }
}
