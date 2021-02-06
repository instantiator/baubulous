using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baubulous.Portable.GameLogic
{
    public class GameBoundingBox
    {
        public GameBoundingBox(float left, float bottom, float width, float height)
        {
            Left = left;
            Bottom = bottom;
            Width = width;
            Height = height;
        }

        public float Left { get; set; }

        public float Bottom { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public float Right { get { return Left + Width; } set { Left = value - Width; } }

        public float Top { get { return Bottom + Height; } set { Bottom = value - Height; } }

        public Vector2 Centre
        {
            get
            {
                return new Vector2(Left + Width / 2f, Bottom + Height / 2f);
            }
        }

        public bool Contains(GameBoundingBox compare)
        {
            return 
                Contains(compare.Left, compare.Top) ||
                Contains(compare.Right, compare.Top) ||
                Contains(compare.Left, compare.Bottom) ||
                Contains(compare.Right, compare.Bottom);
        }

        public bool Contains(float x, float y)
        {
            return (Left < x && x < Right) && (Bottom < y && y < Top);
        }

        public bool Intersects(GameBoundingBox compare, bool specialPiTwoRuleForX)
        {
            if (!specialPiTwoRuleForX)
            {
                throw new ArgumentException("specialPiTwoRuleForX -- should always be true");
            }

            bool tooHigh = compare.Bottom >= Top;
            bool tooLow = compare.Top <= Bottom;

            bool tooLeft;
            bool tooRight;

            if (!specialPiTwoRuleForX)
            {
                tooLeft = compare.Right <= Left;
                tooRight = compare.Left >= Right;
            }
            else
            {
                float twoPI = (float)Math.PI * 2f;
                tooLeft = (compare.Right % twoPI) <= (Left % twoPI);
                tooRight = (compare.Left % twoPI) >= (Right % twoPI);
            }

            return !tooHigh && !tooLeft && !tooRight && !tooLow;
        }

        public float Below(GameBoundingBox other)
        {
            return other.Bottom - Height;
        }

        public float Above(GameBoundingBox other)
        {
            return other.Top - Bottom;
        }

        public void CentreAround(Vector2 centre)
        {
            Left = centre.X - Width / 2f;
            Bottom = centre.Y - Height / 2f;
        }

        public void MoveBelow(GameBoundingBox other)
        {
            Top = other.Bottom;
        }

        public void MoveAbove(GameBoundingBox other)
        {
            Bottom = other.Top;
        }

    }
}
