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
            pictureBox2.Image = Metode.convertToGray(My_Image).AsBitmap();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            HistogramViewer v = new HistogramViewer();
            v.HistogramCtrl.GenerateHistograms(My_Image, 255);
            v.Show();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            string a = textBox1.Text;
            float alfa = float.Parse(a);
            a = textBox2.Text;
            int beta = int.Parse(a);
            pictureBox2.Image = Metode.BrightnessAndContrast(My_Image, alfa, beta).AsBitmap();
            pictureBox2.Image = Metode.BrightnessAndContrast(My_Image, alfa, beta).AsBitmap();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = Metode.Red(My_Image).AsBitmap();
        }



        private void button6_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = Metode.Resize(My_Image, 0.2).AsBitmap();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = Metode.Rotate(My_Image, 180).AsBitmap();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = Metode.Gamma(My_Image, (int)numericUpDown1.Value).AsBitmap();
        }

        private async void button9_Click(object sender, EventArgs e)
        {
            List<Bitmap> listImagesReturn = Metode.videoPlay();
            double Fps_val = Metode.fpsReturn();

            for (int i = 0; i < listImagesReturn.Count; i++)
            {
                pictureBox2.Image = listImagesReturn[i];
                await Task.Delay(1000 / Convert.ToInt16(Fps_val));
            }
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
