using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KyleConibear
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Collider2D))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private int bonusTime = 3;
        [SerializeField] private float movementSpeed = 1.0f;
        private int flyPositionIndex = 0;
        [SerializeField] private List<Vector2> flyPositions = new List<Vector2>();
        [SerializeField] private float delayBetweenPositions = 0.0f;
        private IEnumerator goToNextPosition = null;
        private Animator animator = null;

        public enum State
        {
            idle = 0,
            moving = 1
        }

        private void Awake()
        {
            this.animator = this.GetComponent<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {
            this.transform.position = this.flyPositions[this.flyPositionIndex];
        }

        // Update is called once per frame
        void Update()
        {
            this.Move();
        }

        private IEnumerator GoToNextPosition()
        {
            yield return new WaitForSeconds(this.delayBetweenPositions);
            flyPositionIndex++;
            flyPositionIndex = this.flyPositionIndex % flyPositions.Count;
            goToNextPosition = null;
        }

        private void Move()
        {
            Vector3 target = this.flyPositions[this.flyPositionIndex];
            if (Vector2.Distance(this.transform.position, target) > Mathf.Epsilon)
            {
                this.animator.SetInteger("state", 1);
                this.transform.position = Vector2.MoveTowards(this.transform.position, target, this.movementSpeed * Time.deltaTime);
            }
            else
            {
                if (goToNextPosition == null)
                {
                    this.animator.SetInteger("state", 0);
                    goToNextPosition = this.GoToNextPosition();
                    StartCoroutine(goToNextPosition);
                }
            }
        }

        public void Kill()
        {
            GameManager.level.IncreaseTime(this.bonusTime);
            this.gameObject.SetActive(false);
        }
    }
}