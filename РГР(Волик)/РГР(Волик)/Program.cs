using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ClockApp
{
    public class Program : Form
    {
        Timer timer;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Program());
        }

        public Program()
        {
            this.Text = "Графічний годинник";
            this.Width = 400;
            this.Height = 400;
            this.DoubleBuffered = true;

            timer = new Timer();
            timer.Interval = 1000; // 1 second
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.Invalidate(); // Force the form to be redrawn
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Get the current time
            DateTime now = DateTime.Now;
            int hours = now.Hour;
            int minutes = now.Minute;
            int seconds = now.Second;

            // Define the center of the clock
            float centerX = this.ClientSize.Width / 2;
            float centerY = this.ClientSize.Height / 2;
            float radius = Math.Min(centerX, centerY) - 10;

            // Draw the clock face
            g.DrawEllipse(Pens.Black, centerX - radius, centerY - radius, 2 * radius, 2 * radius);

            // Draw hour markers and numbers
            for (int i = 1; i <= 12; i++)
            {
                float angle = (i * 30 - 90) * (float)Math.PI / 180; // Adjust angle for correct position
                float x1 = centerX + (float)Math.Cos(angle) * (radius - 10);
                float y1 = centerY + (float)Math.Sin(angle) * (radius - 10);
                float x2 = centerX + (float)Math.Cos(angle) * radius;
                float y2 = centerY + (float)Math.Sin(angle) * radius;
                g.DrawLine(Pens.Black, x1, y1, x2, y2);

                // Draw numbers
                string number = i.ToString();
                Font font = new Font("Arial", 12);
                SizeF numberSize = g.MeasureString(number, font);
                float xNumber = centerX + (float)Math.Cos(angle) * (radius - 25) - numberSize.Width / 2;
                float yNumber = centerY + (float)Math.Sin(angle) * (radius - 25) - numberSize.Height / 2;
                g.DrawString(number, font, Brushes.Black, xNumber, yNumber);
            }

            // Draw the hour hand
            float hourAngle = (hours % 12 + minutes / 60f) * 30 * (float)Math.PI / 180 - (float)Math.PI / 2;
            DrawHand(g, centerX, centerY, hourAngle, radius * 0.5f, 6);

            // Draw the minute hand
            float minuteAngle = (minutes + seconds / 60f) * 6 * (float)Math.PI / 180 - (float)Math.PI / 2;
            DrawHand(g, centerX, centerY, minuteAngle, radius * 0.75f, 4);

            // Draw the second hand
            float secondAngle = seconds * 6 * (float)Math.PI / 180 - (float)Math.PI / 2;
            DrawHand(g, centerX, centerY, secondAngle, radius * 0.85f, 2, Pens.Red);
        }

        private void DrawHand(Graphics g, float centerX, float centerY, float angle, float length, float width, Pen pen = null)
        {
            if (pen == null)
                pen = new Pen(Color.Black, width);
            float x = centerX + (float)Math.Cos(angle) * length;
            float y = centerY + (float)Math.Sin(angle) * length;
            g.DrawLine(pen, centerX, centerY, x, y);
        }
    }
}
