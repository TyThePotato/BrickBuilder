using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrickBuilder.World
{
    public class Chunk
    {
        public Mesh Mesh;
        public GameObject GameObject;
        public List<Guid> Bricks = new List<Guid>();
        public bool Locked = false;
        public bool Alpha = false;
    }
}