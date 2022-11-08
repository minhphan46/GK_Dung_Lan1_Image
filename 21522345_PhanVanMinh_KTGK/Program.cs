using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;

namespace _21522345_PhanVanMinh_KTGK
{
    internal class Program
    {
        public static string path = "";

        public static Form form = new Form();
        public static PictureBox ptbSua = new PictureBox();
        public static PictureBox ptbGoc = new PictureBox();
        public static Label lb1 = new Label();
        public static Label lb2 = new Label();
        public static TrackBar tb1 = new TrackBar();
        public static TrackBar tb2 = new TrackBar();
        public static Button btLoad = new Button();
        public static Button btSave = new Button();
        public static Button btReset = new Button();
        public static Label lb_tb1 = new Label();
        public static Label lb_tb2 = new Label();

        public static Bitmap myImage;

        public static Thread t = new Thread((ThreadStart)(() => {
            OpenFileDialog saveFileDialog1 = new OpenFileDialog();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = saveFileDialog1.FileName;
                ptbGoc.Image = new Bitmap(@path);
                ptbSua.Image = new Bitmap(@path);
                myImage = (Bitmap)ptbGoc.Image;
            }
        }));
        public static Thread s = new Thread((ThreadStart)(() => {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = @"PNG|*.png" })
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ptbSua.Image.Save(saveFileDialog.FileName);
                }
            }
        }));

        static void Main(string[] args)
        {
            //form.Paint += new PaintEventHandler(Form1_Paint);
            form.Height = 500;
            form.Width = 500;

            ptbSua.Width = 200;
            ptbSua.Height = 200;
            ptbSua.BackColor = Color.Aqua;
            ptbSua.SizeMode = PictureBoxSizeMode.StretchImage;
            ptbSua.Location = new Point(0, 0);

            ptbGoc.Width = 200;
            ptbGoc.Height = 200;
            ptbGoc.BackColor = Color.Aqua;
            ptbGoc.SizeMode = PictureBoxSizeMode.StretchImage;
            ptbGoc.Location = new Point(220, 0);

            lb1.Text = "Brightness";
            lb1.Width = 70;
            lb1.Location = new Point(10, 240);

            tb1.Location = new Point(110, 240);
            tb1.SetRange(-125, 125);
            tb1.Width = 150;
            tb1.TickFrequency = 10;
            tb1.Scroll += new EventHandler(tb1_Scroll);

            lb_tb1.Location = new Point(90, 242);
            lb_tb2.Location = new Point(90, 352);

            lb2.Text = "Saturation";
            lb2.Width = 70;
            lb2.Location = new Point(10, 350);

            tb2.Location = new Point(110, 350);
            tb2.SetRange(-125, 125);
            tb2.Width = 150;
            tb2.TickFrequency = 10;
            tb2.Scroll += new EventHandler(tb2_Scroll);


            btLoad.Location = new Point(280, 240);
            btLoad.Text = "Load";

            btSave.Location = new Point(280, 280);
            btSave.Text = "Save";

            btReset.Location = new Point(370, 240);
            btReset.Text = "Reset";

            // event
            btLoad.Click += new EventHandler(btLoad_click);
            btSave.Click += new EventHandler(btSave_click);
            btReset.Click += new EventHandler(btReset_click);


            form.Controls.Add(ptbSua);
            form.Controls.Add(ptbGoc);
            form.Controls.Add(lb1);
            form.Controls.Add(lb2);
            form.Controls.Add(tb1);
            form.Controls.Add(tb2);
            form.Controls.Add(btLoad);
            form.Controls.Add(btSave);
            form.Controls.Add(btReset);
            form.Controls.Add(lb_tb1);
            form.Controls.Add(lb_tb2);
            Application.Run(form);
        }

        private static void tb2_Scroll(object sender, EventArgs e)
        {
            lb_tb2.Text = tb2.Value.ToString();
            ptbSua.Image = Saturation1(myImage, (float)tb2.Value/100);
        }
        public static Bitmap Saturation1(Bitmap image, float saturation)
        {
            Bitmap TempBitmap = image;

            Bitmap NewBitmap = new Bitmap(TempBitmap.Width, TempBitmap.Height);

            Graphics NewGraphics = Graphics.FromImage(NewBitmap);

            saturation = (float)saturation + 255 / 255.0f;
            float rWeight = 0.3086f;
            float gWeight = 0.6094f;
            float bWeight = 0.0820f;

            float a = (1.0f - saturation) * rWeight + saturation;
            float b = (1.0f - saturation) * rWeight;
            float c = (1.0f - saturation) * rWeight;
            float d = (1.0f - saturation) * gWeight;
            float e = (1.0f - saturation) * gWeight + saturation;
            float f = (1.0f - saturation) * gWeight;
            float g = (1.0f - saturation) * bWeight;
            float h = (1.0f - saturation) * bWeight;
            float i = (1.0f - saturation) * bWeight + saturation;

            float[][] FloatColorMatrix = {
                new float[] {a,  b,  c,  0, 0},
                new float[] {d,  e,  f,  0, 0},
                new float[] {g,  h,  i,  0, 0},
                new float[] {0,  0,  0,  1, 0},
                new float[] {0, 0, 0, 0, 1}
            };
            ColorMatrix NewColorMatrix = new ColorMatrix(FloatColorMatrix);

            ImageAttributes Attributes = new ImageAttributes();

            Attributes.SetColorMatrix(NewColorMatrix);

            NewGraphics.DrawImage(TempBitmap, new Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), 0, 0, TempBitmap.Width, TempBitmap.Height, GraphicsUnit.Pixel, Attributes);

            Attributes.Dispose();

            NewGraphics.Dispose();

            return NewBitmap;
        }
        private static void tb1_Scroll(object sender, EventArgs e)
        {
            lb_tb1.Text = tb1.Value.ToString();
            ptbSua.Image = AdjustBrightness1(myImage, (float)tb1.Value);

        }
        public static Bitmap AdjustBrightness1(Bitmap image, float brightness)
        {
            Bitmap TempBitmap = image;

            Bitmap NewBitmap = new Bitmap(TempBitmap.Width, TempBitmap.Height);

            Graphics NewGraphics = Graphics.FromImage(NewBitmap);

            float FinalValue = (float)brightness / 255.0f;

            float[][] FloatColorMatrix ={

                    new float[] {1, 0, 0, 0, 0},

                    new float[] {0, 1, 0, 0, 0},

                    new float[] {0, 0, 1, 0, 0},

                    new float[] {0, 0, 0, 1, 0},

                    new float[] {FinalValue, FinalValue, FinalValue, 1, 1}
                };

            ColorMatrix NewColorMatrix = new ColorMatrix(FloatColorMatrix);

            ImageAttributes Attributes = new ImageAttributes();

            Attributes.SetColorMatrix(NewColorMatrix);

            NewGraphics.DrawImage(TempBitmap, new Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), 0, 0, TempBitmap.Width, TempBitmap.Height, GraphicsUnit.Pixel, Attributes);

            Attributes.Dispose();

            NewGraphics.Dispose();

            return NewBitmap;
        }
        private static void btReset_click(object sender, EventArgs e)
        {
            try
            {
                ptbSua.Image = new Bitmap(@path);
                tb1.Value = 0;
                tb2.Value = 0;
            }
            catch { }
        }

        private static void btSave_click(object sender, EventArgs e)
        { 
            try
            {
                s.SetApartmentState(ApartmentState.STA);
                s.Start();
                s.Join();
            }
            catch
            {

            }
        }

        private static void btLoad_click(object sender, EventArgs e)
        {
            try
            {
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
                t.Join();
            }
            catch
            {

            }
        }
    }
}
