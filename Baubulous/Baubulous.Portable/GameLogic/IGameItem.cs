using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baubulous.Portable.GameLogic
{
    public interface IGameItem : IDrawable
    {
        void InitInteraction(float worldRadius);

        Interaction Interaction { get; }

        void OnCollideWith(IGameItem other, bool myMove);

        Vector3 GetWorldMatrixPosition();
    }
}
