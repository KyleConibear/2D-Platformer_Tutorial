using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KyleConibear
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class PickUp2D : MonoBehaviour
    {
        protected int quantity = 1;
        protected abstract void PickedUp();
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Character player = collision.GetComponent<Character>();
            if (player != null)
            {
                this.PickedUp();
                this.gameObject.SetActive(false);
            }
        }
    }
}