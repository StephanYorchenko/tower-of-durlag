using System.Drawing;
using System.Windows.Forms;

namespace LabirintDemoGame
{
    public class LabyrinthWindow : Form
    {
        public LabyrinthWindow(Game labyrinth)
        {
            ClientSize = new Size(32 * labyrinth.MazeWidth, 32 * labyrinth.MazeHeight);
            FormBorderStyle = FormBorderStyle.FixedDialog; // потом можно будет поставить None и сделать свой креатик
        }
    }
}