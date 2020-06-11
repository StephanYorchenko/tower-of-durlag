using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using LabirintDemoGame.Architecture;
using LabirintDemoGame.Controllers;
using NUnit.Framework.Internal;
using Timer = System.Windows.Forms.Timer;

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
        private bool signs;
        private bool larswoodSign;
        private bool underdarkSign;
        private bool towerSign;
        private bool preSign;
        private readonly Leaderboard leaders;
        private readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();
        private readonly Font lazursky;
        private readonly PrivateFontCollection fontCollection;
        private readonly Timer timer;
        private bool flag;
        private bool soundOn = true;
        private SoundPlayer simpleSound = new SoundPlayer(@"Sounds/Sound1.wav");

        public LabyrinthWindow()
        {
            simpleSound.PlayLooping();
            game = new Game(5, 7);
            start = true;
            ng = false;
            contin = false;
            quit = false;
            leader = false;
            preSign = false;
            leaders = new Leaderboard();
            
            fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile("lazursky.ttf");
            var family = fontCollection.Families[0];
            lazursky = new Font(family, 13);

            FormBorderStyle = FormBorderStyle.FixedDialog;
            MinimumSize = new Size(1028, 640);
            MaximumSize = new Size(1028, 640);
            BackColor = Color.Black;

            flag = false;
            
            foreach (var e in new DirectoryInfo("Images").GetFiles("*.png"))
                bitmaps[e.Name.Substring(0, e.Name.Length - 4)] = (Bitmap) Image.FromFile(e.FullName);
            foreach (var e in new DirectoryInfo("Background").GetFiles("*.png"))
                bitmaps[e.Name.Substring(0, e.Name.Length - 4)] = (Bitmap) Image.FromFile(e.FullName);
            foreach (var e in new DirectoryInfo("Stats").GetFiles("*.png"))
                bitmaps[e.Name.Substring(0, e.Name.Length - 4)] = (Bitmap) Image.FromFile(e.FullName);
            
            timer = new Timer {Interval = 100};
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs args)
        {
            if (game.StepType != Step.Maze || pressedKeys.Count <= 0) return;
            switch (pressedKeys.Min())
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
                case Keys.W:
                    game.MakePlayerMove(Directions.Up);
                    break;
                case Keys.S:
                    game.MakePlayerMove(Directions.Down);
                    break;
                case Keys.A:
                    game.MakePlayerMove(Directions.Left);
                    break;
                case Keys.D:
                    game.MakePlayerMove(Directions.Right);
                    break;
            }

            preSign = game.StepType == Step.Tavern;
            Invalidate();
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
            else if (preSign)
                EndAdventure(e);
            else
            {
                if (game.EndGame)
                    Dead(e);
                else if (drawResult)
                    Result(e);
                else if (game.StepType == Step.Plot)
                    Plot(e);
                else if (signs)
                    Signs(e);
                else if (game.StepType == Step.Maze)
                {
                    foreach (var t in game.Map)
                    {
                        var c = GetWindowCoordinates(t);
                        e.Graphics.DrawImage(bitmaps[GetEmptyImageName()],
                            new Point(c.X * (SizeImage - 1), StatBar + c.Y * (SizeImage - 1)));
                        if (t.Type == CellTypes.Wall)
                            e.Graphics.DrawImage(bitmaps[GetWallImageName()], new Point(
                                c.X * (SizeImage - 1), StatBar + c.Y *
                                (SizeImage - 1)));
                        else if (t.Type == CellTypes.Gold)
                            e.Graphics.DrawImage(bitmaps["Gold"], new Point(
                                c.X * (SizeImage - 1), StatBar + c.Y *
                                (SizeImage - 1)));
                        else if (t.Type == CellTypes.End)
                            e.Graphics.DrawImage(bitmaps[GetExitImageName()], new Point(
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
            var name = "Stephan";
            if (InputBox("Игра окончена", "Введите Ваше имя:", ref name) == DialogResult.OK)
            {
                leaders.Update(name, game.Player.Gold);
            }
        }

        private void Result(PaintEventArgs e)
        {
            e.Graphics.DrawImage(
                bitmaps[
                    game.Level.Plot.CurrentAct.Image.Substring(0,
                        game.Level.Plot.CurrentAct.Image.Length - 4)], 0, 0, 1024, 640);
            e.Graphics.FillRectangle(Brushes.Black, 40, 400, 944, 120);
            var s = new StringFormat {Alignment = StringAlignment.Center};
            e.Graphics.DrawString(game.Level.Plot.CurrentAct.GetOptions()[index].Result,
                lazursky,
                Brushes.Silver, 
                new RectangleF(50, 410, 924, 100),
                s);
            e.Graphics.FillRectangle(Brushes.Black, 360, 550, 290, 40);
            e.Graphics.DrawString("[ press space to continue ]",
                lazursky,
                Brushes.Silver, new RectangleF(380, 560, 270, 90));
        }

        private void Plot(PaintEventArgs e)
        {
            pressedKeys.Clear();
            game.StartPlotAct();
            var r = new Button();
            var l = new Button();
            var s = new StringFormat {Alignment = StringAlignment.Center};
            var image = bitmaps[game.Level.Plot.CurrentAct.Image.Substring(0, game.Level.Plot.CurrentAct.Image.Length - 4)];
            e.Graphics.DrawImage( image, 0, 0 , 1024, 640);
            e.Graphics.FillRectangle(Brushes.Black, 40, 400, 944, 120);
            e.Graphics.DrawString(game.Level.Plot.CurrentAct.Text, 
                lazursky, 
                Brushes.Silver,
                new RectangleF(50, 410, 924, 100),
                s);
            MyButton.CreateMyButton(l, 
                this, game.Level.Plot.CurrentOptions[0].Name, 
                new Point(50 , 540), 
                50, 400, 
                (sender, args) => ClickMyButton
                (0),
                IsButtonEnable(game.Plot.CurrentOptions[0]),
                lazursky);
            MyButton.CreateMyButton(r, 
                this, game.Level.Plot.CurrentOptions[1].Name, 
                new Point(574 , 540), 
                50, 400,
                (sender, args) => ClickMyButton(1),
                IsButtonEnable(game.Plot.CurrentOptions[1]), 
                lazursky);
        }
        
        private void ClickMyButton(int btnIndex)
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

        private new void Click()
        {
            game.EndPlotAct(); 
            Controls.Clear();
            drawResult = false;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            pressedKeys.Add(e.KeyCode);
            if (e.KeyCode == Keys.Escape)
                Pause();
            if (drawResult && e.KeyCode == Keys.Space)
                Click();
            if (e.KeyCode == Keys.M)
            {
                if (soundOn)
                {
                    simpleSound.Stop();
                    soundOn = false;
                }
                else
                {
                    simpleSound.PlayLooping();
                    soundOn = true;
                }
            }

            if (preSign)
            {
                preSign = false;
                signs = true;
            }
            if (game.EndGame || leader)
            {
                start = true;
                leader = false;
                NewGame();
            }
            Invalidate();
        }
        
        protected override void OnKeyUp(KeyEventArgs e)
        {
            pressedKeys.Remove(e.KeyCode);
            Invalidate();
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
            e.Graphics.DrawImage(bitmaps["Herb"], 110, 0);
            e.Graphics.DrawString(game.Player.Herb.ToString(), new Font("Arial", 14), Brushes.Yellow, 164, 40);
            e.Graphics.DrawImage(bitmaps["Supplies"], 196, 0);
            e.Graphics.DrawString(game.Player.Supplies.ToString(), new Font("Arial", 14), Brushes.Yellow, 260, 40);
            e.Graphics.DrawImage(bitmaps["Torch"], 292, 0);
            e.Graphics.DrawString(game.Player.Torch.ToString(), new Font("Arial", 14), Brushes.Yellow, 356, 40);
            e.Graphics.DrawImage(bitmaps["Gold"], 388, 0);
            e.Graphics.DrawString(game.Player.Gold.ToString(), new Font("Arial", 14), Brushes.Yellow, 440, 40);
            if (game.Player.Sword != 0)
                e.Graphics.DrawImage(bitmaps["Sword"], 480, 0);
        }

        private string GetHpImageName()
        {
            var hp = (int) (22 - game.Player.Hp / 100d * 21);
            hp = Math.Max(1, hp);
            hp = Math.Min(hp, 21);
            return $"{hp}";
        }

        private string GetWallImageName()
        {
            switch (game.Plot.Location)
            {
                case "TowerOfDurlag":
                    return "Wall0";
                case "Underdark":
                    return "Wall1";
                case "Larswood":
                    return "Wall2";
                default:
                    return "Wall0";
            }
        }

        private string GetExitImageName()
        {
            switch (game.Plot.Location)
            {
                case "Underdark":
                    return "End1";
                case "Larswood":
                    return "End2";
                default:
                    return "End0";
            }
        }

        private string GetEmptyImageName()
        {
            switch (game.Plot.Location)
            {
                case "Underdark":
                    return "Empty1";
                case "Larswood":
                    return "Empty2";
                default:
                    return "Empty0";
            }
        }

        private void MainMenu(PaintEventArgs e)
        {
            var image = bitmaps["Menu"];
            flag = false;
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
            e.Graphics.DrawImage(image, new Point(0, 0));
            var y = 200;
            foreach (var record in leaders.Show())
            {
                e.Graphics.DrawString(record.Item1, lazursky, Brushes.Silver, 240, y);
                e.Graphics.DrawString(record.Item2, lazursky, Brushes.Silver, 600, y);
                y += 60;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (start)
            {
                ng = e.Y < 80 && e.Y > 10 && e.X > 655;
                contin = e.Y < 155 && e.Y > 80 && e.X > 750;
                leader = e.Y < 230 && e.Y > 170 && e.X > 600;
                quit = e.Y < 320 && e.Y > 245 && e.X > 845;
                Invalidate();
            }
            else if (signs)
            {
                towerSign = e.Y > 318 && e.Y < 401 && 232 < e.X && e.X < 419;
                underdarkSign = 211 < e.Y && e.Y < 294 && 472 < e.X && e.X < 621;
                larswoodSign = 173 < e.Y && e.Y < 234 && 772 < e.X && e.X < 994;
                Invalidate();
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (!(start || signs)) return;
            if (ng && start)
            {
                start = false;
                NewGame();
            }
            else if (leader && start)
            {
                start = false;
            }
            else if (contin && start)
            {
                Continue();
                start = false;
            }
            else if (quit && start)
                Close();
            else if (signs)
            {
                if (towerSign)
                    game.Plot.Location = "TowerOfDurlag";
                else if (underdarkSign)
                    game.Plot.Location = "Underdark";
                else if (larswoodSign)
                    game.Plot.Location = "Larswood";
                game.Plot.CreateAdventure();
                game.GetNextLevel();
                game.StepType = Step.Maze;
                signs = false;
            }
            Invalidate();
        }

        private void NewGame()
        {
            game = new Game(5, 7);
            signs = true;
            drawResult = false;
        }

        private void Pause()
        {
            start = true;
        }

        private void Continue()
        {
            string log;
            using (var r = new StreamReader("last.txt"))
                log = r.ReadLine();
            Console.WriteLine(log);
            if (log != null)
            {
                var logList = log.Split(',')
                    .Select(int.Parse)
                    .ToList();
                game = new Game(logList[0], logList[1])
                {
                    Player = new Player(logList[2], logList[3], logList[4], logList[5],
                        logList[6], logList[7])
                };
            }

            game.StepType = Step.Tavern;
            signs = true;
            Invalidate();
        }

        private DialogResult? InputBox(string title, string promptText, ref string value)
        {
            var form = new Form();
            var label = new Label();
            var textBox = new TextBox();
            var buttonOk = new Button();
            var buttonCancel = new Button();
 
            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;
 
            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;
 
            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);
 
            label.AutoSize = true;
            textBox.Anchor |= AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
 
            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            if (flag) return null;
            flag = true;
            var dialogResult = form.ShowDialog();
            value = string.Join("",textBox.Text.Split(' '));
            return dialogResult;

        }

        private void Signs(PaintEventArgs e)
        {
            var p = string.Join(",", game.Player.Check());
            var log = $"{game.MazeWidth},{game.MazeHeight},{game.Player.Hp}," + p;
            using (var w = new StreamWriter("last.txt"))
                w.Write(log);
            e.Graphics.DrawImage(
                bitmaps["Ways"], 0, 0, 1024, 640);
            var pen = new Pen(Brushes.Silver, 3);
            if (towerSign)
            {
                e.Graphics.DrawLine(pen, 232, 354, 287, 324);
                e.Graphics.DrawLine(pen, 287, 324, 419, 318);
                e.Graphics.DrawLine(pen, 232, 354, 296, 387);
                e.Graphics.DrawLine(pen, 415, 401, 296, 387);
            }
            
            if (underdarkSign)
            {
                e.Graphics.DrawLine(pen, 472, 211, 584, 233);
                e.Graphics.DrawLine(pen, 621, 264, 584, 233);
                e.Graphics.DrawLine(pen, 621, 264, 583, 292);
                e.Graphics.DrawLine(pen, 461, 294, 583, 292);
            }
            
            if (larswoodSign)
            {
                e.Graphics.DrawLine(pen, 772, 174, 921, 173);
                e.Graphics.DrawLine(pen, 921, 173, 994, 195);
                e.Graphics.DrawLine(pen, 994, 195, 947, 227);
                e.Graphics.DrawLine(pen, 947, 227, 793, 234);
            }
        }

        private void EndAdventure(PaintEventArgs e)
        {
            string name = "EndAdventure0";
            switch (game.Plot.Location)
            {
                case "TowerOfDurlag":
                    name = "EndAdventure0";
                    break;
                case "Larswood":
                    name = "EndAdventure2";
                    break;
                case "Underdark":
                    name = "EndAdventure1";
                    break;
            }
            e.Graphics.DrawImage(
                bitmaps[name], 0, 0, 1024, 640);
        }
    }
}