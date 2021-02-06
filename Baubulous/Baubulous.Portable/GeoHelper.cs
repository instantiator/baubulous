using Baubulous.Portable.GameLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baubulous.Portable
{
    public class GeoHelper
    {

        public static List<double[]> GenerateCircle(int pieces, double radius)
        {
            return GenerateCircleSection(pieces, radius, 0.0D, Math.PI * 2.0D);
        }

        public static List<Vector3> GenerateCircleVectors(int pieces, double radius, double start_angle, double end_angle)
        {
            var vectors = new List<Vector3>();
            double delta = (end_angle - start_angle) / pieces;
            for (double angle = start_angle; angle <= end_angle; angle += delta)
            {
                vectors.Add(Interaction.CalcWorldMatrix((float)radius, (float)angle, 0.0f).Translation);
            }
            return vectors;
        }

        public static List<double[]> GenerateCircleSection(int pieces, double radius, double start_angle, double end_angle)
        {
            var circle = new List<double[]>();
            double delta = (end_angle - start_angle) / pieces;
            for (double angle = start_angle; angle < end_angle; angle += delta)
            {
                circle.Add(new double[2] { Math.Sin(angle) * radius, Math.Cos(angle) * radius });
            }
            return circle;
        }

        public static VertexPositionNormalTexture[] FlattenGrid(VertexPositionNormalTexture[,] array)
        {
            int width = array.GetLength(0);
            int height = array.GetLength(1);
            var ret = new VertexPositionNormalTexture[width * height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    ret[(x * height) + y] = array[x, y];
                }
            }
            return ret;
        }
    }
}
