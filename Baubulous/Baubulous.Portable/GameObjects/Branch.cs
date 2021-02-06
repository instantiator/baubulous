using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baubulous.Portable
{
    /*
    public class Branch : BufferPrimitiveObject
    {
        float cX;
        float cY;
        float innerRadius;
        float direction;
        float height;
        float length;
        float radius;
        int repetitionsX;
        int repetitionsY;

        public Branch(GraphicsDeviceManager graphics, float cX, float cY, float direction, float height, float innerRadius, float length, float radius, Texture2D texture, int repetitionsX, int repetitionsY)
            : base(graphics, texture, 16, 1)
        {
            this.cX = cX;
            this.cY = cY;
            this.direction = direction;
            this.height = height;
            this.innerRadius = innerRadius;
            this.length = length;
            this.radius = radius;
            this.repetitionsX = repetitionsX;
            this.repetitionsY = repetitionsY;
        }

        protected override VertexPositionNormalTexture[,] GenerateGrid()
        {
            var circle = new List<double[]>();

            double delta = Math.PI * 2 / piecesX;
            for (double r = 0; r < Math.PI * 2; r += delta)
            {
                circle.Add(new double[2] { Math.Sin(r) * radius, Math.Cos(r) * radius });
            }

            var array = new VertexPositionNormalTexture[piecesX + 1, piecesY + 1];

            for (int x = 0; x < piecesX + 1; x++)
            {
                float texturePositionX = ((1.0f * repetitionsX) / piecesX) * x;

                // horizontal cylinder
                array[x, 0] = new VertexPositionNormalTexture(
                    new Vector3((float)circle[x % piecesX][0], innerRadius, (float)circle[x % piecesX][1]),
                    new Vector3(), // TODO: the normal
                    new Vector2(texturePositionX, 0.0f));
                array[x, 1] = new VertexPositionNormalTexture(
                    new Vector3((float)circle[x % piecesX][0], innerRadius + length, (float)circle[x % piecesX][1]),
                    new Vector3(), // TODO: the normal
                    new Vector2(texturePositionX, 1.0f * repetitionsY));
            }

            return array;
        }

        protected override bool DoUpdate(GameTime time)
        {
            return false;
        }

        protected override Matrix GetWorldMatrix()
        {
            // rotate around 0,0 ... then translate 0,0->cX,cY (and raise by height)
            return Matrix.CreateRotationZ(direction) * Matrix.CreateTranslation(cX, cY, height);
        }

    }
    */
}
