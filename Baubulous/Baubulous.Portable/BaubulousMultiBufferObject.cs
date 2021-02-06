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
    public abstract class BaubulousMultiBufferObject<InitParams> : BaseLifecycleObject<InitParams> where InitParams : BaubulousMultiBufferObjectInitParams
    {
        IEnumerable<SurfaceDefinition> surfaces;

        protected override void DoInit(InitParams init)
        {
            surfaces = GenerateSurfaces();
            DoInitialiseLogic();
        }

        protected abstract void DoInitialiseLogic();

        protected abstract IEnumerable<SurfaceDefinition> GenerateSurfaces();

        public delegate VertexPositionNormalTexture[,] GridGenerator(Texture2D texture, int piecesX, int piecesY, float repsX, float repsY);

        protected SurfaceDefinition CreateSurface(Texture2D texture, int piecesX, int piecesY, float repsX, float repsY, GridGenerator generator, bool autoGenerateNormals = true)
        {
            var sd = new SurfaceDefinition()
            {
                texture = texture,
                piecesX = piecesX,
                piecesY = piecesY,
                autoGenerateNormals = autoGenerateNormals
            };

            sd.grid = generator(texture, piecesX, piecesY, repsX, repsY);

            GenerateBuffers(sd);
            UpdateBuffersFromGrid(sd);

            return sd;
        }

        protected virtual void GenerateBuffers(SurfaceDefinition sd)
        {
            sd.vertexBuffer = new VertexBuffer(graphics.GraphicsDevice, typeof(VertexPositionNormalTexture), sd.grid.Length, BufferUsage.WriteOnly);
            sd.indexBuffer = new IndexBuffer(graphics.GraphicsDevice, typeof(short), sd.grid.Length * 6, BufferUsage.WriteOnly);

            var indicesList = new List<int>();

            int cols = (int)sd.grid.GetLength(0);
            int rows = (int)sd.grid.GetLength(1);

            for (int x = 0; x < cols - 1; x++)
            {
                for (int y = 0; y < rows - 1; y++)
                {
                    indicesList.Add(((0 + x) * rows) + (0 + y));
                    indicesList.Add(((1 + x) * rows) + (0 + y));
                    indicesList.Add(((1 + x) * rows) + (1 + y));
                    indicesList.Add(((0 + x) * rows) + (0 + y));
                    indicesList.Add(((0 + x) * rows) + (1 + y));
                    indicesList.Add(((1 + x) * rows) + (1 + y));
                }
            }

            sd.indices = indicesList.Select(i => (short)i).ToArray();
            sd.indexBuffer.SetData(sd.indices);
        }

        protected void UpdateBuffersFromGrid(SurfaceDefinition sd)
        {
            sd.vertices = GeoHelper.FlattenGrid(sd.grid);
            if (sd.autoGenerateNormals) { AutoGenerateVertexNormals(sd); }
            sd.vertexBuffer.SetData(sd.vertices);
        }

        private static void AutoGenerateVertexNormals(SurfaceDefinition sd)
        {
            // zero all normals
            for (int i = 0; i < sd.vertices.Length; i++)
                sd.vertices[i].Normal = new Vector3(0, 0, 0);

            // generate a normal for each vertex on each triangle
            for (int i = 0; i < sd.indices.Length / 3; i++)
            {
                Vector3 firstvec = sd.vertices[sd.indices[i * 3 + 1]].Position - sd.vertices[sd.indices[i * 3]].Position;
                Vector3 secondvec = sd.vertices[sd.indices[i * 3]].Position - sd.vertices[sd.indices[i * 3 + 2]].Position;
                Vector3 normal = Vector3.Cross(firstvec, secondvec);
                normal.Normalize();
                sd.vertices[sd.indices[i * 3]].Normal += normal;
                sd.vertices[sd.indices[i * 3 + 1]].Normal += normal;
                sd.vertices[sd.indices[i * 3 + 2]].Normal += normal;
            }

            // normalize again
            for (int i = 0; i < sd.vertices.Length; i++)
                sd.vertices[i].Normal.Normalize();
        }

        protected abstract Matrix GetWorldMatrix();

        protected override void DoDraw(BasicEffect effect)
        {
            foreach (var sd in surfaces)
            {
                effect.VertexColorEnabled = false;
                effect.TextureEnabled = true;
                effect.Texture = sd.texture;
                effect.World = GetWorldMatrix();

                foreach (var pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    graphics.GraphicsDevice.SetVertexBuffer(sd.vertexBuffer);
                    graphics.GraphicsDevice.Indices = sd.indexBuffer;

                    graphics.GraphicsDevice.DrawIndexedPrimitives(
                        primitiveType: PrimitiveType.TriangleList,
                        baseVertex: 0,
                        minVertexIndex: 0,
                        numVertices: sd.vertices.Length,
                        startIndex: 0,
                        primitiveCount: sd.grid.Length * 2);
                }
            }
        }

        protected override void DoUpdate(GameTime time)
        {
            bool stateChanged = DoUpdateLogic(time);

            foreach (var sd in surfaces)
            {
                bool gridChanged = DoSurfaceUpdate(sd, time, stateChanged);
                if (gridChanged)
                {
                    UpdateBuffersFromGrid(sd);
                }
            }
        }

        protected abstract bool DoUpdateLogic(GameTime time);

        protected abstract bool DoSurfaceUpdate(SurfaceDefinition sd, GameTime time, bool changed);

        public class SurfaceDefinition
        {
            public Texture2D texture;
            public int piecesX;
            public int piecesY;

            public bool autoGenerateNormals;

            public VertexPositionNormalTexture[,] grid;
            public VertexBuffer vertexBuffer;
            public IndexBuffer indexBuffer;
            public VertexPositionNormalTexture[] vertices;
            public short[] indices;
        }

    }

}
