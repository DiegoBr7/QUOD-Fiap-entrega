using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace DeteccaoRostoEmguCV
{
    public partial class FrmDeteccaoRosto : Form
    {

        static readonly CascadeClassifier cascadeClassifier = new CascadeClassifier("haarcascade_frontalface_alt_tree.xml");
        VideoCapture capture;

        public FrmDeteccaoRosto()
        {
            InitializeComponent();
        }

        private void FrmDeteccaoRosto_Load(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Multiselect = false, Filter = "JPEG|* .jpg" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    ProcessarImagem(new Bitmap(Image.FromFile(ofd.FileName)));

                }
            }
        }


        private void PicImagem_Click(object sender, EventArgs e)
        {
            if (capture != null)
            {
                MessageBox.Show("A webcam ja foi iniciada");
                return;
            }

            capture = new VideoCapture(0);
            capture.ImageGrabbed += Capture_ImageGrabbed;
            capture.Start();

        }

        private void Capture_ImageGrabbed(object sender, EventArgs e)
        {

            try
            {
            
                Mat m = new Mat();
                capture.Retrieve(m);
                ProcessarImagem(m.Bitmap);
            }
            catch (Exception ex) 
            { 
            MessageBox.Show(ex.Message);
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (capture == null)
            {

                MessageBox.Show("A webcam esta parada");
                return;
            }

            capture.ImageGrabbed -= Capture_ImageGrabbed;
            capture.Stop();
            capture.Dispose();
            capture = null;
            picImagem = null;
        }

        private void ProcessarImagem(Bitmap bitmap)
        {
            Image<Bgr, byte> grayImage = new Image<Bgr, byte>(bitmap);
            Rectangle[] rectangles = cascadeClassifier.DetectMultiScale(grayImage, 1.1, 1);
            foreach (var rectangle in rectangles)
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {

                    using (Pen pen = new Pen(Color.LimeGreen, 3))
                    {

                        graphics.DrawRectangle(pen, rectangle);
                    }
                }
                picImagem.Image = bitmap;
            }
        }
    }
}
