using System;
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
    [RequireComponent(typeof(InputHandler))]
    /// <summary>
    /// Responsible for moving and animating the character entity.
    /// </summary>
    public class Player : MonoBehaviour
    {
        public bool isLogging = false;

        [SerializeField] private State _state = State.Idle;
        public State state => this._state;
        public enum State
        {
            Idle = 0,
            Moving = 1,
            Jumping = 2,
            Falling = 3,
            Crouching = 4,
            Climbing = 5,
            Hurt = 6
        }

        public static Action OnPlayerHurt;

        [Header("Movement Tuning")]

        [Tooltip("The amount of velocity applied along the x axis when moving.")]
        [Range(3.1f, 6)]
        [SerializeField] private float baseMovementSpeed = 5.0f;

        [Tooltip("The amount the \"baseMovementSpeed\" is multiplied by when running.")]
        [Range(1.1f, 3)]
        [SerializeField] private float runningMultiplier = 1.5f;

        [Tooltip("The amount the character will slide after movement has stopped.")]
        // slideAmount must be less than baseMovementSpeed
        [Range(1, 3)]
        [SerializeField] private float slideAmount = 3.0f;

        [Tooltip("The amount of velocity applied to the y axis when performing a base jump.")]
        [Range(6, 12)]
        [SerializeField] protected float jumpHeight = 12.0f;

        [Tooltip("The amount gravity is multiplied by when falling.")]
        [Range(1, 3)]
        [SerializeField] private float fallMultiplier = 2.0f;

        [Tooltip("If the player releases the jump button before the jumps peak height gravity will be multiplied to quicken descent.")]
        [Range(1, 2)]
        [SerializeField] private float lowJumpGravityMultiplier = 1.5f;

        [Header("Ground Check")]
        protected bool isGrounded = false;

        [Range(0.1f, 1)]
        [SerializeField] private float groundCheckDepth = 0.5f;

        [SerializeField] private LayerMask groundLayerMask;

        [Header("Components")]
        private InputHandler playerInput = null;
        private SpriteRenderer sprite = null;
        private Animator animator = null;
        private Rigidbody2D rigidbody2D = null;
        private BoxCollider2D boxCollider2D = null;

        #region MonoBehaviour Methods
        private void Awake()
        {
            this.playerInput = this.GetComponent<InputHandler>();
            this.playerInput.OnJump.AddListener(this.Jump);

            this.sprite = this.GetComponent<SpriteRenderer>();
            this.animator = this.GetComponent<Animator>();
            this.rigidbody2D = this.GetComponent<Rigidbody2D>();
            this.boxCollider2D = this.GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            if (this._state == State.Hurt)
                return;

            this.animator.SetBool("isRunning", this.playerInput.isRunning);

            // Prevents the velocity from being set to zero killing any momentum.
            // Mathf.Abs: Returns an absolute value converting a negative number into a positive.
            if (Mathf.Abs(this.playerInput.MoveDirection.x) > Mathf.Epsilon) // Mathf.Epsilon: The smallest value that a float can have different from zero.
            {
                this.Move(this.playerInput.MoveDirection.x);
            }
        }

        private void LateUpdate()
        {
            if (this._state == State.Hurt)
                return;

            this.StateController();
            this.FasterFall();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Enemy enemy = collision.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                this.Jump(true);
                // If the character is falling kill the enemy
                if (this._state == State.Falling)
                {
                    enemy.Kill();
                }
                // Else the character becomes hurt
                else
                {
                    this.PlayerHurt();
                }
            }
        }
        #endregion

        #region Class Methods
        private void PlayerHurt(bool disableGameobject = false)
        {
            this._state = State.Hurt;

            this.animator.SetInteger("state", (int)this._state);

            OnPlayerHurt.Invoke();

            this.boxCollider2D.enabled = false;

            this.gameObject.SetActive(!disableGameobject);
        }

        private float CurrentMovementSpeed()
        {
            if (this.playerInput.isRunning)
            {
                return this.baseMovementSpeed * this.runningMultiplier;
            }
            else
            {
                return this.baseMovementSpeed;
            }
        }

        private void Move(float xAxisDirection)
        {
            float x = 0;

            if (xAxisDirection > Mathf.Epsilon)
            {
                x = CurrentMovementSpeed();
            }
            else if (xAxisDirection < Mathf.Epsilon)
            {
                x = -CurrentMovementSpeed();
            }

            if (xAxisDirection < 0)
            {
                this.sprite.flipX = true;
            }
            else if (xAxisDirection > 0)
            {
                this.sprite.flipX = false;
            }

            Log(this.isLogging, Type.Message, $"Player {this.name} is moving, x = {xAxisDirection}");
            this.rigidbody2D.velocity = new Vector2(x, this.rigidbody2D.velocity.y);
        }

        private void Jump()
        {
            if (this.isGrounded)
            {
                this.Jump(true);
            }
        }

        private void Jump(bool isForced = false)
        {
            if (this.isGrounded || isForced)
            {
                Log(this.isLogging, Type.Message, $"Character {this.name} jumped.");
                this.rigidbody2D.velocity = new Vector2(this.rigidbody2D.velocity.x, this.jumpHeight);
            }
        }

        private void FasterFall()
        {
            // We are jumping up
            if (this.rigidbody2D.velocity.y > Mathf.Epsilon && this.playerInput.isJumping == false)
            {
                this.rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * this.lowJumpGravityMultiplier * Time.deltaTime;
            }
            // Check if character is falling
            else if (this.rigidbody2D.velocity.y < Mathf.Epsilon)
            {
                Log(this.isLogging, Type.Message, "Character is falling.");
                this.rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * this.fallMultiplier * Time.deltaTime;
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

        private void StateController()
        {
            // Cache our ground check for use outside the class
            this.isGrounded = this.IsGrounded();

            if (GameManager.level.IsTargetWithinPlayArea(this.transform.position) == false)
            {
                this.PlayerHurt(true);
                
            }

            // Check if character is falling or jumping
            else if (this.isGrounded == false)
            {
                if (this.rigidbody2D.velocity.y > Mathf.Epsilon)
                {
                    this._state = State.Jumping;
                }
                else
                {
                    this._state = State.Falling;
                }
            }

            // Check if the character is moving
            // Mathf.Abs: Returns an absolute value converting a negative number into a positive.
            else if (Mathf.Abs(this.rigidbody2D.velocity.x) > this.slideAmount)
            {
                this._state = State.Moving;
            }
            // Character is idle
            else
            {
                this._state = State.Idle;
            }

            this.animator.SetInteger("state", (int)this._state);
        }
        #endregion
    }
}