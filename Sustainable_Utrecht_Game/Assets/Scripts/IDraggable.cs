using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDraggable
{
    void Pick();
    void UpdatePos(Vector2 pos);
    void Drop();
}
