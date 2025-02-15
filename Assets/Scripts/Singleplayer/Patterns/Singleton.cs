using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    protected virtual void Awake()
    {
        if(Instance == null)
        {
            Instance = GetComponent<T>();
        }
        else if(Instance.GetInstanceID() != gameObject.GetInstanceID())
        {
            Debug.LogError("Singleton is not unique!");
            Destroy(this.gameObject);
        }
    }
}
