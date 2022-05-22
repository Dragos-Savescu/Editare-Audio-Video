using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace lab6
{
    class Metode
    {
        public static int TotalFrame, FrameNo;
        public static double Fps;
        public static bool IsReadingFrame;
        public static VideoCapture capture;


        public static Image<Bgr, Byte> AddImage(Image<Bgr, Byte> My_Image)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                My_Image = new Image<Bgr, byte>(openFile.FileName);

            }
            return My_Image;
        }
        public static Image<Gray, Byte> convertToGray(Image<Bgr, Byte> My_Image)
        {
            Image<Gray, Byte> gray_image = My_Image.Convert<Gray, byte>();
            gray_image[0, 0] = new Gray(200);
            return gray_image;
        }
        public static Image<Bgr, Byte> BrightnessAndContrast(Image<Bgr, Byte> My_Image, double alfa, int beta)
        {
            return (My_Image.Mul(alfa) + beta);
        }
        public static Image<Bgr, Byte> Resize(Image<Bgr, Byte> My_Image, double alfa)
        {
            return My_Image.Resize(alfa, Emgu.CV.CvEnum.Inter.Area);
        }
        public static Image<Bgr, Byte> Rotate(Image<Bgr, Byte> My_Image, int alfa = 180)
        {
            return My_Image.Rotate(180, new Bgr(255, 255, 255));

        }
        public static Image<Bgr, Byte> Gamma(Image<Bgr, Byte> My_Image, int gamma)
        {
            My_Image._GammaCorrect(gamma);
            return My_Image;

        }
        public static Image<Bgr, Byte> Red(Image<Bgr, Byte> My_image)
        {
            Image<Bgr, Byte> outputImage = new Image<Bgr, byte>(My_image.Size);
            My_image.CopyTo(outputImage);
            var data = outputImage.Data;
            for (int i = 0; i < outputImage.Width; i++)
            {
                for (int j = 0; j < outputImage.Height; j++)
                {
                    data[j, i, 0] = 0;
                    data[j, i, 1] = 0;
                }
            }
            return outputImage;
        }

        public static List<Bitmap> videoPlay()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            NumericUpDown numeric = new NumericUpDown();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                capture = new VideoCapture(ofd.FileName);
                Mat m = new Mat();
                capture.Read(m);


                TotalFrame = (int)capture.Get(CapProp.FrameCount);
                Fps = capture.Get(CapProp.Fps);
                FrameNo = 1;
                numeric.Value = FrameNo;
                numeric.Minimum = 0;
                numeric.Maximum = TotalFrame;
            }
            if (capture == null)
            {
                return null;
            }
            IsReadingFrame = true;
            List<Bitmap> listImagesReturn = ReadAllFrames();


            return listImagesReturn;
        }

        public static List<Bitmap> ReadAllFrames()
        {
            List<Bitmap> listImagesReturn = new List<Bitmap>();
            Mat m = new Mat();
            while (IsReadingFrame == true && FrameNo < TotalFrame)
            {
                FrameNo += 1;
                var mat = capture.QueryFrame();
                listImagesReturn.Add(mat.ToBitmap());
            }
            return listImagesReturn;
        }
        public static double fpsReturn()
        {
            return Fps;
        }

    }
}
