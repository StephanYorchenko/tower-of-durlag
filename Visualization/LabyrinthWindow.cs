﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using LabirintDemoGame.Architecture;
using LabirintDemoGame.Controllers;

namespace LabirintDemoGame.Visualization
{
    public class LabyrinthWindow : Form
    {
        private readonly Dictionary<string, Bitmap> bitmaps = new Dictionary<string, Bitmap>();
        private Game game;
        private const int SizeImage = 64;
        private const int StatBar = 64;
        private bool drawResult;
        private int index;
        private bool start;
        private bool ng;
        private bool contin;
        private bool quit;
        private bool leader;
        private Leaderboard leaders;

        public LabyrinthWindow()
        {
            var simpleSound = new SoundPlayer(@"Sounds/Sound1.wav");
            simpleSound.PlayLooping();
            game = new Game(5, 7);
            start = true;
            ng = false;
            contin = false;
            quit = false;
            leader = false;
            leaders = new Leaderboard();
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
            if (start)
                MainMenu(e);
            else if (leader)
                DrawLeaderboard(e);
            else
            {
                if (drawResult)
                {
                    e.Graphics.DrawImage(
                        bitmaps[
                            game.Level.Plot.CurrentAct.Image.Substring(0,
                                game.Level.Plot.CurrentAct.Image.Length - 4)], new Point(0, 0));
                    e.Graphics.FillRectangle(Brushes.Black, 0, 0, ClientSize.Width, StatBar);
                    var c = new Button();
                    MyButton.CreateMyButton(c, this,
                        game.Level.Plot.CurrentAct.GetOptions()[index].Result,
                        new Point((ClientSize.Width - 600) / 2, 400), 100, 600, Click, true);
                }
                else if (game.StepType == Step.Plot)
                    Plot(e);
                else if (game.EndGame)
                    Dead(e);
                else if (game.StepType == Step.Maze)
                {
                    foreach (var t in game.Map)
                    {
                        var c = GetWindowCoordinates(t);
                        e.Graphics.DrawImage(bitmaps["Empty"],
                            new Point(c.X * (SizeImage - 1), StatBar + c.Y * (SizeImage - 1)));
                        if (t.Type != CellTypes.Player)
                            e.Graphics.DrawImage(bitmaps[t.Type.ToString()], new Point(
                                c.X * (SizeImage - 1), StatBar + c.Y *
                                (SizeImage - 1)));
                        if (!t.IsExplored)
                            e.Graphics.FillRectangle(
                                Brushes.Black, c.X * (SizeImage - 1),
                                StatBar + c.Y * (SizeImage - 1), SizeImage, SizeImage);
                    }
                    var player = GetWindowCoordinates(game.PlayerPosition);
                    e.Graphics.DrawImage(bitmaps["Player"],
                        new Point(player.X * (SizeImage - 1),
                            StatBar + player.Y * (SizeImage - 1)));
                    e.Graphics.ResetTransform();
                }
                PaintStatBar(e);
            }
        }

        private void Dead(PaintEventArgs e)
        {
            Controls.Clear();
            Invalidate();
            var image = bitmaps["YouDead"];
            e.Graphics.DrawImage(image, new Point(0, 0));
            leaders.Update("Stephan", game.Player.Gold);
        }


        private void Plot(PaintEventArgs e)
        {
            game.StartPlotAct();
            
            var text = new Button();
            var r = new Button();
            var l = new Button();
            var image = bitmaps[game.Level.Plot.CurrentAct.Image.Substring(0, game.Level.Plot.CurrentAct.Image.Length - 4)];
            e.Graphics.DrawImage( image, new Point(0,0));
            e.Graphics.FillRectangle(Brushes.Black, 0, 0, ClientSize.Width, StatBar);
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
            if (game.EndGame || leader)
            {
                start = true;
                leader = false;
                NewGame();
                Invalidate();
            }
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
                case Keys.Escape:
                    Pause();
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

        private void PaintStatBar(PaintEventArgs e)
        {
            e.Graphics.DrawImage(bitmaps[GetHpImageName()], 0, 0);
            e.Graphics.DrawString(game.Player.Hp.ToString(), new Font("Arial", 14), Brushes.Yellow, 64, 40);
            e.Graphics.DrawImage(bitmaps["Bandage"], 100, 0);
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

        private void MainMenu(PaintEventArgs e)
        {
            var image = bitmaps["Menu"];
            e.Graphics.DrawImage(image, new Point(0, 0));
            if (ng)
                e.Graphics.FillEllipse(Brushes.Silver, 655, 45, 10, 10);
            else if (contin)
                e.Graphics.FillEllipse(Brushes.Silver, 705, 120, 10, 10);
            else if (leader)
                e.Graphics.FillEllipse(Brushes.Silver, 600, 205, 10, 10);
            else if (quit)
                e.Graphics.FillEllipse(Brushes.Silver, 845, 275, 10, 10);
        }

        private void DrawLeaderboard(PaintEventArgs e)
        {
            var image = bitmaps["Leaderboard"];
            var font = new Font("Arial", 36);
            e.Graphics.DrawImage(image, new Point(0, 0));
            var y = 200;
            foreach (var record in leaders.Show())
            {
                e.Graphics.DrawString(record.Item1, font, Brushes.Silver, 240, y);
                e.Graphics.DrawString(record.Item2, font, Brushes.Silver, 600, y);
                y += 60;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!start) return;
            ng = e.Y < 80 && e.Y > 10 && e.X > 655;
            contin = e.Y < 155 && e.Y > 80 && e.X > 750;
            leader = e.Y < 230 && e.Y > 170 && e.X > 600;
            quit = e.Y < 320 && e.Y > 245 && e.X > 845;
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (!start) return;
            if (ng || leader)
                start = false;
            else if (contin)
            {
                Continue();
                start = false;
            }
            else if (quit)
                Close();
            Invalidate();
        }

        private void NewGame()
        {
            game = new Game(5, 7);
            drawResult = false;
        }

        private void Pause()
        {
            start = true;
            var p = string.Join(",", game.Player.Check());
            var log = $"{game.MazeWidth},{game.MazeHeight},{game.Player.Hp}," + p;
            using (var w = new StreamWriter("last.txt"))
                w.Write(log);
        }

        private void Continue()
        {
            string log;
            using (var r = new StreamReader("last.txt"))
                log = r.ReadLine();
            Console.WriteLine(log);
            var logList = log.Split(',')
                .Select(int.Parse)
                .ToList();
            game = new Game(logList[0], logList[1]);
            game.Player = new Player(logList[2], logList[3], logList[4], logList[5], logList[6],
                logList[7], logList[8]);
            Invalidate();
        }
    }
}