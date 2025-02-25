using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
#if UNITY_ANDROID
    [SerializeField] private FloatingJoystick floatingJoystick;
    
#endif

    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    PlayerCamera playerCamera;
    PlayerMovement playerMovement;
    PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCamera = GetComponentInChildren<PlayerCamera>();


    }

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID
        floatingJoystick = FindObjectOfType<FloatingJoystick>();
#endif
#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif
    }

    void Update()
    {
#if UNITY_EDITOR
        viewInputVector.x = Input.GetAxis("Mouse X");
        viewInputVector.y = Input.GetAxis("Mouse Y") * -1;
#elif UNITY_ANDROID || UNITY_IOS
        if(Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        float scaleFactor = 1f / 25f;
                        Vector2 targetInput = new Vector2(touch.deltaPosition.x * scaleFactor,
                                                          touch.deltaPosition.y * scaleFactor * -1);
                
                        viewInputVector = Vector2.Lerp(viewInputVector, targetInput, Time.deltaTime * 10f);
                    }
                    else
                    {
                        viewInputVector = Vector2.zero;
                    }
                }
            }
        }
        else
        {
            viewInputVector = Vector2.zero;
        }
#endif


#if UNITY_EDITOR
        //Move input
        moveInputVector.x = Input.GetAxis("Horizontal");
        moveInputVector.y = Input.GetAxis("Vertical");

        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
            playerController.isJumping = true;

        //Fire
        if (Input.GetMouseButton(0))
            playerController.Attack();
#elif UNITY_ANDROID || UNITY_IOS
        //Move input
        moveInputVector.x = floatingJoystick.Horizontal;
        moveInputVector.y = floatingJoystick.Vertical;
#endif

        playerCamera.SetViewInputVector(viewInputVector);
        playerMovement.SetMoveInputVector(moveInputVector);
    }
}
