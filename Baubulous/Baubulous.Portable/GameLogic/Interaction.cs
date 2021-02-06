using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baubulous.Portable.GameLogic
{
    public class Interaction
    {
        public Interaction(Vector2 position, GameBoundingBox boundingBox, float worldRadius)
        {
            boundings = boundingBox;
            boundingBox.CentreAround(position);

            WorldRadius = worldRadius;
        }

        public static Matrix CalcWorldMatrix(float worldRadius, float x, float y)
        {
            return Matrix.CreateTranslation(0, -worldRadius, y) * Matrix.CreateRotationZ(x);
        }

        public static float CalcProjectedArcDistanceToGameX(float projected, float worldRadius)
        {
            var circumference = Math.PI * 2 * worldRadius;
            var proportion_of_circumference = projected / circumference;
            var gameX = proportion_of_circumference * Math.PI * 2;
            return (float)gameX;
        }

        public float WorldRadius { get; set; }

        public bool Gravity { get; set; }

        public bool Moves { get; set; }

        public bool CheckCollisions { get; set; }

        public bool Collidable { get; set; }

        public float dX { get; set; }
        public float dY { get; set; }

        private GameBoundingBox boundings;

        public GameBoundingBox BoundingBox
        {
            get
            {
                return boundings;
            }
        }

        public float cX { get { return boundings.Centre.X; } }

        public float cY { get { return boundings.Centre.Y; } }
    }
}
