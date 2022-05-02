using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab6
{
    public partial class Form1 : Form
    {
        Image<Bgr, byte> My_Image = null;
        Image<Gray, byte> output = null;

        int TotalFrame, FrameNo;
        double Fps;
        bool IsReadingFrame;
        VideoCapture capture;

        private static VideoCapture cameraCapture;
        private Image<Bgr, Byte> newBackgroundImage;
        private static IBackgroundSubtractor fgDetector;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if(openFile.ShowDialog()== DialogResult.OK)
            {
                 My_Image = new Image<Bgr, byte>(openFile.FileName);
                pictureBox1.Image = My_Image.ToBitmap();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                My_Image = new Image<Bgr, byte>(openFile.FileName);
                Image<Gray, byte> gray_image = My_Image.Convert<Gray, byte>();
                pictureBox2.Image = gray_image.AsBitmap();
                gray_image[0, 0]= new Gray(200);

            }
          

        }

        private void button3_Click(object sender, EventArgs e)
        {
            HistogramViewer v = new HistogramViewer();
            v.HistogramCtrl.GenerateHistograms(My_Image, 255);
            v.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                My_Image = new Image<Bgr, byte>(openFile.FileName);
                pictureBox3.Image = My_Image.ToBitmap();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            output = new Image<Gray, byte>(My_Image.Width, My_Image.Height);
            var tmp = My_Image.Convert<Gray, byte>();
            for (int i =0; i<My_Image.Height;i++)
            {
                for(int j=0;j<My_Image.Width;j++)
                {
                    float a = (float)Convert.ToDouble(alfa.Text);
                    int b = Convert.ToInt32(beta.Text);
                    var val = a * tmp[i, j].Intensity + b;
                    output[i, j] =  new Gray(val);
                    //output[i, j] = new Bgr()
                }
            }
            pictureBox3.Image = output.ToBitmap();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Image<Bgr, byte> resized_image;
            resized_image = My_Image.Resize(256, 128, Emgu.CV.CvEnum.Inter.Nearest);
            pictureBox4.Image = resized_image.AsBitmap();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Image<Bgr, byte> rotate_image;
            rotate_image = My_Image.Rotate(90, new Bgr(255, 255, 255));
            pictureBox5.Image = rotate_image.AsBitmap();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Image<Bgr, byte> gamma__Picture;
            var gamma = Convert.ToDouble(numericUpDown1.Value);
            gamma__Picture = My_Image;
            gamma__Picture._GammaCorrect(gamma);
            pictureBox6.Image = gamma__Picture.AsBitmap();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                capture = new VideoCapture(ofd.FileName);
                Mat m = new Mat();
                capture.Read(m);
                pictureBox7.Image = m.ToBitmap();

                TotalFrame = (int)capture.Get(CapProp.FrameCount);
                Fps = capture.Get(CapProp.Fps);
                FrameNo = 1;
                numericUpDown1.Value = FrameNo;
                numericUpDown1.Minimum = 0;
                numericUpDown1.Maximum = TotalFrame;

                if (capture == null)
                {
                    return;
                }
                IsReadingFrame = true;
                ReadAllFrames();

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private async void ReadAllFrames()
        {

            Mat m = new Mat();
            while (IsReadingFrame == true && FrameNo < TotalFrame)
            {
                FrameNo += 1;
                var mat = capture.QueryFrame();
                pictureBox7.Image = mat.ToBitmap();
                await Task.Delay(1000 / Convert.ToInt16(Fps));
                label1.Text = FrameNo.ToString() + "/" + TotalFrame.ToString();
            }
        }
    }
}
