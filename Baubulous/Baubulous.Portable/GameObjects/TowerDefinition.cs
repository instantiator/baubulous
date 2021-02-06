using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baubulous.Portable.GameObjects
{
    public class TowerDefinition : BaubulousMultiBufferObjectInitParams
    {
        public float cX;
        public float cY;
        public float baseZ;
        public float radius;
        public float height;

        public string textureName = "brickwork";
        public float texture_reps_X = 12.0f;
        public float texture_reps_Y = 8.0f;
    }
}
