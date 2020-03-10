using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "TrashName", menuName = "ScriptableObjects/TrashInfo", order = 1)]
public class TrashInfo : ScriptableObject {
    public Sprite sprite;
    public Trash.TrashType correctType;
    public string explanation;
}

