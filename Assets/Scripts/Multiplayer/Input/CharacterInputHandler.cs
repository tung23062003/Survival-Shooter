using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterInputHandler : MonoBehaviour
{
    public FloatingJoystick floatingJoystick;
    public RectTransform joystickRect;
    public RectTransform shootButtonRect;

    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    private bool isJumpButtonPressed;
    private bool isFireButtonPressed;

    LocalCameraHandler localCameraHandler;
    CharacterMovementHandler characterMovementHandler;


    private void Awake()
    {
        localCameraHandler = GetComponentInChildren<LocalCameraHandler>();
        characterMovementHandler = GetComponent<CharacterMovementHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (!characterMovementHandler.Object.HasInputAuthority)
            return;

        //View input
#if UNITY_EDITOR
        viewInputVector.x = Input.GetAxis("Mouse X");
        viewInputVector.y = Input.GetAxis("Mouse Y") * -1;
#elif UNITY_ANDROID || UNITY_IOS
        if(Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                //if (RectTransformUtility.RectangleContainsScreenPoint(joystickRect, touch.position))
                //{

                //}

                if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        float scaleFactor = 1f / 50f;
                        viewInputVector.x = touch.deltaPosition.x * scaleFactor;
                        viewInputVector.y = touch.deltaPosition.y * scaleFactor * -1;
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

        //if (Input.touchCount > 0) 
        //{
        //    Touch touch = floatingJoystick.IsJoystickAtive() ? Input.GetTouch(1) : Input.GetTouch(0);
        //    if (touch.phase == TouchPhase.Moved) 
        //    {
        //        float scaleFactor = 1f / 50f;
        //        viewInputVector.x = touch.deltaPosition.x * scaleFactor;
        //        viewInputVector.y = touch.deltaPosition.y * scaleFactor * -1;
        //    }
        //    else
        //    {
        //        viewInputVector = Vector2.zero;
        //    }
        //}
        //else
        //{
        //    viewInputVector = Vector2.zero;
        //}
#endif


#if UNITY_EDITOR
        //Move input
        moveInputVector.x = Input.GetAxis("Horizontal");
        moveInputVector.y = Input.GetAxis("Vertical");

        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
            isJumpButtonPressed = true;

        //Fire
        if (Input.GetMouseButton(0))
            isFireButtonPressed = true;
#elif UNITY_ANDROID || UNITY_IOS
        //Move input
        moveInputVector.x = floatingJoystick.Horizontal;
        moveInputVector.y = floatingJoystick.Vertical;
#endif

        localCameraHandler.SetViewInputVector(viewInputVector);
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new();

        //Aim data
        networkInputData.aimForwardVector = localCameraHandler.transform.forward;

        //Move data
        networkInputData.movementInput = moveInputVector;

        //Jump data
        networkInputData.isJumpPressed = isJumpButtonPressed;

        //Fire data
        networkInputData.isFirePressed = isFireButtonPressed;

        isJumpButtonPressed = false;
        isFireButtonPressed = false;

        return networkInputData;
    }
}
