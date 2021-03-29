using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace GravityCS
{
	public partial class GamePanel : Panel
	{
		List<Ball> balls = new List<Ball>();
		SolidBrush brush = new SolidBrush(Color.Black);
		Vector2 translation = new Vector2();
		Vector2 tempTranslation = new Vector2();
		Font font = new Font("Comic Sans MS", 10);
		Graphics g;
		const double G = 0.06743f;
		float scale = 1f;
		float tempScale = 1f;
		int currentFrameRate;
		int frameCount = 0;
		int previous_y;
		int previous_x;
		bool enabled = false;
		bool isDown = false;
		public GamePanel(int width, int height) : base()
		{
			this.Size = new Size(width, height);
			this.Location = new Point(0, 0);
			DoubleBuffered = true;
			MouseUp += new MouseEventHandler((sender, e) => isDown = false);
			MouseDown += new MouseEventHandler((sender, e) => isDown = true);
			MouseMove += new MouseEventHandler(OnMouseMove);
			MouseWheel += new MouseEventHandler(OnMouseWheel);

			PrepareBalls();
			Focus();
			InitializeComponent();
		}

		public void Start()
		{
			enabled = true;
			Thread thread = new Thread(MainLoop);
			thread.Start();
		}

		private void MainLoop()
		{
			long previousTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
			long currentTime, elapsedTime, totalElapsedTime = 0;
			while (enabled)
			{
				currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
				elapsedTime = (currentTime - previousTime);
				totalElapsedTime += elapsedTime;
				if (totalElapsedTime > 1000)
				{
					currentFrameRate = frameCount;
					frameCount = 0;
					totalElapsedTime = 0;
				}
				translation = new Vector2(tempTranslation.X, tempTranslation.Y);
				scale = tempScale;
				Update(elapsedTime / 1000f);
				Invalidate();
				previousTime = currentTime;
				frameCount++;
			}
		}

		private void PrepareBalls()
		{
			balls.Add(new Ball(6.96342f, 1988500, new Vector2(0, 0)));
			balls[0].Velocity = new Vector2();
			balls[0].Name = "Sun";
			balls.Add(new Ball(0.244f, 0.33011f, new Vector2(0, 46)));
			balls[1].SetVelocity(58.98f, 0);
			balls[1].Name = "Mercury";
			balls.Add(new Ball(0.6052f, 3.867f, new Vector2(0, 107.48f)));
			balls[2].SetVelocity(35.26f, 0);
			balls[2].Name = "Venus";
			balls.Add(new Ball(0.6378f, 5.9724f, new Vector2(0, 147.09f)));
			balls[3].SetVelocity(30.29f, 0);
			balls[3].Name = "Earth";
			balls.Add(new Ball(0.3396f, 0.64171f, new Vector2(0, 206.62f)));
			balls[4].SetVelocity(26.5f, 0);
			balls[4].Name = "Mars";
			balls.Add(new Ball(7.1492f, 1898.19f, new Vector2(0, 740.52f)));
			balls[5].SetVelocity(13.72f, 0);
			balls[5].Name = "Jupiter";
			balls.Add(new Ball(6.0268f, 568.34f, new Vector2(0, 1352.55f)));
			balls[6].SetVelocity(10.18f, 0);
			balls[6].Name = "Saturn";
			balls.Add(new Ball(2.5559f, 86.813f, new Vector2(0, 2741.3f)));
			balls[7].SetVelocity(7.11f, 0);
			balls[7].Name = "Uranus";
			balls.Add(new Ball(2.4764f, 102.413f, new Vector2(0, 4444.45f)));
			balls[8].SetVelocity(5.5f, 0);
			balls[8].Name = "Neptune";

		}

		private void Render()
		{
			DrawBackground();
			DrawPath();
			DrawBalls();
			DrawInfo();
		}

		private void DrawBackground()
		{
			brush.Color = Color.FromArgb(255, 62, 66, 75);
			g.FillRectangle(brush, 0, 0, Width, Height);
		}

		private void DrawPath()
		{
			foreach (Ball ball in balls)
			{
				for (int i = ball.Path.Count - 3; i >= 0; i--)
				{
					Color color = Color.FromArgb(255 / ball.Path.Count * i, ball.Color.R, ball.Color.G, ball.Color.B);
					ball.Path[i].Draw(g, color, ball.Path[i + 1], translation, scale);
				}
			}
		}

		private void DrawBalls()
		{
			foreach (Ball ball in balls)
			{
				ball.Draw(g, translation, scale);
				if (!ball.Name.Equals("Sun"))
					ball.Position.Draw(g, Color.Red, ball.Position + ball.Force / ball.Mass, translation, scale);
				else
					ball.Position.Draw(g, Color.Red, ball.Position + ball.Force, translation, scale);
			}
		}

		private void DrawInfo()
		{
			for (int i = 0; i < balls.Count; i++)
			{
				g.DrawString("ball " + i + " force:" + balls[i].Force.ToString(), font, brush, 15, 35 + 20 * i);
			}
		}

		public void Update(float deltaTime)
		{
			MoveBalls(deltaTime);
			CheckCollisions();
		}

		private void MoveBalls(float deltaTime)
		{
			Vector2 force;
			float dist;
			foreach (Ball ball1 in balls)
			{
				force = new Vector2();
				foreach (Ball ball2 in balls)
				{
					if (ball1.Equals(ball2))
						continue;
					dist = Vector2.Distance(ball1.Position, ball2.Position);
					force += (ball2.Position - ball1.Position).Normalize() * ((float)G * ball1.Mass * ball2.Mass / (dist * dist));
				}
				ball1.Force = force;
			}
			foreach (Ball ball in balls)
			{
				ball.SetVelocity(
					ball.Velocity.X + ball.Force.X / ball.Mass * deltaTime,
					ball.Velocity.Y + ball.Force.Y / ball.Mass * deltaTime
					);
				ball.SetPosition(
					ball.Position.X + ball.Velocity.X * deltaTime,
					ball.Position.Y + ball.Velocity.Y * deltaTime
					);

				if ((int)(-17 * ball.Velocity.Length + 1120) != 0 && frameCount % (int)(-17 * ball.Velocity.Length + 1120) == 0 /*&& (ball.Name.Equals("Neptune") || ball.Name.Equals("Uranus") || ball.Name.Equals("Saturn") || ball.Name.Equals("Jupiter"))*/)
					ball.Path.Add(new Vector2(ball.Position.X, ball.Position.Y));
				if (ball.Path.Count >= 100)
					ball.Path.RemoveAt(0);
			}
		}

		private void CheckCollisions()
		{
			foreach (Ball ball1 in balls)
			{
				//CheckWalls(ball1);
				foreach (Ball ball2 in balls)
				{
					if (ball1.Equals(ball2))
						continue;
					if (Vector2.Distance(ball1.Position, ball2.Position) < ball1.Radius + ball2.Radius)
						ConductCollision(ball1, ball2);
				}
			}
		}

		private void ConductCollision(Ball ball1, Ball ball2)
		{
			Vector2 v1 = ball1.Velocity;
			Vector2 v2 = ball2.Velocity;
			Vector2 r1 = ball1.Position;
			Vector2 r2 = ball2.Position;
			float delta = ball1.Radius + ball2.Radius - Vector2.Distance(r1, r2);
			if (ball1.Velocity.Length > ball2.Velocity.Length)
			{
				ball1.Position -= ball1.Velocity.Normalize() * delta;
				ball2.Position += ball1.Velocity.Normalize() * delta;
			}
			else
			{
				ball1.Position += ball2.Velocity.Normalize() * delta;
				ball2.Position -= ball2.Velocity.Normalize() * delta;
			}

			ball1.Velocity = v1 - (r1 - r2) * Vector2.Dot(v1 - v2, r1 - r2) / (float)(Math.Pow((r1 - r2).Length, 2)) * 2 * ball2.Mass / (ball1.Mass + ball2.Mass);
			ball2.Velocity = v2 - (r2 - r1) * Vector2.Dot(v2 - v1, r2 - r1) / (float)(Math.Pow((r2 - r1).Length, 2)) * 2 * ball1.Mass / (ball1.Mass + ball2.Mass);

		}

		private void CheckWalls(Ball ball)
		{
			float delta;
			if ((delta = ball.Position.X - 2 * ball.Radius) < 0)
			{
				ball.SetPosition(ball.Position.X - delta, ball.Position.Y);
				ball.SetVelocity(-1f * ball.Velocity.X, ball.Velocity.Y);
			}
			else if ((delta = Width - ball.Position.X - 2 * ball.Radius) < 0)
			{
				ball.SetPosition(ball.Position.X + delta, ball.Position.Y);
				ball.SetVelocity(-1f * ball.Velocity.X, ball.Velocity.Y);
			}

			if ((delta = ball.Position.Y - 2 * ball.Radius) < 0)
			{
				ball.SetPosition(ball.Position.X, ball.Position.Y - delta);
				ball.SetVelocity(ball.Velocity.X, -1f * ball.Velocity.Y);
			}
			else if ((delta = Height - ball.Position.Y - 2 * ball.Radius) < 0)
			{
				ball.SetPosition(ball.Position.X, ball.Position.Y + delta);
				ball.SetVelocity(ball.Velocity.X, -1f * ball.Velocity.Y);
			}
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
			g = pe.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			brush.Color = Color.FromArgb(255, 62, 66, 75);
			//PrepareBalls();
			Render();
		}

		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			Point point = e.Location;
			if (isDown)
			{
				tempTranslation.X += (point.X - previous_x) / scale;
				tempTranslation.Y += (point.Y - previous_y) / scale;
			}

			previous_x = point.X;
			previous_y = point.Y;
		}

		private void OnMouseWheel(object sender, MouseEventArgs e)
		{
			if (e.Delta > 0)
			{
				tempScale += tempScale * 0.2f;
			}
			else
			{
				tempScale -= tempScale * 0.2f;
			}


		}
	}
}
