using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityChanger : MonoBehaviour
{
    [SerializeField] private List<CityVariable> cityVariables;

    [System.Serializable]
    public struct CityVariable {
        public string _name;
        public SDGs _SDG;

        public GameObject _object;
        public Vector2 activeRange;

    }

    private void OnEnable() {
        SaveData save = DataSaver.LoadData();
        foreach(CityVariable cityVariable in cityVariables) {
            bool inrange = (save.SDGPoints[(int)cityVariable._SDG] >= cityVariable.activeRange.x && save.SDGPoints[(int)cityVariable._SDG] < cityVariable.activeRange.y);
            if(cityVariable._object != null) {
                cityVariable._object.SetActive(inrange);
            }
        }
    }
}
