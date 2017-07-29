﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu.Game.Rulesets.Shape.Objects
{
    public class BaseShape : ShapeHitObject
    {
        public override HitObjectType Type => HitObjectType.Shape;
        public Vector2 StartPosition { get; set; }
        public float ShapeSize { get; set; } = 40;
        public int ShapeID { get; set; }//1 - circle | 2 - square | 3 - 
    }
}