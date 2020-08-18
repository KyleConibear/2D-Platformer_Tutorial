using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KyleConibear
{
    [RequireComponent(typeof(InputHandler))]
    public class PlayerCharacter : Character
    {
        private InputHandler playerInput = null;

        #region MonoBehaviour Methods
        private void Awake()
        {
            this.playerInput = this.GetComponent<InputHandler>();
            this.playerInput.OnJump.AddListener(base.Jump);
        }

        private void Update()
        {
            if(base.isGrounded)
            {
                base.isRunning = this.playerInput.IsRunning;
            }

            // Prevents the velocity from being set to zero killing any momentum.
            // Mathf.Abs: Returns an absolute value converting a negative number into a positive.
            if (Mathf.Abs(this.playerInput.MoveDirection.x) > Mathf.Epsilon) // Mathf.Epsilon: The smallest value that a float can have different from zero.
            {
                base.Move(this.playerInput.MoveDirection.x);
            }            
        }
        #endregion
    }
}