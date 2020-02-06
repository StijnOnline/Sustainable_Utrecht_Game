using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSystem : MonoBehaviour {
    //Singleton Access
    private static TouchSystem _instance;
    public static TouchSystem Instance {
        get {
            if(_instance == null) {
                GameObject g = new GameObject("TouchSystem");
                _instance = g.AddComponent<TouchSystem>();
            }

            return _instance;
        }
    }

    [Header("Settings")]
    [SerializeField] private LayerMask tappableMask = 0;
    [SerializeField] private LayerMask draggableMask = 0;
    [SerializeField] private bool dragging = false;
    [SerializeField] private bool mouse = false;

    private IDraggable draggingObject;

    private void Awake() {
        if(_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    void Start() {

    }



    void Update() {
        if((!mouse && Input.touchCount > 0) || (mouse)) {
            Vector2 pos;
            Touch touch = new Touch();
            if(!mouse) {
                touch = Input.GetTouch(0);
                pos = touch.position;
            } else {
                pos = Input.mousePosition;
            }
            Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 100000));


            if((!mouse && touch.phase == TouchPhase.Began) || (mouse && Input.GetMouseButtonDown(0))) {
                Ray touchRay = new Ray(Vector3.forward, touchedPos);
                RaycastHit2D hit = Physics2D.Raycast(touchedPos, Vector2.zero, Mathf.Infinity, tappableMask);

                if(hit) {
                    Transform checking = hit.transform;
                    IClickable clickable = null;
                    do {
                        checking = checking.parent;
                        clickable = checking.GetComponent<IClickable>();

                    } while(clickable == null && checking.parent != null);


                    if(clickable != null)
                        clickable.Click();

                }

                if(dragging && draggingObject == null) {
                    hit = Physics2D.Raycast(touchedPos, Vector2.zero, 10, draggableMask);
                    if(hit) {
                        IDraggable draggable = hit.collider.GetComponent<IDraggable>();
                        if(draggable != null) {
                            draggingObject = draggable;
                            draggingObject.Pick();
                        }
                    }
                }

            }

            if((!mouse && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)) || (mouse && Input.GetMouseButtonUp(0))) {
                if(dragging && draggingObject != null) {
                    draggingObject.Drop();
                    draggingObject = null;
                }
            }

            if(dragging && draggingObject != null) {
                draggingObject.UpdatePos(touchedPos);
            }

        }




    }
}
