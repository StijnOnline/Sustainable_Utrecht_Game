using StijnUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowAI : MonoBehaviour
{
    [SerializeField] private Path path;
    [SerializeField] private float movespeed;
    float d = 0;
    
    private void FixedUpdate() {
        d += movespeed;
        transform.position = path.Evaluate(d);
    }
}
