using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireButtonHandle : MonoBehaviour
{
    private bool isShooting = false;
    private PlayerController player;

    private void Awake()
    {
        GameEvent.OnPlayerSpawn.AddListener(OnPlayerSpawn);
    }

    private void OnDestroy()
    {
        GameEvent.OnPlayerSpawn.RemoveAllListeners();
    }

    void Start()
    {
        AddEventTrigger();
    }

    private void OnPlayerSpawn(GameObject playerPrefab)
    {
        if (playerPrefab != null)
            player = playerPrefab.GetComponent<PlayerController>();
    }

    private void AddEventTrigger()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();

        EventTrigger.Entry pointerDown = new();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((data) => StartShooting());

        EventTrigger.Entry pointerUp = new();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((data) => StopShooting());

        trigger.triggers.Add(pointerDown);
        trigger.triggers.Add(pointerUp);
    }

    private void Update()
    {
        if (isShooting)
        {
            player.Attack();
        }
    }

    void StartShooting()
    {
        isShooting = true;
    }

    void StopShooting()
    {
        isShooting = false;
    }
}
