using Baubulous.Portable.Geometries;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baubulous.Portable
{
    public abstract class BaubulousSphericalObject<T> : BaseLifecycleObject<T> where T : BaubulousSphericalInitParams
    {
        protected TexturedSphere primitive;
        protected Texture2D tex;

        protected override void DoLoadResources(ContentManager content)
        {
            tex = content.Load<Texture2D>(init.texture);
        }

        protected override void DoInit(T init)
        {
            primitive = new TexturedSphere(init.radius, graphics.GraphicsDevice, tex);
        }

        protected override void DoDraw(BasicEffect effect)
        {
            effect.World = GetWorldMatrix();
            primitive.Draw(effect);
        }

        protected abstract Matrix GetWorldMatrix();
    }
}
