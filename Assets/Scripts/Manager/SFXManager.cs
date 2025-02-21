using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public List<SFXData> sfxList = new List<SFXData>();
    private Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        foreach (var sfx in sfxList)
        {
            sfxDictionary[sfx.key] = sfx.clip;
        }

        GameEvent.OnTakeDamage.AddListener(OnTakeDamage);
    }

    private void OnDestroy()
    {
        GameEvent.OnTakeDamage.RemoveAllListeners();
    }

    private void OnTakeDamage(EntityInfo entityInfo, Vector3 damagePos)
    {
        if (entityInfo.entityType == EntityType.Player)
        {
            Debug.Log("SFX Player");
        }
        else if (entityInfo.entityType == EntityType.Monster)
        {
            switch (entityInfo.monsterType)
            {
                case MonsterType.Zombie:
                    PlaySFX("ZombieDamaged", damagePos);
                    break;

                case MonsterType.MistFiend:
                    Debug.Log("SFX MistFiend");
                    break;

                case MonsterType.BullDemon:
                    Debug.Log("SFX BullDemon");
                    break;

                default:
                    PlaySFX("ZombieDamaged", damagePos);
                    Debug.Log("SFX Default");
                    break;
            }
        }
    }

    public void PlaySFX(string key, Vector3 point, float volume = 1.0f)
    {
        if (sfxDictionary.TryGetValue(key, out AudioClip clip))
        {
            AudioSource.PlayClipAtPoint(clip, point, volume);
        }
        else
        {
            Debug.LogWarning("SFX is not founded: " + key);
        }
    }
}

[System.Serializable]
public class SFXData
{
    public string key;
    public AudioClip clip;
}
