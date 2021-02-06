using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace Baubulous.Portable.GameObjects
{
    public class Waves : BaubulousMultiBufferObject<WavesDefinition>
    {
        Texture2D ocean;

        double[] wave;
        int waveMotions;
        int[,] waveMotionOffsets;
        int waveMotionDuration_ms;
        float zDiff;
        Random rand;

        protected override void DoInit(WavesDefinition init)
        {
            base.DoInit(init);
            zDiff = init.maxZ - init.minZ;
            waveMotionDuration_ms = 10;
            rand = new Random();
            PrepareWavePattern(128);
        }

        protected override IList<IDrawable> GenerateInitChildren()
        {
            return null;
        }

        protected override void DoLoadResources(ContentManager content)
        {
            ocean = content.Load<Texture2D>(init.textureName);
        }

        protected override IEnumerable<SurfaceDefinition> GenerateSurfaces()
        {
            return new[]
            {
                CreateSurface(ocean, init.piecesX, init.piecesY, init.repetitions, init.repetitions, GenerateWaveGrid)
            };
        }

        protected VertexPositionNormalTexture[,] GenerateWaveGrid(Texture2D texture, int piecesX, int piecesY, float repsX, float repsY)
        {
            int squares = piecesX * piecesY;
            int triangles = squares * 2;

            float physicalSquareWidth = init.widthX / (float)piecesX;
            float physicalSquareDepth = init.depthY / (float)piecesY;

            float textureSquareWidth = (1.0f / (float)piecesX) * (repsX + 1);
            float textureSquareDepth = (1.0f / (float)piecesY) * (repsY + 1);

            // build grid

            var array = new VertexPositionNormalTexture[piecesX + 1, piecesY + 1];

            for (int pieceX = 0; pieceX < (piecesX + 1); pieceX++)
            {
                for (int pieceY = 0; pieceY < (piecesY + 1); pieceY++)
                {
                    // locate the minimal corner of the square and its texture
                    float square_X = pieceX * physicalSquareWidth;
                    float square_Y = pieceY * physicalSquareDepth;
                    float texture_X = pieceX * textureSquareWidth;
                    float texture_Y = pieceY * textureSquareDepth;

                    array[pieceX, pieceY] =
                        new VertexPositionNormalTexture(
                            new Vector3(square_X, square_Y, 0.0f),
                            new Vector3(), // TODO: the normal
                            new Vector2(texture_X, texture_Y));
                }
            }

            return array;
        }

        private void PrepareWavePattern(int waveMotions)
        {
            this.waveMotions = waveMotions;
            double granularity = (Math.PI * 2.0D) / (double)waveMotions;

            var motions = new List<double>();
            for (double i = 0; i < Math.PI * 2; i += granularity)
            {
                motions.Add(Math.Sin(i));
            }

            wave = motions.ToArray();
            waveMotionOffsets = new int[init.piecesX + 1, init.piecesY + 1];
            for (int x = 0; x < init.piecesX + 1; x += 2)
            {
                for (int y = 0; y < init.piecesY + 1; y+= 2)
                {
                    waveMotionOffsets[x, y] = rand.Next(0, waveMotions);
                }
            }

            // horizontal-only
            for (int x = 1; x < init.piecesX + 1; x += 2)
            {
                for (int y = 0; y < init.piecesY + 1; y += 2)
                {
                    if (x == init.piecesX + 1) { waveMotionOffsets[x, y] = rand.Next(0, waveMotions); continue; }
                    var nearby = new List<int>();
                    nearby.Add(waveMotionOffsets[x - 1, y]);
                    nearby.Add(waveMotionOffsets[x + 1, y]);
                    waveMotionOffsets[x, y] = nearby.Sum() / nearby.Count();
                }
            }

            // vertical-only
            for (int x = 0; x < init.piecesX + 1; x += 2)
            {
                for (int y = 1; y < init.piecesY + 1; y += 2)
                {
                    if (y == init.piecesY + 1) { waveMotionOffsets[x, y] = rand.Next(0, waveMotions); continue; }
                    var nearby = new List<int>();
                    nearby.Add(waveMotionOffsets[x, y - 1]);
                    nearby.Add(waveMotionOffsets[x, y + 1]);
                    waveMotionOffsets[x, y] = nearby.Sum() / nearby.Count();
                }
            }

            // 4 pointer - diagonals
            for (int x = 1; x < init.piecesX + 1; x += 2)
            {
                for (int y = 1; y < init.piecesY + 1; y += 2)
                {
                    if (y == init.piecesY + 1 || x == init.piecesX + 1) { waveMotionOffsets[x, y] = rand.Next(0, waveMotions); continue; }
                    var nearby = new List<int>();
                    nearby.Add(waveMotionOffsets[x - 1, y - 1]);
                    nearby.Add(waveMotionOffsets[x - 1, y + 1]);
                    nearby.Add(waveMotionOffsets[x + 1, y - 1]);
                    nearby.Add(waveMotionOffsets[x + 1, y + 1]);
                    waveMotionOffsets[x, y] = nearby.Sum() / nearby.Count();
                }
            }
        }

        protected override Matrix GetWorldMatrix()
        {
            return Matrix.CreateTranslation(init.offsetX, init.offsetY, 0.0f);
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
            double totalMillis = time.TotalGameTime.TotalMilliseconds;
            double totalMotions = totalMillis / waveMotionDuration_ms;
            int motion = (int)totalMotions % waveMotions;

            for (int x = 0; x < (sd.piecesX + 1); x++)
            {
                for (int y = 0; y < (sd.piecesY + 1); y++)
                {
                    int vertexMotion = (waveMotionOffsets[x, y] + motion) % waveMotions;

                    float newZ = (float)wave[vertexMotion] * zDiff;
                    sd.grid[x, y].Position.Z = newZ;

                    //if (Grid[x, y].Position.Z > maxZ) { Grid[x, y].Position.Z = maxZ; }
                    //if (Grid[x, y].Position.Z < minZ) { Grid[x, y].Position.Z = minZ; }
                }
            }

            return true;
        }
    }
}

