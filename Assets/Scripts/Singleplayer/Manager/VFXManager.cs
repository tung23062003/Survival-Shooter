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

        GameEvent.OnTakeDamage.AddListener(OnTakeDamage);
    }

    private void OnDestroy()
    {
        GameEvent.OnTakeDamage.RemoveAllListeners();
    }

    public void OnTakeDamage(EntityInfo entityInfo, Vector3 damagePos)
    {
        if(entityInfo.entityType == EntityType.Player)
        {
            Debug.Log("VFX Player");
        }
        else if(entityInfo.entityType == EntityType.Monster)
        {
            switch (entityInfo.monsterType)
            {
                case MonsterType.Zombie:
                    PlayVFX_Addressable(AddressableKey.ZOMBIE_HIT_VFX, damagePos, Quaternion.identity);
                    break;

                case MonsterType.MistFiend:
                    Debug.Log("VFX MistFiend");
                    break;

                case MonsterType.BullDemon:
                    Debug.Log("VFX BullDemon");
                    break;

                default:
                    PlayVFX_Addressable(AddressableKey.ZOMBIE_HIT_VFX, damagePos, Quaternion.identity);
                    Debug.Log("VFX Default");
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

    public void PlayVFX_Addressable(string key, Vector3 position, Quaternion rotation, float destroyTime = 2f)
    {
        AddressableManager.Instance.CreateAsset<GameObject>(key, result =>
        {
            GameObject vfxInstance = ObjectPool.Instance.GetObject(result);
            vfxInstance.transform.SetPositionAndRotation(position, rotation);
            StartCoroutine(DeactiveAfterTime(vfxInstance, destroyTime));
            vfxInstance.SetActive(true);
        });
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
