using StijnUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowAI : MonoBehaviour
{
    [SerializeField] private Path path;
    [SerializeField] private float movespeed;
    [SerializeField] private float turnSmooth;
    float d = 0;
    
    private void FixedUpdate() {
        d += movespeed;
        Path.PathData p = path.Evaluate(d);
        transform.position = p.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation( p.forward,Vector3.up), turnSmooth);
    }
}
