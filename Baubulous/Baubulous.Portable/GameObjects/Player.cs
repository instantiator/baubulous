using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Baubulous.Portable.GameObjects
{
    public class Player : BaubulousMultiBufferObject<PlayerDefinition>
    {
        protected Vector3 position;
        protected double angle;
        protected Texture2D skin;

        protected override void DoInitialiseLogic()
        {
            // TODO -- use game coordinate conversions for this -- same for platforms (soon!)

            angle = 0.0D;
            position = new Vector3(init.start.X, init.start.Y, init.start.Z);
        }

        protected override void DoLoadResources(ContentManager content)
        {
            skin = content.Load<Texture2D>(init.texture);
        }

        protected override bool DoSurfaceUpdate(SurfaceDefinition sd, GameTime time, bool changed)
        {
            return false; // player doesn't animate (for now)
        }

        protected override bool DoUpdateLogic(GameTime time)
        {
            // if the player moves in game coordinates (simpler, actually 2D) we update the position
            // use a 2D-to-3D mapping helper so that all game objects can do this
            // use the same helper to adjust platform start/finish ... or just work on angles?

            angle += Math.PI * 2 * time.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            return false; // stuff might happen and if it affects the grid, we should pass back true
        }

        protected override IList<IDrawable> GenerateInitChildren()
        {
            return null;
        }

        protected override IEnumerable<SurfaceDefinition> GenerateSurfaces()
        {
            return new[]
            {
                CreateSurface(skin, 24,7, 1, 1, GenerateSphere)
            };

        }

        private VertexPositionNormalTexture[,] GenerateSphere(Texture2D tex, int piecesX, int piecesY, float repsX, float repsY)
        {
            var zStep = (Math.PI) / piecesY;
            var aStep = (Math.PI * 2) / piecesX;

            var grid = new VertexPositionNormalTexture[piecesX + 1, piecesY + 1];

            var texStepY = (1.0f * repsY) / piecesY;
            var texStepX = (1.0f * repsX) / piecesX;

            for (int lat = 0; lat <= piecesY; lat++)
            {
                double vertAngle = lat * zStep;
                double z = Math.Cos(vertAngle) * init.radius;
                double r = Math.Sin(vertAngle) * init.radius;

                float tY = lat * texStepY;

                for (int lon = 0; lon <= piecesX; lon++)
                {
                    double horizAngle = lon * aStep;
                    double x = Math.Sin(horizAngle) * r;
                    double y = Math.Cos(horizAngle) * r;

                    float tX = lat * texStepX;

                    grid[lon, lat] = new VertexPositionNormalTexture(
                        new Vector3((float)x, (float)y, (float)z),
                        new Vector3(0,0,0), // normal
                        new Vector2(tX, tY)
                    );
                }
            }

            return grid;
        }

        protected override Matrix GetWorldMatrix()
        {
            return Matrix.CreateRotationZ((float)angle) * Matrix.CreateTranslation(position);
        }
    }
}
