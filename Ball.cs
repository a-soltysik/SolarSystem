using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravityCS
{
    class Ball
    {
        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        private Vector2 velocity;
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        private Vector2 force;
        public Vector2 Force
        {
            get { return force; }
            set { force = value; }
        }
        private float radius;
        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }
        private float mass;
        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }
        private List<Vector2> path;
        public List<Vector2> Path
        {
            get { return path; }
            set { path = value; }
        }
        private Color color;
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        private String name;
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public static Random random = new Random();

        public Ball() : this(50f, 1f, new Vector2(), RandomColor())
        {
            
        }

        public Ball(float radius, float mass, Vector2 position) : this(radius, mass, position, RandomColor())
        {

        }
        public Ball(float radius, float mass, Vector2 position, Color color)
        {
            Radius = radius;
            Position = position;
            Mass = mass;
            Color = color;
            Velocity = new Vector2();
            Force = new Vector2();
            Path = new List<Vector2>();
            Name = "";
        }

        public static Color RandomColor()
        {
            return Color.FromArgb(255,
                random.Next(128, 255),
                random.Next(128, 255),
                random.Next(128, 255)
                );
        }

        public void Draw(Graphics g, Vector2 translation, float scale)
        {
            SolidBrush brush = new SolidBrush(Color);
            g.FillEllipse(brush,
                (Position.X + translation.X - Radius) * scale,
                (Position.Y + translation.Y - Radius) * scale,
                2 * Radius * scale,
                2 * Radius * scale
                );
            brush.Color = Color.Black;
            Font font = new Font("Cascadia Code", 10);
            g.DrawString(name, font, brush,
                (Position.X - Radius + translation.X) * scale,
                (Position.Y - Radius + translation.Y) * scale
                );
  
        }

        public void SetPosition(float x, float y)
        {
            Position.X = x;
            Position.Y = y;
        }

        public void SetVelocity(float x, float y)
        {
            Velocity.X = x;
            Velocity.Y = y;
        }

        public void SetForce(float x, float y)
        {
            Force.X = x;
            Force.Y = y;
        }
    }
}
