using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using LabirintDemoGame.Architecture;
using LabirintDemoGame.Controllers;

namespace LabirintDemoGame.Visualization
{
    public class LabyrinthWindow : Form
    {
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private readonly Game game;
        private const int SizeImage = 64;
        private const int StatBar = 64;
        private bool drawResult;
        private int index;

        public LabyrinthWindow(Game game)
        {
            var simpleSound = new SoundPlayer(@"Sounds/Sound1.wav");
            simpleSound.PlayLooping();
            this.game = game;
            ClientSize = new Size(1028, 640);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MinimumSize = new Size(1028, 640);
            MaximumSize = new Size(1028, 640);
            BackColor = Color.Black;
            foreach (var e in new DirectoryInfo("Images").GetFiles("*.png"))
                bitmaps[e.Name.Substring(0, e.Name.Length - 4)] = (Bitmap) Image.FromFile(e.FullName);
            foreach (var e in new DirectoryInfo("Background").GetFiles("*.png"))
                bitmaps[e.Name.Substring(0, e.Name.Length - 4)] = (Bitmap) Image.FromFile(e.FullName);
            foreach (var e in new DirectoryInfo("Stats").GetFiles("*.png"))
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
            e.Graphics.Clear(BackColor);
            if (drawResult)
            {
                e.Graphics.DrawImage( bitmaps[game.Level.Plot.CurrentAct.Image.Substring(0, game.Level.Plot.CurrentAct.Image.Length - 4)], new Point(0,0));
                var c = new Button();
                MyButton.CreateMyButton(c, this, game.Level.Plot.CurrentAct.GetOptions()[index].Result, 
                    new Point((ClientSize.Width - 600)/2, 400), 100, 600, Click, true);
            }
            else if (game.EndGame)
                Dead(e);
            else if (game.StepType == Step.Plot)
                Plot(e);
            else if (game.StepType == Step.Maze)
            {
                foreach (var t in game.Map)
                {
                    var c = GetWindowCoordinates(t);
                    e.Graphics.DrawImage(bitmaps["Empty"], new Point(c.X * SizeImage, StatBar + c.Y * SizeImage));
                    if (t.Type != CellTypes.Player)
                        e.Graphics.DrawImage(bitmaps[t.Type.ToString()], new Point(c.X * SizeImage, StatBar + c.Y * 
                        SizeImage));
                    if (!t.IsExplored)
                        e.Graphics.FillRectangle(
                            Brushes.Black, c.X * SizeImage, StatBar + c.Y * SizeImage, SizeImage, SizeImage);

                }
                var player = GetWindowCoordinates(game.PlayerPosition);
                e.Graphics.DrawImage(bitmaps["Player"], 
                    new Point(player.X * SizeImage, StatBar + player.Y * SizeImage));
                e.Graphics.ResetTransform();
            }
            PaintStatBar(e);
        }

        private void Dead(PaintEventArgs e)
        {
            var image = bitmaps["YouDead"];
            e.Graphics.DrawImage(image, new Point(0, 0));
        }


        private void Plot(PaintEventArgs e)
        {
            game.StartPlotAct();
            
            var text = new Button();
            var r = new Button();
            var l = new Button();
            var image = bitmaps[game.Level.Plot.CurrentAct.Image.Substring(0, game.Level.Plot.CurrentAct.Image.Length - 4)];
            e.Graphics.DrawImage( image, new Point(0,StatBar));
            MyButton.CreateMyButton(text, this, game.Level.Plot.CurrentAct.Text, 
                new Point(214, 380), 100, 600, null, false);
            MyButton.CreateMyButton(l, this, game.Level.Plot.CurrentOptions[0].Name, 
                new Point(50 , ClientSize.Height - 80), 50, 250, (sender, args) => ClickMyButton(0, new []{l,r,
                text}), IsButtonEnable(game.Plot.CurrentOptions[0]));
            MyButton.CreateMyButton(l, this, game.Level.Plot.CurrentOptions[1].Name, 
                new Point(ClientSize.Width - 300 , ClientSize.Height - 80), 50, 250, (sender, args) => 
                ClickMyButton(1, new []{l,r,text}), IsButtonEnable(game.Plot.CurrentOptions[1]));
        }
        
        private void ClickMyButton(int btnIndex, Button[] bts)
        {
            game.MakePlotAction(btnIndex);
            index = btnIndex;
            Controls.Clear();
            drawResult = true;
            Invalidate();
        }

        private bool IsButtonEnable(Option option)
        {
            return option.Requirements == null || option.IsValid(game.Player);
        }

        private new void Click(object sender, EventArgs e)
        {
            game.EndPlotAct(); 
            Controls.Clear();
            drawResult = false;
            Invalidate();
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
                case Keys.F:
                    if (game.EndGame) 
                        Close();
                    break;
            }
        }

        private Point GetWindowCoordinates(Cell cell)
        {
            var deltaX = Math.Max(0, game.PlayerPosition.X - ClientSize.Width/SizeImage + 3);
            var deltaY = Math.Max(0, game.PlayerPosition.Y - ClientSize.Height/SizeImage + 3);
            return new Point(cell.X - deltaX, cell.Y-deltaY);
        }

        private void PaintStatBar(PaintEventArgs e)
        {
            e.Graphics.DrawImage(bitmaps[GetHpImageName()], 0, 0);
            e.Graphics.DrawString(game.Player.Hp.ToString(), new Font("Arial", 14), Brushes.Yellow, 64, 40);
            e.Graphics.DrawImage(bitmaps["Bandage"], 96, 0);
            e.Graphics.DrawString(game.Player.Bandage.ToString(), new Font("Arial", 14), Brushes.Yellow, 160, 40);
            e.Graphics.DrawImage(bitmaps["Herb"], 192, 0);
            e.Graphics.DrawString(game.Player.Herb.ToString(), new Font("Arial", 14), Brushes.Yellow, 256, 40);
            e.Graphics.DrawImage(bitmaps["Supplies"], 288, 0);
            e.Graphics.DrawString(game.Player.Supplies.ToString(), new Font("Arial", 14), Brushes.Yellow, 352, 40);
            e.Graphics.DrawImage(bitmaps["Torch"], 384, 0);
            e.Graphics.DrawString(game.Player.Torch.ToString(), new Font("Arial", 14), Brushes.Yellow, 448, 40);
            e.Graphics.DrawImage(bitmaps["Gold"], 480, 0);
            e.Graphics.DrawString(game.Player.Gold.ToString(), new Font("Arial", 14), Brushes.Yellow, 544, 40);
            if (game.Player.Sword != 2)
                e.Graphics.DrawImage(bitmaps["Sword"], 600, 0);
        }

        private string GetHpImageName()
        {
            var hp = (int) (22 - game.Player.Hp / 100d * 21);
            hp = Math.Max(1, hp);
            hp = Math.Min(hp, 21);
            return $"{hp}";
        }
    }
}