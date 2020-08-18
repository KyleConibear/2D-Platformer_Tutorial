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

        [SerializeField] private float baseMovementSpeed = 1.0f;

        [Range(1.1f, 3)]
        [SerializeField] private float runningMultiplier = 2.0f;

        [SerializeField] private float jumpHeight = 10.0f;

        [Range(0.1f,1)]
        [SerializeField] private float groundCheckDepth = 0.5f;

        [SerializeField] private LayerMask groundLayerMask;

        [SerializeField] private SpriteRenderer sprite = null;
        [SerializeField] private Animator animator = null;
        [SerializeField] private new Rigidbody2D rigidbody2D = null;
        [SerializeField] private new BoxCollider2D boxCollider2D = null;

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

            Log(this.isLogging, Type.Message, $"Player {this.name} is moving, x = {x}");
            this.rigidbody2D.velocity = new Vector2(x, this.rigidbody2D.velocity.y);
        }
        protected void Jump()
        {
            if(this.IsGrounded())
            {
                Log(this.isLogging, Type.Message, $"Character {this.name} jumped.");
                this.rigidbody2D.velocity = new Vector2(this.rigidbody2D.velocity.x, this.jumpHeight);
            }            
        }

        protected bool IsGrounded()
        {
            RaycastHit2D hit = Physics2D.BoxCast(this.boxCollider2D.bounds.center, this.boxCollider2D.bounds.size, 0f, Vector2.down, this.groundCheckDepth, groundLayerMask);
            Color rayColor;

            if(hit.collider != null)
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

            Log(this.isLogging, Type.Message, $"Character {this.name} is grounded = {hit.collider != null}");

            return hit.collider != null;
        }
        #endregion
    }
}