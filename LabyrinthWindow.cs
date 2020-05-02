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
        private const int size = 32;

        public LabyrinthWindow(Game game, DirectoryInfo imagesDirectory = null)
        {
            this.game = game;
            ClientSize = new Size(size * game.MazeWidth, size * game.MazeHeight);
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
            for (var i = 0; i < game.MazeHeight; i++)
            for (var j = 0; j < game.MazeWidth; j++)
                e.Graphics.DrawImage(bitmaps["Empty"], new Point(j * size, i * size));
                
            
            for (var i = 0; i < game.MazeHeight; i++)
            for (var j = 0; j < game.MazeWidth; j++)
                if (game.Map[i, j].Type.ToString() != "Player")
                    e.Graphics.DrawImage(bitmaps[game.Map[i, j].Type.ToString()], new Point(j * 32, i * 32));
            
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