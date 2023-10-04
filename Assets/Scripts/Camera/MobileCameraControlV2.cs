using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class MobileCameraControlV2 : MonoBehaviour {
    PlayerInput input;
    Vector2 primaryFingerPosition, secondaryFingerPosition;
    Vector2 deltaPrimary, deltaSecondary;
    bool isZooming;

    public PlayerStateMachine playerStateMachine;
    public Camera mainCamera;
    public float cameraZoomSpeed = 1f;
    public float scrollSensitivity = 1f, scrollSensitivityMultiplier = 1f;
    public float ortographicCameraZoomMin = .2f, ortographicCameraZoomMax = 15f;

    void Awake() {
        input = new PlayerInput();
    }

    void OnEnable() {
        input.CameraControls.Enable();
    }

    void OnDisable() {
        input.CameraControls.Disable();
    }

    void Start() {
        setUpInputControls();
    }

    void setUpInputControls() {
        input.CameraControls.PrimaryFingerPosition.started += ctx => onPrimaryFingerPostion(ctx);
        input.CameraControls.PrimaryFingerPosition.performed += ctx => onPrimaryFingerPostion(ctx);
        input.CameraControls.PrimaryFingerPosition.canceled += ctx => onPrimaryFingerPostion(ctx);

        input.CameraControls.PrimaryFingerDelta.started += ctx => onPrimaryFingerDelta(ctx);
        input.CameraControls.PrimaryFingerDelta.performed += ctx => onPrimaryFingerDelta(ctx);
        input.CameraControls.PrimaryFingerDelta.canceled += ctx => onPrimaryFingerDelta(ctx);

        input.CameraControls.SecondaryFingerPosition.started += ctx => onSecondaryFingerPostion(ctx);
        input.CameraControls.SecondaryFingerPosition.performed += ctx => onSecondaryFingerPostion(ctx);
        input.CameraControls.SecondaryFingerPosition.canceled += ctx => onSecondaryFingerPostion(ctx);
        
        input.CameraControls.SecondaryFingerDelta.started += ctx => onSecondaryFingerDelta(ctx);
        input.CameraControls.SecondaryFingerDelta.performed += ctx => onSecondaryFingerDelta(ctx);
        input.CameraControls.SecondaryFingerDelta.canceled += ctx => onSecondaryFingerDeltaCanceled(ctx);
    }

    void onPrimaryFingerPostion(InputAction.CallbackContext ctx) {
        primaryFingerPosition = ctx.ReadValue<Vector2>();
    }


    // Scroll
    void onPrimaryFingerDelta(InputAction.CallbackContext ctx) {
        if (!playerStateMachine.IsMovePress) {
            deltaPrimary = ctx.ReadValue<Vector2>();
            var frameIndepentDeltaPrimary = deltaPrimary * Time.deltaTime * scrollSensitivity * scrollSensitivityMultiplier;
            transform.Translate(-frameIndepentDeltaPrimary.y, 0, frameIndepentDeltaPrimary.x, Space.World);
        }
    }

    void onSecondaryFingerPostion(InputAction.CallbackContext ctx) {
        secondaryFingerPosition = ctx.ReadValue<Vector2>();
    }
    
    void onSecondaryFingerDelta(InputAction.CallbackContext ctx) {
        deltaSecondary = ctx.ReadValue<Vector2>();
        isZooming = true;
        if (playerStateMachine.IsMovePress) { // If using joystick than use secondaryFinger to control camera
            isZooming = false;
            var frameIndepentDeltaSecondary = deltaSecondary * Time.deltaTime * scrollSensitivity * scrollSensitivityMultiplier;
            transform.Translate(-frameIndepentDeltaSecondary.y, 0, frameIndepentDeltaSecondary.x, Space.World);
        }
    }

    void onSecondaryFingerDeltaCanceled(InputAction.CallbackContext ctx) {
        deltaSecondary = ctx.ReadValue<Vector2>();
        isZooming = false;
    }

    float smoothVelocity;
    void Update() {
        if (isZooming) { // Apply zoom
            // Debug.Log($"{deltaPrimary}, {deltaSecondary}");
            float zoom = Vector2.Distance(primaryFingerPosition - deltaPrimary, secondaryFingerPosition - deltaSecondary) / Vector2.Distance(primaryFingerPosition, secondaryFingerPosition);
            // Perspective
            // camera.transform.position = Vector3.LerpUnclamped(pos1After, camera.transform.position, 1 / zoom);
            // Ortographic
            if (zoom < 1) {
                mainCamera.orthographicSize = Mathf.Min(Mathf.SmoothDamp(mainCamera.orthographicSize, mainCamera.orthographicSize + cameraZoomSpeed, ref smoothVelocity, Time.deltaTime * cameraZoomSpeed), ortographicCameraZoomMax);
            } else if (zoom > 1) {
                mainCamera.orthographicSize = Mathf.Max(Mathf.SmoothDamp(mainCamera.orthographicSize, mainCamera.orthographicSize - cameraZoomSpeed, ref smoothVelocity, Time.deltaTime * cameraZoomSpeed), ortographicCameraZoomMin);
            }
        }

        // switch(Input.deviceOrientation) {
        //     case DeviceOrientation.LandscapeLeft:
        //         if (Screen.orientation != ScreenOrientation.LandscapeLeft) {
        //             Screen.orientation = ScreenOrientation.LandscapeLeft;
        //         }
        //         break;
        //     case DeviceOrientation.LandscapeRight:
        //         if (Screen.orientation != ScreenOrientation.LandscapeRight) {
        //             Screen.orientation = ScreenOrientation.LandscapeRight;
        //         }
        //         break;
        // }
    }
}
