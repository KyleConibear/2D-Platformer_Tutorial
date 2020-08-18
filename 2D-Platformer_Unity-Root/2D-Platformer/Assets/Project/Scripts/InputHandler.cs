using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace KyleConibear
{
    public class InputHandler : MonoBehaviour
    {
        #region Fields
        public bool isLogging = false;

        private Vector2 moveDirection = Vector2.zero;
        public Vector2 MoveDirection => this.moveDirection;

        private bool isRunning = false;
        public bool IsRunning => this.isRunning;

        public UnityEvent OnJump = new UnityEvent();
        #endregion

        #region Methods       
        /// <summary>
        /// Player Input Callback Event capturing the left joystick & 'W','A','S','D' keys
        /// </summary>
        /// <param name="context">Data captured from the input device</param>
        public void Move(InputAction.CallbackContext context)
        {
            Logger.Log(this.isLogging, Logger.Type.Message, $"Move Input: {context.ReadValue<Vector2>()}");

            this.moveDirection = context.ReadValue<Vector2>();
        }
        public void Running(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Logger.Log(this.isLogging, Logger.Type.Message, $"Running Input-context.started.");
                this.isRunning = true;
            }
            else if (context.canceled)
            {
                Logger.Log(this.isLogging, Logger.Type.Message, $"Running Input-context.canceled.");
                this.isRunning = false;
            }
        }
        public void Jump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Logger.Log(this.isLogging, Logger.Type.Message, $"Jump Input received.");
                this.OnJump.Invoke();
            }
        }
        #endregion
    }
}