using UnityEngine;

class CameraPan : MonoBehaviour {

    private TouchSystem touchSystem;
    private Camera cam;
    Vector3 targetPos;

    private void Start() {
        touchSystem = TouchSystem.Instance;
        cam = GetComponent<Camera>();
        touchSystem.startTouch += StartPan;
    }

    private void Update() {
        if(touchSystem.touching) {
            transform.Translate(targetPos - cam.ScreenToWorldPoint(touchSystem.lastTouchedScreenPos));
        }
    }

    void StartPan(Vector2 pos) {
        targetPos = cam.ScreenToWorldPoint(pos);
    }
}