using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baubulous.Portable.GameLogic;
using Microsoft.Xna.Framework;

namespace Baubulous.Portable.GameObjects
{
    public class BaublePlayer : BaubulousSphericalObject<BaubleInitParams>, IGameItem
    {
        protected float angle;

        protected GameState state;

        public BaublePlayer(GameState state) : base()
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
            //angle += (((float)time.ElapsedGameTime.TotalMilliseconds / 1500.0f)) * ((float)Math.PI);
            //angle += (((float)time.ElapsedGameTime.TotalMilliseconds / 1500.0f) + (Math.Abs(state.SwipeUp) / 1000.0f)) * ((float)Math.PI);
            angle += Math.Abs((1.0f + Interaction.dY) * ((float)time.ElapsedGameTime.TotalMilliseconds / 1000.0f)) * 1.5f;
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

        private float speed = 0.2f;

        public void InitInteraction(float worldRadius)
        {
            // horizontal bounds need to be calculated properly - as does the radius
            var boundingRadiusX = Interaction.CalcProjectedArcDistanceToGameX(init.radius, worldRadius);
            var boundingbox = new GameBoundingBox(-boundingRadiusX, -init.radius, boundingRadiusX * 2.0f, init.radius * 2.0f);


            boundingbox.CentreAround(new Vector2(init.start.X, init.start.Z)); // TODO -- vector3-to-vector2 -- not ideal!

            Interaction = new Interaction(boundingbox.Centre, boundingbox, worldRadius)
            {
                Moves = true,
                Gravity = true,
                dX = (float)Math.PI * speed,
                CheckCollisions = true,
                Collidable = true
            };
        }

        public void OnCollideWith(IGameItem other, bool myMove)
        {
            var platform = other as Platform;

            if (myMove && platform != null)
            {
                bool travelling_up = Interaction.dY > 0;

                if (travelling_up) // travelling up
                {
                    Interaction.BoundingBox.MoveBelow(platform.Interaction.BoundingBox);
                    Interaction.dY = 0.0f;
                    //-Math.Abs(Interaction.dY)
                }
                else
                {
                    Interaction.BoundingBox.MoveAbove(platform.Interaction.BoundingBox);
                    //Interaction.dY = Math.Abs(Interaction.dY);

                    if (Math.Abs(state.SwipeUp) > state.SwipeTolerance && state.SwipeUp < 0.0f)
                    {
                        // TODO - adjust dY
                        Interaction.dY = 1.0f + (Math.Abs(state.SwipeUp) / 1000.0f);
                        state.SwipeUp = 0.0f;
                    }
                    else
                    {
                        Interaction.dY = 1.0f;
                    }

                }
            }
        }

    }
}
