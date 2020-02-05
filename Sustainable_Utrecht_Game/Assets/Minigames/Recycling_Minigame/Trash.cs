using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour, IDraggable
{
    public TrashType type;
    private Rigidbody2D rb = null;
    [SerializeField] private float dragSpeedModifier = 0.5f;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Drop() {
    }

    public void Pick() {
       
    }

    public void UpdatePos(Vector2 pos) {
        if(rb != null)
            rb.velocity += ( pos - (Vector2) transform.position) * dragSpeedModifier;
    }

    public enum TrashType {
        Paper,
        Plastic,
        Green,
        Other
    }
}
