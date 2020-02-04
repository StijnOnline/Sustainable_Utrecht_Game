using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSystem : MonoBehaviour
{
    //Singleton Access
    private static TouchSystem _instance;
    public static TouchSystem Instance {
        get {
            if (_instance == null)
            {
                GameObject g = new GameObject("TouchSystem");
                _instance = g.AddComponent<TouchSystem>();
            }

            return _instance;
        }
    }

    [Header("Settings")]
    [SerializeField] private LayerMask tappableMask = 0;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        
    }



    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);


            if (touch.phase == TouchPhase.Began)
            {
            Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
            Ray touchRay = new Ray(Vector3.forward, touchedPos);
            RaycastHit2D hit = Physics2D.Raycast(touchedPos, Vector2.zero, 10, tappableMask);

                if (hit)
                {
                    IClickable clickable = hit.collider.GetComponent<IClickable>();
                    clickable.Click();
                }

            }
        }
    }
}
