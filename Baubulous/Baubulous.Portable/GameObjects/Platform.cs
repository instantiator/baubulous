using Baubulous.Portable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Baubulous.Portable.GameLogic;

namespace Baubulous.Portable.GameObjects
{
    public class Platform : BaubulousMultiBufferObject<PlatformDefinition>, IGameItem
    {
        Texture2D top_texture;
        Texture2D long_side_texture;

        //protected List<double[]> circle_inner;
        //protected List<double[]> circle_outer;

        List<Vector3> circle_inner;
        List<Vector3> circle_outer;

        protected override IEnumerable<SurfaceDefinition> GenerateSurfaces()
        {
            int piecesX = 32;
            int piecesY = 1;

            circle_inner = GeoHelper.GenerateCircleVectors(piecesX, init.min_radius, init.start_angle, init.end_angle);
            circle_outer = GeoHelper.GenerateCircleVectors(piecesX, init.max_radius, init.start_angle, init.end_angle);

            var surfaces = new List<SurfaceDefinition>();

            double delta_angle = init.end_angle - init.start_angle; // total sweep
            double delta_angle_per_X = delta_angle / piecesX; // change in angle per piece
            float repsX = (float) (delta_angle / (Math.PI / 4.0D)); // adjust to fit
            float repsY_top = 1.0f;
            float repsY_long_side = 0.3f;

            surfaces.Add(CreateSurface(top_texture, piecesX, piecesY, repsX, repsY_top, GenerateTopSurfaceGrid, false));
            surfaces.Add(CreateSurface(long_side_texture, piecesX, piecesY, repsX * 4, repsY_long_side, GenerateSideSurfaceGrid, false));
            surfaces.Add(CreateSurface(top_texture, piecesX, piecesY, repsX, repsY_top, GenerateBottomSurfaceGrid, false));

            // TODO: bottom? other sides?

            return surfaces;
        }

        protected VertexPositionNormalTexture[,] GenerateTopSurfaceGrid(Texture2D texture, int piecesX, int piecesY, float repsX, float repsY)
        {
            if (piecesY != 1) { throw new ArgumentOutOfRangeException("piecesY should be 1"); }

            var array = new VertexPositionNormalTexture[piecesX + 1, piecesY + 1]; // full enclosing grid of vertices

            double delta_angle = init.end_angle - init.start_angle; // total sweep
            double delta_angle_per_X = delta_angle / piecesX; // change in angle per piece

            for (int x = 0; x <= piecesX; x++)
            {
                float texturePositionX = (float)((1.0f * repsX) / piecesX) * x;

                array[x, 0] = new VertexPositionNormalTexture(
                    circle_inner[x % piecesX],
                    new Vector3(0.0f, 0.0f, 1.0f),
                    new Vector2(texturePositionX, 0.0f));
                array[x, 1] = new VertexPositionNormalTexture(
                    circle_outer[x % piecesX],
                    new Vector3(0.0f, 0.0f, 1.0f),
                    new Vector2(texturePositionX, 1.0f * (float)repsY));
            }

            return array;
        }

        protected VertexPositionNormalTexture[,] GenerateBottomSurfaceGrid(Texture2D texture, int piecesX, int piecesY, float repsX, float repsY)
        {
            if (piecesY != 1) { throw new ArgumentOutOfRangeException("piecesY should be 1"); }

            var array = new VertexPositionNormalTexture[piecesX + 1, piecesY + 1]; // full enclosing grid of vertices

            double delta_angle = init.end_angle - init.start_angle; // total sweep
            double delta_angle_per_X = delta_angle / piecesX; // change in angle per piece

            for (int x = 0; x <= piecesX; x++)
            {
                float texturePositionX = (float)((1.0f * repsX) / piecesX) * x;

                var inner = new Vector3(
                    circle_inner[x % piecesX].X,
                    circle_inner[x % piecesX].Y,
                    -init.height);

                var outer = new Vector3(
                    circle_outer[x % piecesX].X,
                    circle_outer[x % piecesX].Y,
                    -init.height);


                array[x, 0] = new VertexPositionNormalTexture(
                    inner,
                    new Vector3(0.0f, 0.0f, -1.0f),
                    new Vector2(texturePositionX, 0.0f));
                array[x, 1] = new VertexPositionNormalTexture(
                    outer,
                    new Vector3(0.0f, 0.0f, -1.0f),
                    new Vector2(texturePositionX, 1.0f * (float)repsY));
            }

            return array;
        }

        protected VertexPositionNormalTexture[,] GenerateSideSurfaceGrid(Texture2D texture, int piecesX, int piecesY, float repsX, float repsY)
        {
            if (piecesY != 1) { throw new ArgumentOutOfRangeException("piecesY should be 1"); }

            var array = new VertexPositionNormalTexture[piecesX + 1, piecesY + 1]; // full enclosing grid of vertices

            double delta_angle = init.end_angle - init.start_angle; // total sweep
            double delta_angle_per_X = delta_angle / piecesX; // change in angle per piece

            for (int x = 0; x <= piecesX; x++)
            {
                float texturePositionX = (float)((1.0f * repsX) / piecesX) * x;

                var upper = circle_outer[x % piecesX];
                var lower = new Vector3(upper.X, upper.Y, upper.Z - init.height);

                var normal = new Vector3(upper.X, upper.Y, 0.0f);
                normal.Normalize();

                array[x, 0] = new VertexPositionNormalTexture(
                    upper,
                    normal,
                    new Vector2(texturePositionX, 0.0f));
                array[x, 1] = new VertexPositionNormalTexture(
                    lower,
                    normal,
                    new Vector2(texturePositionX, 1.0f * (float)repsY));
            }

            return array;
        }

        protected override Matrix GetWorldMatrix()
        {
            return Matrix.CreateTranslation(init.cX, init.cY, init.topZ);
        }

        public Vector3 GetWorldMatrixPosition()
        {
            return GetWorldMatrix().Translation;
        }

        protected override void DoInitialiseLogic()
        {
            // NOP
        }

        protected override bool DoUpdateLogic(GameTime time)
        {
            return false;
        }

        protected override bool DoSurfaceUpdate(SurfaceDefinition sd, GameTime time, bool changed)
        {
            return false;
        }

        protected override IList<IDrawable> GenerateInitChildren()
        {
            return null;
        }

        protected override void DoLoadResources(ContentManager content)
        {
            top_texture = content.Load<Texture2D>(init.top_texture);
            long_side_texture = content.Load<Texture2D>(init.long_side_texture);
        }

        public Interaction Interaction { get; private set; }

        public void InitInteraction(float worldRadius)
        {
            var boundingbox = new GameBoundingBox(init.start_angle, init.topZ - init.height, init.end_angle - init.start_angle, init.height);

            Interaction = new Interaction(
                boundingbox.Centre, 
                boundingbox,
                worldRadius)
            {
                Collidable = true,
                CheckCollisions = false,
                Moves = false
            };
        }

        public void OnCollideWith(IGameItem other, bool myMove)
        {
            // NOP -- don't react
        }
    }
}
