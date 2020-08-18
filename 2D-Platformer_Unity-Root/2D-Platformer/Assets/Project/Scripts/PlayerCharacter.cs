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
            base.isRunning = this.playerInput.IsRunning;
            base.Move(this.playerInput.MoveDirection.x);
        }
        #endregion
    }
}