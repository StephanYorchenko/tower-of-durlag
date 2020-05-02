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
        private readonly MapController map;
        
        public LabyrinthWindow(MapController labyrinth, DirectoryInfo imagesDirectory = null)
        {
            map = labyrinth;
            ClientSize = new Size(32 * labyrinth.MazeHeight, 32 * labyrinth.MazeWidth);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            if (imagesDirectory == null)
                imagesDirectory = new DirectoryInfo("Images");
            foreach (var e in imagesDirectory.GetFiles("*.png"))
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
            e.Graphics.FillRectangle(Brushes.Black, 0, 0, 32 * map.MazeHeight, 32 * map.MazeWidth);
            for (int i = 0; i < map.MazeHeight; i++)
            {
                for (int j = 0; j < map.MazeWidth; j++)
                {
                    if (map.Maze[i, j].Type.ToString() != "Empty")
                    {
                        e.Graphics.DrawImage(bitmaps[map.Maze[i, j].Type.ToString()], new Point(i * 32, j * 32));
                    }
                }
            }
            e.Graphics.ResetTransform();
        }
    }
}