using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KyleConibear
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class PickUp2D : MonoBehaviour
    {
        protected int quantity = 1;
        protected abstract void PickedUp(PlayerCharacter player);
        private void OnTriggerEnter2D(Collider2D collision)
        {
            PlayerCharacter player = collision.GetComponent<PlayerCharacter>();
            if (player != null)
            {
                this.PickedUp(player);
                this.gameObject.SetActive(false);
            }
        }
    }
}