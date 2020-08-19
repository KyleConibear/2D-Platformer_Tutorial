using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KyleConibear
{
    public class CherryPickUp : PickUp2D
    {
        protected override void PickedUp()
        {
            GameManager.level.IncrementCherryCount();
        }
    }
}