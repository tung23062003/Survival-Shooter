using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : Singleton<SFXManager>
{
    public List<SFXData> sfxList = new();
    private Dictionary<string, AudioClip> sfxDictionary = new();

    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
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

    public void PlaySFX(string key, Vector3 position, float volume = 1.0f)
    {
        if (sfxDictionary.TryGetValue(key, out AudioClip clip))
        {
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.transform.position = position;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("SFX is not founded: " + key);
        }
    }

    public void PlaySFX_Addressable(string key, Vector3 position, float volume = 1.0f)
    {
        AddressableManager.Instance.CreateAsset<AudioClip>(key, result =>
        {
            audioSource.clip = result;
            audioSource.volume = volume;
            audioSource.transform.position = position;
            audioSource.Play();
        });
    }
}

[System.Serializable]
public class SFXData
{
    public string key;
    public AudioClip clip;
}
