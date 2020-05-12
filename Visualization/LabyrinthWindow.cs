using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using LabirintDemoGame.Architecture;
using LabirintDemoGame.Controllers;

namespace LabirintDemoGame.Visualization
{
    public class LabyrinthWindow : Form
    {
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private readonly Game game;
        private const int SizeImage = 64;

        public LabyrinthWindow(Game game, DirectoryInfo imagesDirectory = null)
        {
            var simpleSound = new SoundPlayer(@"Sounds\Sound1.wav");
            simpleSound.PlayLooping();
            this.game = game;
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            if (imagesDirectory == null)
                imagesDirectory = new DirectoryInfo("Images");
            BackColor = Color.Black;
            foreach (var e in imagesDirectory.GetFiles("*.png"))
                bitmaps[e.Name.Substring(0, e.Name.Length - 4)] = (Bitmap) Image.FromFile(e.FullName);
            var timer = new Timer {Interval = 19};
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
                var c = GetWindowCoordinates(t);
                e.Graphics.DrawImage(bitmaps["Empty"], new Point(c.X * SizeImage, c.Y * SizeImage));
                if (t.Type != CellTypes.Player)
                    e.Graphics.DrawImage(bitmaps[t.Type.ToString()], new Point(c.X * SizeImage, c.Y * SizeImage));
                if (!t.IsExplored)
                    e.Graphics.FillRectangle(
                        Brushes.Black, c.X * SizeImage, c.Y * SizeImage, SizeImage, SizeImage);

            }
            var player = GetWindowCoordinates(game.PlayerPosition);
            e.Graphics.DrawImage(bitmaps["Player"], 
                new Point(player.X * SizeImage, player.Y * SizeImage));
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

        private Point GetWindowCoordinates(Cell cell)
        {
            var deltaX = Math.Max(0, game.PlayerPosition.X - ClientSize.Width/SizeImage + 2);
            var deltaY = Math.Max(0, game.PlayerPosition.Y - ClientSize.Height/SizeImage + 2);
            return new Point(cell.X - deltaX, cell.Y-deltaY);
        }
    }
}