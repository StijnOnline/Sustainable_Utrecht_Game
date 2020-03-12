﻿using UnityEngine;
using System.Collections;

public class SlingShot : MonoBehaviour, IDraggable {

    [SerializeField] private RecycleGameManager gameManager = null;
    public Trash projectile;
    [SerializeField] private GameObject line;
    [SerializeField] private float velocity;
    private Vector3 aimDir;

    public void Drop() {
        projectile.GetComponent<Rigidbody2D>().AddForce(aimDir.normalized * velocity, ForceMode2D.Impulse);
        line.SetActive(false);
        gameManager.shrinkRoutine = StartCoroutine(gameManager.Shrink(projectile));
    }    

    public void Pick() {
        line.SetActive(true);
    }

    public void UpdatePos(Vector2 pos) {


        Vector3 eulers = Quaternion.LookRotation(Vector3.forward, aimDir).eulerAngles;
        eulers.z = Mathf.Clamp(eulers.z, -45, 45 );
        line.transform.rotation = Quaternion.Euler(eulers);


        aimDir = line.transform.forward;

        /*aimDir = transform.position-(Vector3) pos ;
        line.transform.rotation = Quaternion.LookRotation(Vector3.forward, aimDir);*/
        /*
               snapping
                Vector3 rot;
                rot = line.transform.rotation.eulerAngles;
                rot.z = Mathf.FloorToInt(rot.z / 30) * 30;
                line.transform.rotation = Quaternion.Euler(rot);
        */
    }
}
