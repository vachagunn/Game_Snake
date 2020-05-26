using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class SnakeGame : Form
    {
        SolidBrush black_brush;
        SolidBrush white_brush;
        SolidBrush green_brush;
        Point[] snake;
        Point apple;
        Random random;
        int len = 1;
        int width;
        int height;

        public SnakeGame()
        {
            InitializeComponent();

            snake = new Point[10000];
            random = new Random();
            
            picture_box.Image = new Bitmap(picture_box.Width, picture_box.Height);
            width = picture_box.Width / 10; //вся площадка по ширине
            height = picture_box.Height / 10; //вся площадка по высоте

            snake[0].X = width / 2;    //размещение змейки в центре карты 
            snake[0].Y = height / 2;

            black_brush = new SolidBrush(Color.Black); 
            white_brush = new SolidBrush(Color.White);
            green_brush = new SolidBrush(Color.Green);

            apple.X = random.Next(0, width - 1); // чтобы не вышло за пределы яблоко
            apple.Y = random.Next(0, height - 1);
        }
         
        string direction = "up"; //запоминаем направление змейки (клавиатура)

        private void timer1_Tick(object sender, EventArgs e) //метод отрисовки
        {
            Graphics graphics = Graphics.FromImage(picture_box.Image);

            graphics.FillRectangle(white_brush, 0, 0, picture_box.Width, picture_box.Height);   // очистка игрового поля при помощи белой кисти
            // задаём коордниты , верхний левый угол и нижный правый (диагональ)

            if (len > 4)
            {
                for (int i = 1; i < len; i++) 
                {
                    for (int j = i + 1; j < len; j++)
                    {
                        if (snake[i].X == snake[j].X && snake[i].Y == snake[j].Y) // если координаты одинаковы ,то змея ест себя 
                        {
                            len = 3; //обрезаем длину змейки
                        }
                    }
                }
            }

            for (int i = 0; i < len; i++) // рисуем змейку
            {
                if (snake[i].X < 0) snake[i].X += width;    // змейка может уходить за границы
                if (snake[i].X > width) snake[i].X -= width;
                if (snake[i].Y < 0) snake[i].Y += height;
                if (snake[i].Y > height) snake[i].Y -= height;

                graphics.FillEllipse(black_brush, snake[i].X*10, snake[i].Y*10, 10, 10);

                if (apple.X == snake[i].X && apple.Y == snake[i].Y) //проверка на столкновение змейки с яблоком
                {
                    apple.X = random.Next(0, width - 1); //появляется новое яблоко
                    apple.Y = random.Next(0, height - 1);
                    len++;                // увеличиваем длину змейки после поедания яблока
                }
            }

            graphics.FillEllipse(green_brush, apple.X * 10, apple.Y * 10, 10, 10); // рисуем яблоко

            //перемещение змейки по координатам
            if (direction == "up") snake[0].Y -= 1; 
            if (direction == "down") snake[0].Y += 1;
            if (direction == "left") snake[0].X -= 1;
            if (direction == "right") snake[0].X += 1;

            if (len > 10000 - 3)
            {
                len = 10000 - 3; //не выйдем за пределы массива (проверка на границы)
            }

            for (int i = len ; i >= 0; i--) //перемещение змейки целиком
            {
                snake[i + 1].X = snake[i].X; 
                snake[i + 1].Y = snake[i].Y;
            }

            if (len < 4) //увеличение длины 
            {
                len++;
            }

            picture_box.Invalidate(); // перересовка

            timer1.Enabled = true;
            timer2.Enabled = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                direction = "up";
            }

            if (e.KeyCode == Keys.Down)
            {
                direction = "down";
            }

            if (e.KeyCode == Keys.Left)
            {
                direction = "left";
            }

            if (e.KeyCode == Keys.Right)
            {
                direction = "right";
            }
        }

        private void timer2_Tick_1(object sender, EventArgs e) // ускорение змейки
        {
            if (timer1.Interval < 11)
            {
                timer1.Interval = 11;
            }

            timer1.Interval -= 10;
        }
    }
}
