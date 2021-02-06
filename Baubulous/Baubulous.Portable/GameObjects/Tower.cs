using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baubulous.Portable.GameObjects
{
    public class Tower : BaubulousMultiBufferObject<TowerDefinition>
    {

        private int wall_piecesX = 64;
        private int wall_piecesY = 1;

        private Texture2D wall_texture;
        private float wall_repsX;
        private float wall_repsY;

        public Tower() : base()
        {
        }

        protected override void DoInitialiseLogic()
        {
            // game stuff
        }

        protected override IEnumerable<SurfaceDefinition> GenerateSurfaces()
        {
            return new []
            {
                CreateSurface(wall_texture, wall_piecesX, wall_piecesY, wall_repsX, wall_repsY, GenerateWallGrid, false)
            };
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
            wall_texture = content.Load<Texture2D>(init.textureName);
            wall_repsX = init.texture_reps_X;
            wall_repsY = init.texture_reps_Y;
        }

        protected VertexPositionNormalTexture[,] GenerateWallGrid(Texture2D texture, int piecesX, int piecesY, float repsX, float repsY)
        {
            var circle = GeoHelper.GenerateCircle(piecesX, init.radius);

            var array = new VertexPositionNormalTexture[piecesX + 1, piecesY + 1];

            for (int x = 0; x < piecesX + 1; x++)
            {
                float texturePositionX = ((1.0f * repsX) / piecesX) * x;

                var bottom = new Vector3((float)circle[x % piecesX][0], (float)circle[x % piecesX][1], 0.0f);
                var top = new Vector3((float)circle[x % piecesX][0], (float)circle[x % piecesX][1], init.height);

                var normal = new Vector3(bottom.X, bottom.Y, 0.0f);
                normal.Normalize();

                array[x, 0] = new VertexPositionNormalTexture(
                    bottom,
                    normal,
                    new Vector2(texturePositionX, 0.0f));
                array[x, 1] = new VertexPositionNormalTexture(
                    top,
                    normal,
                    new Vector2(texturePositionX, 1.0f * repsY));
            }

            return array;
        }

        protected override Matrix GetWorldMatrix()
        {
            // no need for instance variables, doesn't move
            return Matrix.CreateTranslation(init.cX, init.cY, init.baseZ);
        }

    }
}
