using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField] private float minimapYaxis = 40;
    private Transform player;

    private void Awake()
    {
        GameEvent.OnPlayerSpawn.AddListener(GetPlayerPosition);
    }

    private void OnDestroy()
    {
        GameEvent.OnPlayerSpawn.RemoveAllListeners();
    }

    private void GetPlayerPosition(GameObject player)
    {
        this.player = player.transform;
    }
    
    private void LateUpdate()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.position.x, minimapYaxis, player.position.z);
            transform.rotation = Quaternion.Euler(90, player.eulerAngles.y, 0);
        }
    }
}
