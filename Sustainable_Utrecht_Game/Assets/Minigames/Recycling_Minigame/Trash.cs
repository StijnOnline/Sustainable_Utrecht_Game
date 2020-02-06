using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour, IDraggable {
    public TrashType type;
    private Rigidbody2D rb = null;
    private Collider2D coll = null;
    [SerializeField] private float flickSpeedModifier = 5f;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    public void Drop() {
        coll.enabled = true;
    }

    public void Pick() {
        coll.enabled = false;
    }

    public void UpdatePos(Vector2 pos) {
        if(rb != null)
            rb.velocity += ( pos - (Vector2) transform.position) * flickSpeedModifier;
        transform.position = pos;
        //rb.position = pos;
    }

    public enum TrashType {
        Paper,
        Plastic,
        Green,
        Other
    }
}
