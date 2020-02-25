using UnityEngine;

class CameraPan : MonoBehaviour {

    private TouchSystem touchSystem;
    private Camera cam;
    Vector3 targetPos;
    [SerializeField] Vector2 XMinMax;
    [SerializeField] Vector2 YMinMax;
    float[] minmax = new float[2];
    
    private void Start() {
        touchSystem = TouchSystem.Instance;
        cam = GetComponent<Camera>();
        touchSystem.startTouch += StartPan;
    }

    private void Update() {
        if(touchSystem.touching) {
            Vector3 camPos = transform.position + targetPos - cam.ScreenToWorldPoint(touchSystem.lastTouchedScreenPos);
            camPos.x = Mathf.Clamp(camPos.x, XMinMax.x, XMinMax.y);
            camPos.y = Mathf.Clamp(camPos.y, YMinMax.x, YMinMax.y);
            transform.position = camPos;
        }
    }

    void StartPan(Vector2 pos) {
        targetPos = cam.ScreenToWorldPoint(pos);
    }
}