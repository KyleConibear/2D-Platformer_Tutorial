using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KyleConibear
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]

    /// <summary>
    /// Responsible for moving and animating the character entity.
    /// </summary>
    public class Character : MonoBehaviour
    {
        public bool isLogging = false;

        protected bool isRunning = false;

        [SerializeField] private float baseMovementSpeed = 1.0f;

        [Range(1.1f, 3)]
        [SerializeField] private float runningMultiplier = 2.0f;

        [SerializeField] private SpriteRenderer sprite = null;
        [SerializeField] private Animator animator = null;
        [SerializeField] private new Rigidbody2D rigidbody2D = null;

        #region MonoBehaviour Methods
        private void LateUpdate()
        {
            this.animator.SetBool("isMoving", this.rigidbody2D.velocity.x != 0);
            this.animator.SetBool("isRunning", this.isRunning);
        }
        #endregion

        #region Class Methods
        protected void Move(float direction)
        {
            float x;

            if (isRunning)
            {
                x = direction * (baseMovementSpeed * runningMultiplier);
            }
            else
            {
                x = direction * baseMovementSpeed;
            }

            if (x < 0)
            {
                this.sprite.flipX = true;
            }
            else if (x > 0)
            {
                this.sprite.flipX = false;
            }

            Logger.Log(this.isLogging, Logger.Type.Message, $"Player is moving, x = {x}");
            this.rigidbody2D.velocity = new Vector2(x, 0);
        }
        #endregion
    }
}