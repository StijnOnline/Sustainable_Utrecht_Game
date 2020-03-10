using UnityEngine;
using System.Collections;

public class Catapult : MonoBehaviour, IDraggable {

    public Rigidbody2D projectile;
    [SerializeField] private GameObject line;
    [SerializeField] private float velocity;
    private Vector3 aimDir;

    public void Drop() {
        projectile.AddForce(aimDir * velocity, ForceMode2D.Impulse);
        line.SetActive(false);
    }

    public void Pick() {
        line.SetActive(true);
    }

    public void UpdatePos(Vector2 pos) {
        aimDir = transform.position-(Vector3) pos ;
        line.transform.rotation = Quaternion.LookRotation(Vector3.forward, aimDir);
    }
}
