using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KyleConibear
{
    public class GemPickUp : PickUp2D
    {
        protected override void PickedUp(PlayerCharacter player)
        {
            GameManager.level.IncrementGemCount();
        }
    }
}