using UnityEngine;
using System.Collections;

public class SlingShot : MonoBehaviour, IDraggable {

    [SerializeField] private RecycleGameManager gameManager = null;
    public Trash projectile;
    [SerializeField] private GameObject line;
    [SerializeField] private float velocity;
    private Vector3 aimDir;

    public void Drop() {

        StartCoroutine(MoveProjectile(2f));
        GetComponentInChildren<Animator>().SetBool("Stretch", false);

        projectile.GetComponent<Rigidbody2D>().AddForce(aimDir.normalized * velocity, ForceMode2D.Impulse);
        line.SetActive(false);
        gameManager.shrinkRoutine = StartCoroutine(gameManager.Shrink(projectile));

        GetComponent<Collider2D>().enabled = false;
    }    

    public void Pick() {
        StartCoroutine(MoveProjectile(-2f));
        GetComponentInChildren<Animator>().SetBool("Stretch",true);
        line.SetActive(true);
    }

    public IEnumerator MoveProjectile(float dist) {
        for(int i = 0; i < 20; i++) {
            projectile.transform.position += Vector3.up * dist  / 20f;
            yield return 0;
        }
    }

    public void UpdatePos(Vector2 pos) {


        Vector3 eulers = Quaternion.LookRotation(Vector3.forward, (Vector3)pos - transform.position).eulerAngles;
        
        eulers.z =  Mathf.Clamp(eulers.z , 125, 235) + 180 /*Mathf.Clamp(eulers.z, 270, 360)*/;
        
        line.transform.rotation = Quaternion.Euler(eulers);

        aimDir = line.transform.up;
    }
}
