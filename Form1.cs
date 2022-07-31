using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Paint
{
    public partial class Form1 : Form
    {
        Bitmap picture;
        int x_before = 0, y_before = 0;
        int x_click = 0, y_click = 0;        
        string figure = "line";
        Graphics graph;     
        
        public void Validate(Bitmap bm,Stack<Point>stack, int x, int y, Color old_color, Color new_color)
        {
            Color color = bm.GetPixel(x, y);
            if(color == old_color)
            {
                stack.Push(new Point(x, y));
                bm.SetPixel(x, y, new_color);
            }
        }
        public void Fill(Bitmap bm, int x, int y, Color new_color)
        {
            Color old_color = bm.GetPixel(x, y);
            Stack<Point> stack = new Stack<Point>();
            stack.Push(new Point(x, y));
            bm.SetPixel(x, y, new_color);
            if (old_color.ToArgb() == new_color.ToArgb()) return;

            while(stack.Count > 0)
            {
                Point point = (Point)stack.Pop();
                if(point.X > 0 && point.Y > 0 && point.X < bm.Width-1  && point.Y < bm.Height-1)
                {
                    Validate(bm, stack, point.X - 1, point.Y, old_color, new_color);
                    Validate(bm, stack, point.X, point.Y - 1, old_color, new_color);
                    Validate(bm, stack, point.X + 1, point.Y, old_color, new_color);
                    Validate(bm, stack, point.X, point.Y + 1, old_color, new_color);
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
            picture = new Bitmap(1920, 1080);
            graph = Graphics.FromImage(picture);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
             
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button1.BackColor = button.BackColor;            
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
            if(saveFileDialog1.FileName != "")
            {
                picture.Save(saveFileDialog1.FileName);
            }
        }
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if(openFileDialog1.FileName != "")
            {
                picture = (Bitmap)Image.FromFile(openFileDialog1.FileName);
                pictureBox1.Image = picture;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            figure = "line";
        }

        private void button15_Click(object sender, EventArgs e)
        {
            figure = "rectangle";
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Color color = button1.BackColor;
            Pen pen = new Pen(color, trackBar1.Value);
            if (figure == "static_line")
            {
                graph.DrawLine(pen, x_click, y_click, e.X, e.Y);

                textBox1.Text += "static_line\r\n";
                textBox1.Text += $"{x_click}\r\n";
                textBox1.Text += $"{y_click}\r\n";
                textBox1.Text += $"{e.X}\r\n";
                textBox1.Text += $"{e.Y}\r\n";                
            }
            if (figure == "rectangle")
            {
                graph.DrawRectangle(pen, x_click, y_click, e.X - x_click, e.Y - y_click);

                textBox1.Text += "rectangle\r\n";
                textBox1.Text += $"{x_click}\r\n";
                textBox1.Text += $"{y_click}\r\n";
                textBox1.Text += $"{e.X - x_click}\r\n";
                textBox1.Text += $"{e.Y - y_click}\r\n";
            }
            if (figure == "circle")
            {
                graph.DrawEllipse(pen, x_click, y_click, e.X - x_click, e.Y - y_click);
                textBox1.Text += "circle\r\n";
                textBox1.Text += $"{x_click}\r\n";
                textBox1.Text += $"{y_click}\r\n";
                textBox1.Text += $"{e.X - x_click}\r\n";
                textBox1.Text += $"{e.Y - y_click}\r\n";
            }
            pictureBox1.Image = picture;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            figure = "circle";
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
                     
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (button16.Focused)
            {
                Color color = button1.BackColor;                
                Fill(picture, e.X, e.Y, color);
            }
            if (button17.Focused)
            {
                Color selected_color = picture.GetPixel(e.X, e.Y);
                button1.BackColor = selected_color;
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {

        }

        private void button18_Click(object sender, EventArgs e)
        {
            figure = "static_line";
        }

        private void button22_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.ShowDialog();
            string name = save.FileName;
            File.WriteAllText(name, textBox1.Text);
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void button19_Click(object sender, EventArgs e)
        {
            graph.Clear(Color.White);
            openFileDialog1.ShowDialog();
            string name = openFileDialog1.FileName;

            string[] arr = File.ReadAllLines(name);
            Pen pen = new Pen(Color.Black);
            for (int i = 0; i < arr.Length; i += 5)
            {
                if (arr[i] == "static_line")
                {
                    graph.DrawLine(pen,Convert.ToInt32(arr[i + 1]), Convert.ToInt32(arr[i + 2]), Convert.ToInt32(arr[i + 3]), Convert.ToInt32(arr[i + 4]));
                }
                else if (arr[i] == "rectangle")
                {
                    graph.DrawRectangle(pen,Convert.ToInt32(arr[i + 1]), Convert.ToInt32(arr[i + 2]), Convert.ToInt32(arr[i + 3]), Convert.ToInt32(arr[i + 4]));
                }
                else if (arr[i] == "circle")
                {
                    graph.DrawEllipse(pen,Convert.ToInt32(arr[i + 1]), Convert.ToInt32(arr[i + 2]), Convert.ToInt32(arr[i + 3]), Convert.ToInt32(arr[i + 4]));
                }               
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            x_click = e.X;
            y_click = e.Y;             
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Color color = button1.BackColor;
            Pen pen = new Pen(color, trackBar1.Value);
            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                       
            if(e.Button == MouseButtons.Left)
            {
                if(figure == "line")
                {
                    graph.DrawLine(pen, x_before, y_before, e.X, e.Y);                      
                }              
                pictureBox1.Image = picture;
            }
            x_before = e.X;
            y_before = e.Y;      
        }
    }
}
