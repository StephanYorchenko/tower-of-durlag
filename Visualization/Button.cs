using System;
using System.Drawing;
using System.Windows.Forms;
using LabirintDemoGame.Architecture;

namespace LabirintDemoGame.Visualization
{
    public class MyButton : Button
    {
        public static void CreateMyButton(Button btn, Form form, string text, Point loc, int h, int w, EventHandler evh)
        {
            btn = new Button();
            btn.Size = new Size(w, h);
            btn.BackColor = Color.Black;
            btn.Text = text;
            btn.ForeColor = Color.Azure;
            btn.Location = (loc);
            btn.Click += evh;
            
            form.Controls.Add(btn);
        }
    }
}