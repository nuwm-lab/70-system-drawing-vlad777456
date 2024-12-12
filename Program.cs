using System;
using System.Drawing;
using System.Windows.Forms;

namespace Graph
{
    public class GraphForm : Form
    {
        // Константи для налаштувань графіка
        private const double XStart = -10; // Початкове значення X
        private const double XEnd = 10;  // Кінцеве значення X
        private const double DeltaX = 0.6; // Крок для X

        public GraphForm()
        {
            // Налаштування форми
            this.Text = "Графік функції";
            this.Size = new Size(800, 800); // Розмір вікна
            this.BackColor = Color.White;

            // Перерисовка графіка при зміні розміру
            this.Resize += (s, e) => this.Invalidate();
            // Перерисовка графіка при малюванні
            this.Paint += (s, e) => DrawGraph(e.Graphics);
        }

        private void DrawGraph(Graphics graph)
        {
            // Очищаємо форму перед малюванням
            graph.Clear(Color.White);

            using (Pen axisPen = new Pen(Color.Black, 1)) // Перо для осей
            using (Pen pen = new Pen(Color.SlateBlue, 2)) // Перо для графіка
            {
                float widthForm = this.ClientSize.Width;
                float heightForm = this.ClientSize.Height;

                float offsetX = widthForm / 2; // Центрування графіка по осі X
                float offsetY = heightForm / 2; // Центрування графіка по осі Y

                // Визначаємо масштабування
                double scaleX = widthForm / 20.0; // Масштаб для X (діапазон -10 до 10)
                double scaleY = heightForm / 20.0; // Масштаб для Y (діапазон -10 до 10)

                // Малюємо осі X і Y
                graph.DrawLine(axisPen, 0, offsetY, widthForm, offsetY); // Вісь X
                graph.DrawLine(axisPen, offsetX, 0, offsetX, heightForm); // Вісь Y

                // Шрифт для підписів
                Font font = new Font("Arial", 10);
                Brush brush = Brushes.Black;

                // Підписи для осі X
                for (int t = -10; t <= 10; t++)
                {
                    int screenX = (int)(t * scaleX + offsetX);
                    graph.DrawString(t.ToString(), font, brush, screenX - 10, offsetY + 5);
                }

                // Підписи для осі Y
                for (int y = -10; y <= 10; y++)
                {
                    int screenY = (int)(offsetY - y * scaleY);
                    graph.DrawString(y.ToString(), font, brush, offsetX + 5, screenY - 10);
                }

                // Малювання графіка функції y = ((x + 2)^2) / sqrt(x^2 + 1)
                int screenX1 = 0, screenY1 = 0;
                bool firstPoint = true;

                for (double x = XStart; x <= XEnd; x += DeltaX)
                {
                    // Обчислюємо значення функції
                    double y = ((x + 2) * (x + 2)) / Math.Sqrt(x * x + 1);

                    // Перетворюємо координати функції на координати екрану
                    int screenX2 = (int)(x * scaleX + offsetX);
                    int screenY2 = (int)(offsetY - y * scaleY);

                    if (!firstPoint)
                    {
                        graph.DrawLine(pen, screenX1, screenY1, screenX2, screenY2); // Малюємо лінію
                    }
                    else
                    {
                        firstPoint = false;
                    }

                    // Оновлюємо початкові координати для наступної точки
                    screenX1 = screenX2;
                    screenY1 = screenY2;
                }
            }
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new GraphForm());
        }
    }
}
