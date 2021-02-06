using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Baubulous.Portable.GameLogic;

namespace Baubulous.Portable
{
    public abstract class BaseLifecycleObject<InitParams> : IDrawable, IInitialises<InitParams>  where InitParams : BaseInitParams
    {
        protected GraphicsDeviceManager graphics;
        protected ContentManager content;
        public InitParams init;

        public IList<IDrawable> DrawingChildren;

        public virtual void Init(GraphicsDeviceManager graphics, ContentManager content, InitParams init)
        {
            this.init = init;
            this.graphics = graphics;
            this.content = content;

            DoLoadResources(content);
            DoInit(init);
            DrawingChildren = GenerateInitChildren() ?? new List<IDrawable>();
            Initialised = true;
        }

        public bool Initialised { get; private set; }

        protected abstract IList<IDrawable> GenerateInitChildren();

        protected abstract void DoInit(InitParams init);

        protected abstract void DoLoadResources(ContentManager content);

        public void Draw(BasicEffect effect)
        {
            if (!Initialised) { throw new Exception("Not initialised."); }
            DoDraw(effect);

            foreach (var child in DrawingChildren)
            {
                child.Draw(effect);
            }
        }

        protected abstract void DoDraw(BasicEffect effect);

        public void Update(GameTime time)
        {
            if (!Initialised) { throw new Exception("Not initialised."); }
            DoUpdate(time);

            foreach (var child in DrawingChildren)
            {
                child.Update(time);
            }

        }

        protected abstract void DoUpdate(GameTime time);

    }
}
