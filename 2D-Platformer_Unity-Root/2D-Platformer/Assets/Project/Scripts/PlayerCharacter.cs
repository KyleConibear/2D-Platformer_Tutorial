using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KyleConibear
{
    using static Logger;

    [RequireComponent(typeof(InputHandler))]
    public class PlayerCharacter : Character
    {
        [Tooltip("The amount the \"baseMovementSpeed\" is multiplied by when running.")]
        [Range(1.1f, 3)]
        [SerializeField] private float runningMultiplier = 2.0f;

        [Tooltip("If the player releases the jump button before the jumps peak height gravity will be multiplied to quicken descent.")]
        [Range(1,2)]
        [SerializeField] private float lowJumpGravityMultiplier = 1.25f;

        private InputHandler playerInput = null;

        #region MonoBehaviour Methods
        private void Awake()
        {
            this.playerInput = this.GetComponent<InputHandler>();
            this.playerInput.OnJump.AddListener(base.Jump);
        }

        private void Update()
        {
            base.animator.SetBool("isRunning", this.playerInput.isRunning);

            // Prevents the velocity from being set to zero killing any momentum.
            // Mathf.Abs: Returns an absolute value converting a negative number into a positive.
            if (base.isGrounded && Mathf.Abs(this.playerInput.MoveDirection.x) > Mathf.Epsilon) // Mathf.Epsilon: The smallest value that a float can have different from zero.
            {
                base.Move(this.playerInput.MoveDirection.x);
            }
        }
        #endregion

        #region Class Methods
        protected override void FasterFall()
        {
            // We are jumping up
            if(base.rigidbody2D.velocity.y > Mathf.Epsilon && this.playerInput.isJumping == false)
            {
                base.rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * this.lowJumpGravityMultiplier * Time.deltaTime;
            }
            base.FasterFall();
        }

        protected override float CurrentMovementSpeed()
        {
            if(this.playerInput.isRunning)
            {
                return base.baseMovementSpeed * this.runningMultiplier;
            }
            else
            {
                return base.CurrentMovementSpeed();
            }           
        }
        #endregion
    }
}