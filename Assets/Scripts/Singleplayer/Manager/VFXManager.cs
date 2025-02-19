using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public List<VFXData> vfxList = new();
    private Dictionary<string, GameObject> vfxDictionary = new();

    private void Awake()
    {
        foreach (var vfx in vfxList)
        {
            vfxDictionary[vfx.key] = vfx.effectPrefab;
        }

        GameEvent.OnTakeDamage.AddListener(test);
    }

    private void OnDestroy()
    {
        GameEvent.OnTakeDamage.RemoveAllListeners();
    }

    public void test(EntityInfo entityInfo, Vector3 damagePos)
    {
        if(entityInfo.entityType == EntityType.Player)
        {
            Debug.Log("Player");
        }
        else if(entityInfo.entityType == EntityType.Monster)
        {
            switch (entityInfo.monsterType)
            {
                case MonsterType.Zombie:
                    PlayVFX("ZombieDamaged", damagePos, Quaternion.identity);
                    break;

                case MonsterType.MistFiend:
                    Debug.Log("MistFiend");
                    break;

                default:
                    Debug.Log("No Monster");
                    break;
            }
        }
    }

    public void PlayVFX(string key, Vector3 position, Quaternion rotation, float destroyTime = 2f)
    {
        if (vfxDictionary.TryGetValue(key, out GameObject prefab))
        {
            GameObject vfxInstance = ObjectPool.Instance.GetObject(prefab);
            vfxInstance.transform.SetPositionAndRotation(position, rotation);
            StartCoroutine(DeactiveAfterTime(vfxInstance, destroyTime));
            vfxInstance.SetActive(true);
        }
        else
        {
            Debug.LogWarning("VFX is not founded: " + key);
        }
    }

    private IEnumerator DeactiveAfterTime(GameObject prefab, float time)
    {
        yield return new WaitForSeconds(time);

        prefab.SetActive(false);
    }
}

[System.Serializable]
public class VFXData
{
    public string key;
    public GameObject effectPrefab;
}
