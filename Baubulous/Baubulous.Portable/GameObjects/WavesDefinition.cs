using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baubulous.Portable.GameObjects
{
    public class WavesDefinition : BaubulousMultiBufferObjectInitParams
    {
        public float offsetX = -10.0f;
        public float offsetY = -10.0f;

        public float widthX = 20.0f;
        public float depthY = 20.0f;

        public string textureName = "ocean_512";
        public float repetitions = 4.0f;

        public int piecesX = 32;
        public int piecesY = 32;
        public float maxZ = 0.1f;
        public float minZ = -0.1f;
    }
}
