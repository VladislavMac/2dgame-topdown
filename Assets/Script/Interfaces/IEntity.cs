﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomInterface
{
    public interface IEntity
    {
        public void HitEntity(float damage);
        public bool IsEntityDead();
        public void Die();
    }
}