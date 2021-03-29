using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravityCS
{
    class Vector2
    {
        private float x;
        public float X
        {
            get { return x; }
            set { x = value; }
        }
        private float y;
        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public float Length
        {
            get { return (float) Math.Sqrt(X * X + Y * Y); }
        }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2()
        {
            X = 0;
            Y = 0;
        }

        public void Set(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return
                new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2 operator *(Vector2 v1, float scaleFactor)
        {
            if (float.IsNaN(scaleFactor))
                throw new ArgumentException("Argument is not a number");
            if (float.IsInfinity(scaleFactor))
                throw new ArgumentException("Argument is infinity");

            return
                new Vector2(v1.X * scaleFactor, v1.Y * scaleFactor);
        }

        public static Vector2 operator /(Vector2 v1, float scaleFactor)
        {
            if (float.IsNaN(scaleFactor))
                throw new ArgumentException("Argument is not a number");
            if (float.IsInfinity(scaleFactor))
                throw new ArgumentException("Argument is infinity");
            if (scaleFactor == 0f)
                throw new ArgumentException("You cannot divide by 0");

            return
                new Vector2(v1.X / scaleFactor, v1.Y / scaleFactor);
        }

        public Vector2 Normalize()
        {
            Vector2 result = new Vector2(X, Y);
            float len = result.Length;
            if (len != 0f)
            {
                result /= len;
            } 
            else
            {
                result.Set(0, 0);
            }
            return result;
        }

        public override String ToString()
        {
            return "x: " + X + "y: " + Y;
        }

        public void Draw(Graphics g, Color color, Vector2 v2, Vector2 translation, float scale)
        {
            Pen pen = new Pen(color);
            g.DrawLine(pen,
                (X + translation.X) * scale,
                (Y + translation.Y) * scale,
                (v2.X + translation.X) * scale,
                (v2.Y + translation.Y) * scale
                );
        }

        public static float Distance(Vector2 v1, Vector2 v2)
        {
            return (float)Math.Sqrt(Math.Pow(v1.X - v2.X, 2) + Math.Pow(v1.Y - v2.Y, 2));
        }

        public static float Dot(Vector2 v1, Vector2 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }


    }
}
