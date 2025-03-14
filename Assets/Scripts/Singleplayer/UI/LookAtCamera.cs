using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool invert;

    private Transform playerCameraTransform;

    private void Awake()
    {
        playerCameraTransform = FindObjectOfType<PlayerCamera>().transform;
    }


    private void Update()
    {
        LookAt();
    }

    private void OnEnable()
    {
        LookAt();
    }

    private void LookAt()
    {
        if (invert)
        {
            Vector3 dir = (transform.position - playerCameraTransform.position).normalized;
            transform.LookAt(transform.position + dir);
        }
        else
        {
            transform.LookAt(playerCameraTransform.position);
        }
    }
}
