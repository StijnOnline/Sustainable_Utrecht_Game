using UnityEngine;
using System.Collections;

public class SetScreenOrientation : MonoBehaviour {

    [SerializeField] private ScreenOrientation screenOrientation;
    void Start() {

        Screen.orientation = screenOrientation;
    }
}
