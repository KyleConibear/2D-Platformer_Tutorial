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
            Player player = collision.GetComponent<Player>();
            if (player != null && player.state != Player.State.Hurt)
            {
                this.PickedUp();
                this.gameObject.SetActive(false);
            }
        }
    }
}