using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using LabirintDemoGame.Architecture;
using LabirintDemoGame.Controllers;
using System.Threading;

namespace LabirintDemoGame.Visualization
{
    public class LabyrinthWindow : Form
    {
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private readonly Game game;
        private const int SizeImage = 64;

        public LabyrinthWindow(Game game)
        {
            // var simpleSound = new SoundPlayer(@"Sounds\Sound1.wav");
            // simpleSound.PlayLooping();
            this.game = game;
            ClientSize = new Size(1028, 640);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            BackColor = Color.Black;
            foreach (var e in new DirectoryInfo("Images").GetFiles("*.png"))
                bitmaps[e.Name.Substring(0, e.Name.Length - 4)] = (Bitmap) Image.FromFile(e.FullName);
            foreach (var e in new DirectoryInfo("Background").GetFiles("*.png"))
                bitmaps[e.Name.Substring(0, e.Name.Length - 4)] = (Bitmap) Image.FromFile(e.FullName);
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Text = "Tower of Durlag";
            DoubleBuffered = true;
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            if (game.StepType == Step.Plot)
            {
                Plot(e);
            }
            else
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
                e.Graphics.DrawString(game.Player.Gold.ToString(), new Font("Arial", 20), Brushes.Yellow, 0, 0);
            }
        }
        
        private void Plot(PaintEventArgs e)
        {
            var text = new Button();
            MyButton.CreateMyButton(text, this, game.Level.Plot.CurrentAct.Text, 
                new Point((ClientSize.Width - 600)/2, 400), 100, 600, null);
            var l = new Button();
            MyButton.CreateMyButton(l, this, game.Level.Plot.CurrentOptions[0].Name, 
                new Point(50 , ClientSize.Height - 80), 50, 250, null);
            var r = new Button();
            MyButton.CreateMyButton(l, this, game.Level.Plot.CurrentOptions[1].Name, 
                new Point(ClientSize.Width - 300 , ClientSize.Height - 80), 50, 250, null);
            e.Graphics.DrawImage(
                bitmaps[game.Level.Plot.CurrentAct.Image.Substring(
                    0, game.Level.Plot.CurrentAct.Image.Length - 4)], new Point(0,0));
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    game.MakePlayerMove(Directions.Up);
                    Invalidate();
                    break;
                case Keys.Down:
                    game.MakePlayerMove(Directions.Down);
                    Invalidate();
                    break;
                case Keys.Left:
                    game.MakePlayerMove(Directions.Left);
                    Invalidate();
                    break;
                case Keys.Right:
                    game.MakePlayerMove(Directions.Right);
                    Invalidate();
                    break;
            }
        }

        private Point GetWindowCoordinates(Cell cell)
        {
            var deltaX = Math.Max(0, game.PlayerPosition.X - ClientSize.Width/SizeImage + 3);
            var deltaY = Math.Max(0, game.PlayerPosition.Y - ClientSize.Height/SizeImage + 3);
            return new Point(cell.X - deltaX, cell.Y-deltaY);
        }
    }
}