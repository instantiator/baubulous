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
    public abstract class BaubulousModel : BaseLifecycleObject<BaubulousModelInitParams>
    {
        protected Model model;
        protected Texture2D tx;

        protected abstract Matrix GetWorldMatrix();

        protected override void DoDraw(BasicEffect fk)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect fx in mesh.Effects)
                {
                    if (tx != null)
                    {
                        fx.TextureEnabled = true;
                        fx.Texture = tx;
                    }
                    else
                    {
                        fx.TextureEnabled = false;
                    }

                    fx.EnableDefaultLighting();
                    fx.AmbientLightColor = new Vector3(1.0f, 1.0f, 1.0f);
                    //fx.World = Matrix.CreateTranslation(0.0f, 600.0f, -1000.0f);
                    fx.World = Matrix.Identity;
                    fx.VertexColorEnabled = true;
                    fx.View = fk.View;
                    fx.Projection = fk.Projection;
                }

                mesh.Draw();
            }

            /*
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.TextureEnabled = true;
                    effect.Texture = tx;
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.CreateTranslation(new Vector3(0, 0, 0));
                    effect.View = Matrix.CreateLookAt(new Vector3(0, 0, 10), Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), 1.6f, 0.1f, 10000.0f);
                }

                mesh.Draw();
            }
            */
        }

        protected override void DoInit(BaubulousModelInitParams init)
        {
            DoInitialiseLogic();
        }

        protected abstract void DoInitialiseLogic();

        protected override void DoLoadResources(ContentManager content)
        {
            model = content.Load<Model>(init.modelName);
            if (init.textureName != null)
            {
                tx = content.Load<Texture2D>(init.textureName);
            }
        }

        protected override void DoUpdate(GameTime time)
        {
            bool stateChanged = DoUpdateLogic(time);
        }

        protected abstract bool DoUpdateLogic(GameTime time);
    }
}
