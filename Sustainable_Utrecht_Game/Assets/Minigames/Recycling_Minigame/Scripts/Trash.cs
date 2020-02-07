using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour, IDraggable {
    [SerializeField] private float flickSpeedModifier = 5f;
    private float targetSize = 2f;
    private Rigidbody2D rb = null;
    private CircleCollider2D coll = null;

    public TrashInfo trashInfo;

    public void Init() {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();
        Transform trail = GetComponentInChildren<TrailRenderer>().transform;

        SpriteRenderer r = GetComponent<SpriteRenderer>();
        r.sprite = trashInfo.sprite;
        float size = Mathf.Max(r.bounds.size.x ,r.bounds.size.y);

        r.transform.localScale = (targetSize /  size) * r.transform.localScale;
        trail.localScale = 2f *  trail.localScale / trail.lossyScale.y;
        coll.radius =  (0.8f / r.transform.localScale.x);
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
        Papier,
        PMD,
        GFT,
        RestAfval
    }
}