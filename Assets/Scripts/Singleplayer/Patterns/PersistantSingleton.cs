using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<T>();
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance.GetInstanceID() != gameObject.GetInstanceID())
        {
            Debug.LogError("PSTSingleton is not unique!");
            Destroy(this.gameObject);
        }
    }
}
