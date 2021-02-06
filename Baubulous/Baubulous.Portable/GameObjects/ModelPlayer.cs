using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Baubulous.Portable.GameObjects
{
    [Obsolete]
    public class ModelPlayer : BaubulousModel
    {
        protected override void DoInitialiseLogic()
        {
            // NOP
        }

        protected override bool DoUpdateLogic(GameTime time)
        {
            return false;
        }

        protected override IList<IDrawable> GenerateInitChildren()
        {
            return null;
        }

        protected override Matrix GetWorldMatrix()
        {
            return Matrix.Identity;
        }
    }
}
