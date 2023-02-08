using System;
using BrickBuilder.Input;
using BrickBuilder.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BrickBuilder.User {
    public class EditorCamera : MonoBehaviour
    {
        public static Camera Camera;
        
        public float CameraSpeed;
        public float PanSpeed;
        public float LookSpeed;
        public bool InvertCamera = false;
        
        public float VerticalLookLimit = 89f;

        private bool _movementBoost = false;
        private bool _lookButtonDown = false;
        private bool _panButtonDown = false;

        private void Awake()
        {
            Camera = GetComponent<Camera>();
            
            // inputsystem doesnt really have a GetButton equivalent
            InputHelper.boost.started += _ => _movementBoost = true;
            InputHelper.boost.canceled += _ => _movementBoost = false;
            InputHelper.lookButton.started += _ => _lookButtonDown = true;
            InputHelper.lookButton.canceled += _ => _lookButtonDown = false;
            InputHelper.panButton.started += _ => _panButtonDown = true;
            InputHelper.panButton.canceled += _ => _panButtonDown = false;
        }

        private void Update()
        {
            // Movement
            // get movement delta
            Vector3 movementDelta = InputHelper.movement.ReadValue<Vector2>();
            movementDelta.z = movementDelta.y; // movement input is vector2 with forward being y axis
            
            float vertical = InputHelper.vertical.ReadValue<float>();
            movementDelta.y = vertical;
            
            // get movement delta relative to camera orientation
            Vector3 relativeMovementDelta = transform.right * movementDelta.x +
                                            transform.up * movementDelta.y +
                                            transform.forward * movementDelta.z;
            
            // move
            float speed = CameraSpeed;
            if (_movementBoost) speed *= 2f;
            transform.position += relativeMovementDelta * (speed * Time.deltaTime);
            
            
            // Pan movement
            if (_panButtonDown) 
            {
                // get mouse delta
                Vector2 mouseDelta = InputHelper.look.ReadValue<Vector2>();
                
                // get world movement delta
                Vector3 worldMovementDelta = transform.right * -mouseDelta.x +
                                             transform.up * -mouseDelta.y;
                
                // move
                transform.position += worldMovementDelta * (PanSpeed * Time.deltaTime);
            }
            
            // Camera Look
            if (_lookButtonDown) 
            {
                // lock mouse
                if (!MouseLocker.IsLocked)
                    MouseLocker.SetLockState(true);
                
                // get look delta
                Vector2 lookDelta = InputHelper.look.ReadValue<Vector2>();
                lookDelta.Set(lookDelta.y, lookDelta.x); // swap x and y
                
                // invert x if desired
                if (!InvertCamera) lookDelta.x = -lookDelta.x;
                
                // get current camera rotation and add look delta
                Vector3 cameraEulers = transform.localEulerAngles;
                cameraEulers += (Vector3) lookDelta * LookSpeed;

                // clamp camera rotation
                cameraEulers.x = Utils.ClampAngle(cameraEulers.x, -VerticalLookLimit, VerticalLookLimit);
                
                // set camera rotation
                transform.localEulerAngles = cameraEulers;

            }
            else
            {
                if (MouseLocker.IsLocked)
                    MouseLocker.SetLockState(false);
            }
        }
    }
}