using System.Drawing;
using System.Windows.Forms;

namespace LabirintDemoGame
{
    public class LabyrinthWindow : Form
    {
        public LabyrinthWindow(MapController labyrinth)
        {
            ClientSize = new Size(32 * labyrinth.MazeWidth, 32 * labyrinth.MazeHeight);
            FormBorderStyle = FormBorderStyle.FixedDialog;
        }
    }
}