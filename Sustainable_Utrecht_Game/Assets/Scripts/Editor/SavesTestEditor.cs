

using UnityEditor;
using UnityEngine;

namespace Assets.Scripts {
    class SavesTestEditor : EditorWindow {


        string labeltext;

    [MenuItem("Window/SaveTesting")]
        static void Init() {
            SavesTestEditor window = (SavesTestEditor)EditorWindow.GetWindow(typeof(SavesTestEditor));
        }


        void OnGUI() {
            if(GUILayout.Button("Test Save")) {
                SaveData s = new SaveData();
                s.SDGPoints[0] = 10;
                s.SDGPoints[2] = 20;
                s.SDGPoints[4] = 30;
                DataSaver.SaveData(s);
            }
            if(GUILayout.Button("Test Load")) {
                SaveData s = DataSaver.LoadData();
                labeltext= JsonUtility.ToJson(s);
            }
            GUILayout.Label(labeltext);
        }
    }
}
