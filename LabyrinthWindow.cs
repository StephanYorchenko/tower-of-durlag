using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LabirintDemoGame
{
    public class LabyrinthWindow : Form
    {
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private readonly Game game;
        private const int size = 64;

        public LabyrinthWindow(Game game, DirectoryInfo imagesDirectory = null)
        {
            this.game = game;
            ClientSize = new Size(1028, 640);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            if (imagesDirectory == null)
                imagesDirectory = new DirectoryInfo("Images");
            foreach (var e in imagesDirectory.GetFiles("*.png"))
                bitmaps[e.Name.Substring(0, e.Name.Length - 4)] = (Bitmap) Image.FromFile(e.FullName);
            var timer = new Timer();
            timer.Interval = 1;
            timer.Tick += TimerTick;
            timer.Start();
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = "Tower of Durlag";
            DoubleBuffered = true;
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            if (game.EndGame)
                Close();

            foreach (var t in game.Map)
            {
                e.Graphics.DrawImage(bitmaps["Empty"], new Point(t.X * size, t.Y * size));
                if (t.Type != CellTypes.Player)
                    e.Graphics.DrawImage(bitmaps[t.Type.ToString()], new Point(t.X * size, t.Y * size));
                if (!t.IsExplored)
                    e.Graphics.FillRectangle(
                        Brushes.Black, t.X * size, t.Y * size, size, size);

            }

            e.Graphics.DrawImage(bitmaps["Player"], 
                new Point(game.PlayerPosition.X * size, game.PlayerPosition.Y * size));
            e.Graphics.ResetTransform();
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    game.MakePlayerMove(Directions.Up);
                    break;
                case Keys.Down:
                    game.MakePlayerMove(Directions.Down);
                    break;
                case Keys.Left:
                    game.MakePlayerMove(Directions.Left);
                    break;
                case Keys.Right:
                    game.MakePlayerMove(Directions.Right);
                    break;
            }
        }
        
        private void TimerTick(object sender, EventArgs args)
        {
            Invalidate();
        }
    }
}