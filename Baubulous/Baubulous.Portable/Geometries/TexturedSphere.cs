using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baubulous.Portable.Geometries
{
    public class TexturedSphere
    {
        VertexPositionNormalTexture[] vertices;
        VertexBuffer vbuffer;
        Texture2D texture;

        short[] indices;
        IndexBuffer ibuffer;
        float radius;
        int nvertices, nindices;
        GraphicsDevice graphics;

        public TexturedSphere(float radius, GraphicsDevice graphics, Texture2D texture)
        {
            this.texture = texture;
            this.radius = radius;
            this.graphics = graphics;

            nvertices = 90 * 90; // 90 vertices in a circle, 90 circles in a sphere
            nindices = 90 * 90 * 6;

            vbuffer = new VertexBuffer(graphics, typeof(VertexPositionNormalTexture), nvertices, BufferUsage.WriteOnly);
            ibuffer = new IndexBuffer(graphics, IndexElementSize.SixteenBits, nindices, BufferUsage.WriteOnly);

            CreateVertices();
            CreateIndices();
            
            vbuffer.SetData(vertices);
            ibuffer.SetData(indices);
        }

        void CreateVertices()
        {
            vertices = new VertexPositionNormalTexture[nvertices];
            Vector3 rad = new Vector3((float)Math.Abs(radius), 0, 0);

            float difx = 360.0f / 90.0f;
            float dify = 360.0f / 90.0f;

            for (int x = 0; x < 90; x++) // circles
            {
                for (int y = 0; y < 90; y++) // vertices
                {
                    Matrix zrot = Matrix.CreateRotationZ(MathHelper.ToRadians(y * dify));  //rotate vertex around z
                    Matrix yrot = Matrix.CreateRotationY(MathHelper.ToRadians(x * difx));  //rotate circle around y

                    Vector3 point = 
                        Vector3.Transform(Vector3.Transform(rad, zrot), yrot); //transformation

                    float facing = y * dify;
                    float tilt = x * difx;

                    var normal = new Vector3(point.X, point.Y, point.Z);
                    normal.Normalize();

                    var tu = (float) (Math.Asin(normal.Z) / Math.PI + 0.5f);
                    var tv = 1.0f - (float) (Math.Asin(normal.X) / Math.PI + 0.5f);
                    var tex = new Vector2(tu, tv);

                    vertices[x + y * 90] = new VertexPositionNormalTexture(point, normal, tex);
                }
            }
        }

        private void CreateIndices()
        {
            indices = new short[nindices];
            int i = 0;
            for (int x = 0; x < 90; x++)
            {
                for (int y = 0; y < 90; y++)
                {
                    int s1 = x == 89 ? 0 : x + 1;
                    int s2 = y == 89 ? 0 : y + 1;
                    short upperLeft = (short)(x * 90 + y);
                    short upperRight = (short)(s1 * 90 + y);
                    short lowerLeft = (short)(x * 90 + s2);
                    short lowerRight = (short)(s1 * 90 + s2);
                    indices[i++] = upperLeft;
                    indices[i++] = upperRight;
                    indices[i++] = lowerLeft;
                    indices[i++] = lowerLeft;
                    indices[i++] = upperRight;
                    indices[i++] = lowerRight;
                }
            }
        }

        public void Draw(BasicEffect effect)
        {
            effect.TextureEnabled = true;
            effect.Texture = texture;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, nvertices, indices, 0, indices.Length / 3);
            }
        }
    }
}
