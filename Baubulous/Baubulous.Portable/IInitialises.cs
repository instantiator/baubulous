using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baubulous.Portable
{
    public interface IInitialises<InitParams> where InitParams : BaseInitParams
    {
        bool Initialised { get; }

        void Init(GraphicsDeviceManager graphics, ContentManager content, InitParams parameters);
    }
}
