using Baubulous.Portable.GameLogic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baubulous.Portable.GameObjects
{
    public class BaubleCollectible : BaubulousSphericalObject<BaubleInitParams>, IGameItem
    {
        protected float angle;

        protected GameState state;

        public bool Collected { get; set; }

        public bool Counted { get; set; }

        public BaubleCollectible(GameState state) : base()
        {
            this.state = state;
        }

        protected override void DoInit(BaubleInitParams init)
        {
            base.DoInit(init);
            angle = 0.0f;
        }

        protected override void DoUpdate(GameTime time)
        {
            angle += ((float)time.ElapsedGameTime.TotalMilliseconds / 1000.0f) * ((float)Math.PI);

            if (Collected)
            {
                Interaction.dY += 1.0f * ((float)time.ElapsedGameTime.TotalMilliseconds / 1000.0f);
            }
        }

        protected override IList<IDrawable> GenerateInitChildren()
        {
            return null;
        }

        protected override Matrix GetWorldMatrix()
        {
            return Matrix.CreateRotationZ(angle) * Interaction.CalcWorldMatrix(Interaction.WorldRadius, Interaction.cX, Interaction.cY);
        }

        public Vector3 GetWorldMatrixPosition()
        {
            return Interaction.CalcWorldMatrix(Interaction.WorldRadius, Interaction.cX, Interaction.cY).Translation;
        }

        public Interaction Interaction { get; private set; }

        private float speed = 0.1f;

        public void InitInteraction(float worldRadius)
        {
            // horizontal bounds need to be calculated properly - as does the radius
            var boundingRadiusX = Interaction.CalcProjectedArcDistanceToGameX(init.radius, worldRadius);
            var boundingbox = new GameBoundingBox(-boundingRadiusX, -init.radius, boundingRadiusX * 2.0f, init.radius * 2.0f);

            boundingbox.CentreAround(new Vector2(init.start.X, init.start.Z)); // TODO -- vector3-to-vector2 -- not ideal!

            Interaction = new Interaction(boundingbox.Centre, boundingbox, worldRadius)
            {
                Moves = true,
                Gravity = false,
                CheckCollisions = false,
                Collidable = true
            };
        }

        public void OnCollideWith(IGameItem other, bool myMove)
        {
            var player = other as BaublePlayer;

            if (player != null && !Collected)
            {
                Interaction.dY = 1f;
                Collected = true;
            }
        }

    }
}
