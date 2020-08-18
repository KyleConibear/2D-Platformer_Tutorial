using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KyleConibear
{
    using static Logger;

    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]

    /// <summary>
    /// Responsible for moving and animating the character entity.
    /// </summary>
    public class Character : MonoBehaviour
    {
        public bool isLogging = false;

        protected bool isRunning = false;

        [Header("Movement Tuning")]

        [Tooltip("The amount of velocity applied along the x axis when moving.")]
        [SerializeField] private float baseMovementSpeed = 1.0f;

        [Tooltip("The amount the \"baseMovementSpeed\" is multiplied by when running.")]
        [Range(1.1f, 3)]
        [SerializeField] private float runningMultiplier = 2.0f;

        [Tooltip("The amount of velocity applied to the y axis when performing a base jump.")]
        [Range(3,12)]
        [SerializeField] private float jumpHeight = 6.0f;

        [Tooltip("The amount the \"jumpHeight\" is multiplied by when a running.")]
        [Range(1.1f, 2)]
        [SerializeField] private float jumpMultiplier = 1.25f;

        [Header("Ground Check")]

        protected bool isGrounded = false;

        [Range(0.1f,1)]
        [SerializeField] private float groundCheckDepth = 0.5f;

        [SerializeField] private LayerMask groundLayerMask;

        [Header("Components")]

        [SerializeField] private SpriteRenderer sprite = null;
        [SerializeField] private Animator animator = null;
        [SerializeField] private Rigidbody2D rigidbody2D = null;
        [SerializeField] private BoxCollider2D boxCollider2D = null;

        #region MonoBehaviour Methods
        private void LateUpdate()
        {
            this.animator.SetBool("isMoving", this.rigidbody2D.velocity.x != 0);
            this.animator.SetBool("isRunning", this.isRunning);

            this.isGrounded = this.IsGrounded();
            this.animator.SetBool("isGrounded", isGrounded);
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

            Log(this.isLogging, Type.Message, $"Player {this.name} is moving, x = {x}");
            this.rigidbody2D.velocity = new Vector2(x, this.rigidbody2D.velocity.y);
        }
        protected void Jump()
        {
            if(this.IsGrounded())
            {
                Log(this.isLogging, Type.Message, $"Character {this.name} jumped.");

                float height = this.jumpHeight;

                if (this.isRunning)
                {
                    height = this.jumpHeight * this.jumpMultiplier;
                }

                this.rigidbody2D.velocity = new Vector2(this.rigidbody2D.velocity.x, height);
            }            
        }
        private bool IsGrounded()
        {
            bool isGrounded = false;

            RaycastHit2D hit = Physics2D.BoxCast(this.boxCollider2D.bounds.center, this.boxCollider2D.bounds.size, 0f, Vector2.down, this.groundCheckDepth, groundLayerMask);
            Color rayColor;

            isGrounded = hit.collider != null;

            if (isGrounded)
            {
                rayColor = Color.green;
                Log(this.isLogging, Type.Message, $"Character {this.name} ground collider hit {hit.collider}.");
            }
            else
            {
                rayColor = Color.red;
            }

            Debug.DrawRay(this.boxCollider2D.bounds.center + new Vector3(this.boxCollider2D.bounds.extents.x, 0), Vector2.down * (this.boxCollider2D.bounds.extents.y + this.groundCheckDepth), rayColor);
            Debug.DrawRay(this.boxCollider2D.bounds.center - new Vector3(this.boxCollider2D.bounds.extents.x, 0), Vector2.down * (this.boxCollider2D.bounds.extents.y + this.groundCheckDepth), rayColor);
            Debug.DrawRay(this.boxCollider2D.bounds.center - new Vector3(this.boxCollider2D.bounds.extents.x, this.boxCollider2D.bounds.extents.y + this.groundCheckDepth), Vector2.right * (this.boxCollider2D.bounds.extents.x * 2f), rayColor);

            Log(this.isLogging, Type.Message, $"Character {this.name} is grounded = {isGrounded}");

            return isGrounded;
        }
        #endregion
    }
}