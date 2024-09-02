using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomInterface
{
    public interface IShooter
    {
        public void SetHandsOwner(HandController handsController, GameObject shooter);
        public void Shoot();
    }
}