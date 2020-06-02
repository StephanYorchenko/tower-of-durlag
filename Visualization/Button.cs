using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace LabirintDemoGame.Visualization
{
    public class MyButton : Button
    {
        public static void CreateMyButton(Button btn, Form form, string text, Point loc, int h, 
        int w, EventHandler evh, bool enabled, Font font)
        {
            btn = new Button
            {
                Size = new Size(w, h),
                BackColor = Color.Black,
                Text = text,
                ForeColor = Color.Azure,
                Location = (loc),
                Enabled = enabled,
                Font = font
            };
            btn.Click += enabled ? evh : null;
            form.Controls.Add(btn);
        }
    }
}