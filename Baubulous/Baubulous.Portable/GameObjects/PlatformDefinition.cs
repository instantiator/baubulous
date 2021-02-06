using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baubulous.Portable.GameObjects
{
    public class PlatformDefinition : BaubulousMultiBufferObjectInitParams
    {
        public float cX;
        public float cY;
        public float topZ;
        public float height;
        public float min_radius;
        public float max_radius;
        public float start_angle;
        public float end_angle;

        public string top_texture;
        public string long_side_texture;
        public string short_side_texture;
        public string underside_texture;
    }
}
