using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour {
    private float targetSize = 2f;
    private CircleCollider2D coll = null;

    public TrashInfo trashInfo;

    public void Init() {
        Transform trail = GetComponentInChildren<TrailRenderer>().transform;

        SpriteRenderer r = GetComponent<SpriteRenderer>();
        CircleCollider2D coll = GetComponent<CircleCollider2D>();
        r.sprite = trashInfo.sprite;
        float size = Mathf.Max(r.bounds.size.x ,r.bounds.size.y);

        r.transform.localScale = (targetSize /  size) * r.transform.localScale;
        trail.localScale = 2f *  trail.localScale / trail.lossyScale.y;
        coll.radius =  (0.8f / r.transform.localScale.x);
    }

    public enum TrashType {
        Papier,
        PMD,
        GFT,
        RestAfval
    }
}