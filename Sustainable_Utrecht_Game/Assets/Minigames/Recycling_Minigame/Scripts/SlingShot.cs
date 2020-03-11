using UnityEngine;
using System.Collections;

public class SlingShot : MonoBehaviour, IDraggable {

    public Rigidbody2D projectile;
    [SerializeField] private GameObject line;
    [SerializeField] private float velocity;
    private Vector3 aimDir;

    public void Drop() {
        projectile.constraints = RigidbodyConstraints2D.None;
        projectile.AddForce(aimDir * velocity, ForceMode2D.Impulse);
        line.SetActive(false);
    }

    public void Pick() {
        line.SetActive(true);
    }

    public void UpdatePos(Vector2 pos) {
        aimDir = transform.position-(Vector3) pos ;
        line.transform.rotation = Quaternion.LookRotation(Vector3.forward, aimDir);
/*
       snapping
        Vector3 rot;
        rot = line.transform.rotation.eulerAngles;
        rot.z = Mathf.FloorToInt(rot.z / 30) * 30;
        line.transform.rotation = Quaternion.Euler(rot);
*/
    }
}
