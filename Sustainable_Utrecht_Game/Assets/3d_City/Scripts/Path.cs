using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StijnUtility {
    public class Path : MonoBehaviour {
        [SerializeField] private bool loop = false;
        [SerializeField] private List<Transform> nodes = new List<Transform>();

        public float length { get; private set; } = 0;



        void Start() {
            nodes.Clear();

            foreach(Transform child in GetComponentsInChildren<Transform>()) {
                if(child != transform)
                    nodes.Add(child);
            }

            for(int i = 0; i < nodes.Count; i++) {
                if(i > 0) {
                    Vector3 toNext = (nodes[i].position - nodes[i - 1].position);
                    length += toNext.magnitude;
                }
            }
            if(loop) {
                Vector3 toNext = (nodes[nodes.Count - 1].position - nodes[0].position);
                length += toNext.magnitude;
            }

        }

        void OnDrawGizmos() {
            Gizmos.color = Color.red;
            nodes.Clear();

            foreach(Transform child in GetComponentsInChildren<Transform>()) {
                if(child != transform)
                    nodes.Add(child);
            }

            for(int i = 0; i < nodes.Count; i++) {
                Gizmos.DrawWireSphere(nodes[i].position, 0.1f);
                if(i > 0)
                    Gizmos.DrawLine(nodes[i - 1].position, nodes[i].position);
            }
            if(loop)
                Gizmos.DrawLine(nodes[nodes.Count - 1].position, nodes[0].position);
        }

        public PathData Evaluate(float dist) {
            if(dist <= 0) { return new PathData( nodes[0].position, (nodes[1].position - nodes[0].position).normalized); }
            float distLeft = dist % length;
            int n = 0;

            while(distLeft > 0) {

                if(!loop && n == nodes.Count - 1) { return new PathData(nodes[n].position, (nodes[n].position - nodes[n-1].position).normalized); } //if there aren't any more nodes


                Vector3 toNext = (nodes[(n + 1) % nodes.Count].position - nodes[n % nodes.Count].position);
                if((distLeft) < toNext.magnitude) { //if next node is enough
                    Vector3 pos = nodes[n % nodes.Count].position + toNext.normalized * distLeft;
                    return new PathData(pos, toNext.normalized);
                } else {
                    distLeft -= toNext.magnitude;
                    n++;
                }
            }
            Debug.LogError("Error Evaluating path");
            return new PathData(Vector3.zero, Vector3.zero);
        }

        public List<Transform> GetPath() {
            return nodes;
        }

        public class PathData {
            public Vector3 position;
            public Vector3 forward;
            public PathData(Vector3 _position, Vector3 _forward) {
                position = _position;
                forward = _forward;
            }
        }
    }
}