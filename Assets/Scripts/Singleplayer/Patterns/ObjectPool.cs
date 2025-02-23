using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    Dictionary<GameObject, List<GameObject>> _listOjects = new();

    public GameObject Spawn(GameObject defaultPref)
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
        List<GameObject> _newList = new();

        GameObject o = Instantiate(defaultPref, this.transform.position, Quaternion.identity);
        _newList.Add(o);
        o.SetActive(false);
        _listOjects.Add(defaultPref, _newList);

        return o;
    }

    public async Task DespawnAll()
    {
        foreach (var obj in _listOjects)
        {
            foreach (var item in obj.Value)
            {
                item.SetActive(false);
            }
        }

        await Task.Yield();
    }
}
