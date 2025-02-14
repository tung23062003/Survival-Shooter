using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    Dictionary<GameObject, List<GameObject>> _listOjects = new Dictionary<GameObject, List<GameObject>>();

    public GameObject GetObject(GameObject defaultPref)
    {
        if (_listOjects.ContainsKey(defaultPref))
        {
            foreach (GameObject g in _listOjects[defaultPref])
            {
                if (g.activeSelf)
                    continue;
                return g;
            }
            GameObject b = Instantiate(defaultPref, this.transform.position, Quaternion.identity);
            _listOjects[defaultPref].Add(b);
            b.SetActive(false);

            return b;
        }
        List<GameObject> _newList = new List<GameObject>();

        GameObject o = Instantiate(defaultPref, this.transform.position, Quaternion.identity);
        _newList.Add(o);
        o.SetActive(false);
        _listOjects.Add(defaultPref, _newList);

        return o;
    }
}
