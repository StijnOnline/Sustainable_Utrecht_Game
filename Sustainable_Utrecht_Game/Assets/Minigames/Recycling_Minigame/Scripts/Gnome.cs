using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gnome : MonoBehaviour
{
    [SerializeField] private GameObject idle;
    [SerializeField] private GameObject reaction;

    private void OnCollisionEnter2D(Collision2D collision) {
        idle.SetActive(false);
        reaction.SetActive(true);
        Invoke("Idle", 0.3f);
    }

    void Idle() {
        idle.SetActive(true);
        reaction.SetActive(false);
    }
}
