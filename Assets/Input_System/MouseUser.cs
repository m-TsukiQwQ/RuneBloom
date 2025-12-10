using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public enum MouseButton
    {
        Left, Right
    }
    public class MouseUser : MonoBehaviour
    {
        private PlayerInputSystem _input;

        public Vector2 mousePosition { get; private set; }
        public Vector2 mouseInWorldPosition => Camera.main.ScreenToWorldPoint(mousePosition);

        private bool _isLeftMouseButtonPressed;
        private bool _isRightMouseButtonPressed;


        private void OnEnable()
        {
            _input = PlayerInputSystem.Instance;

            _input.Player.MousePosition.performed += OnMousePositionPerformed;

            _input.Player.UseItem.performed += OnPerformActionPerformed;
            _input.Player.UseItem.canceled += OnPerformActionCanceled;

            _input.Player.CancelAction.performed += OnCancelActionPerformed;
            _input.Player.CancelAction.canceled += OnCancelActionCanceled;
        }

        private void OnDisable()
        {
            _input.Player.MousePosition.performed -= OnMousePositionPerformed;

            _input.Player.UseItem.performed -= OnPerformActionPerformed;
            _input.Player.UseItem.canceled -= OnPerformActionCanceled;

            _input.Player.CancelAction.performed -= OnCancelActionPerformed;
            _input.Player.CancelAction.canceled -= OnCancelActionCanceled;
        }

        private void OnMousePositionPerformed(InputAction.CallbackContext ctx)
        {
            mousePosition = ctx.ReadValue<Vector2>();
        }

        private void OnPerformActionPerformed(InputAction.CallbackContext ctx)
        {
            _isRightMouseButtonPressed = true;
        }
        private void OnPerformActionCanceled(InputAction.CallbackContext ctx)
        {
            _isRightMouseButtonPressed = false;
        }

        private void OnCancelActionPerformed(InputAction.CallbackContext ctx)
        {
            _isLeftMouseButtonPressed = true;
        }
        private void OnCancelActionCanceled(InputAction.CallbackContext ctx)
        {
            _isLeftMouseButtonPressed = false;
        }

        public bool IsMouseButtonPressed(MouseButton button)
        {
            bool result = button == MouseButton.Left ? _isLeftMouseButtonPressed : _isRightMouseButtonPressed;
            Debug.Log(result);
            return  result;
        }
    }
}