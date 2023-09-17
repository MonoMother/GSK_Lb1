using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1
{
    public partial class Form1 : Form
    {
        Graphics g;
        Pen DrawPen = new Pen(Color.Black, 1);
        int SplineType = 0; // Код типа сплайна
        const int np = 20;
        Point[] ArPoints = new Point[np]; // Массив точек
        int CountPoints = 0; // Счетчик точек
        public Form1()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics(); //инициализация графики
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }
        // факториал
        static double Factorial(int n)
        {
            double x = 1;
            for (int i = 1; i <= n; i++)
                x *= i;
            return x;
        }
        // Кубический сплайн
        public void DrawCubeSpline(Pen DrPen, Point[] P)
        {
            PointF[] L = new PointF[4]; // Матрица вещественных коэффициентов
            Point Pv1 = P[0];
            Point Pv2 = P[0];
            const double dt = 0.04;
            double t = 0;
            double xt, yt;
            Point Ppred = P[0], Pt = P[0];
            // Касательные векторы
            Pv1.X = 4 * (P[1].X - P[0].X);
            Pv1.Y = 4 * (P[1].Y - P[0].Y);
            Pv2.X = 4 * (P[3].X - P[2].X);
            Pv2.Y = 4 * (P[3].Y - P[2].Y);
            // Коэффициенты полинома
            L[0].X = 2 * P[0].X - 2 * P[2].X + Pv1.X + Pv2.X; // Ax
            L[0].Y = 2 * P[0].Y - 2 * P[2].Y + Pv1.Y + Pv2.Y; // Ay
            L[1].X = -3 * P[0].X + 3 * P[2].X - 2 * Pv1.X - Pv2.X; // Bx
            L[1].Y = -3 * P[0].Y + 3 * P[2].Y - 2 * Pv1.Y - Pv2.Y; // By
            L[2].X = Pv1.X; // Cx
            L[2].Y = Pv1.Y; // Cy
            L[3].X = P[0].X; // Dx
            L[3].Y = P[0].Y; // Dy
            while (t < 1 + dt / 2)
            {
                xt = ((L[0].X * t + L[1].X) * t + L[2].X) * t + L[3].X;
                yt = ((L[0].Y * t + L[1].Y) * t + L[2].Y) * t + L[3].Y;
                Pt.X = (int)Math.Round(xt);
                Pt.Y = (int)Math.Round(yt);
            
            g.DrawLine(DrPen, Ppred, Pt);
                Ppred = Pt;
                t = t + dt;
            }
        }
        // Кривая Безье
        public void DrawBezie(Pen DrPen, Point[] P, int n)
        {
            // по алгоритму из раздела 1.1 методических указаний
        }
        // Обработчик события
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (CountPoints >= np) return;
            ArPoints[CountPoints].X = e.X; ArPoints[CountPoints].Y = e.Y;
            g.DrawEllipse(DrawPen, e.X - 2, e.Y - 2, 5, 5);
            if (SplineType == 0) // Кубический сплайн
            {
                switch (CountPoints)
                {
                    case 1: // первый вектор
                        {
                            g.DrawLine(new Pen(Color.Magenta, 1), ArPoints[0], ArPoints[1]);
                            CountPoints++;
                        }
                        break;
                    case 3: // второй вектор
                        {
                            g.DrawLine(new Pen(Color.Magenta, 1), ArPoints[2], ArPoints[3]);
                            DrawCubeSpline(new Pen(DrawPen.Color, 3), ArPoints);
                            CountPoints = 0;
                        }
                        break;
                    default:
                        CountPoints++; // иначе
                        break;
                }
            }
            else // Безье
            {
                if (e.Button == MouseButtons.Right) // Конец ввода
                {
                    g.DrawLine(new Pen(Color.Magenta, 1), ArPoints[CountPoints - 1],
                   ArPoints[CountPoints]);
                    DrawBezie(new Pen(DrawPen.Color, 1), ArPoints, CountPoints);
                    CountPoints = 0;
                }
                else
                {
                    if (CountPoints > 0)
                        g.DrawLine(new Pen(Color.Magenta, 1),
                       ArPoints[CountPoints - 1], ArPoints[CountPoints]);
                    CountPoints++;
                }
            }
        }
        // Обработчик события выбора типа сплайна
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SplineType = comboBox1.SelectedIndex;
        }
    // Обработчик события выбора цвета
    private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (comboBox2.SelectedIndex) // выбор цвета
        {
            case 0:
                DrawPen.Color = Color.Black;
                break;
            case 1:
                DrawPen.Color = Color.Red;
                break;
            case 2:
                DrawPen.Color = Color.Green;
                break;
            case 3:
                DrawPen.Color = Color.Blue;
                break;
        }
    }
    // Очистка окна

        private void button1_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            CountPoints = 0;
        }
    }
}

