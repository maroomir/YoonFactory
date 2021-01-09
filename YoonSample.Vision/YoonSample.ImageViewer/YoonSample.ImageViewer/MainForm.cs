using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using YoonFactory.Image;
using YoonFactory.Camera;
using YoonFactory.Camera.Basler;
using YoonFactory.Camera.Realsense;

namespace YoonSample.ImageViewer
{
    public partial class MainForm : Form
    {
        private eYoonCamera m_nSelectedCamera = eYoonCamera.None;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CommonClass.pCLM.Write("Init Camera List");
            {
                foreach (string strCamera in Enum.GetNames(typeof(eYoonCamera)))
                {
                    if (strCamera == "None" || strCamera == "RealsenseDepth") continue;
                    comboBox_Camera.Items.Add(strCamera);
                }
            }
            CommonClass.pCLM.Write("Init Image Viewer");
            {
                imageViewer_Main.OnGetGuidePoint += OnUpdateCurrentPoint;
                imageViewer_Main.OnGetPixel += OnUpdateCurrentPixel;
                imageViewer_Main.OnMeasurement += OnUpdateCurrentMeasurement;
            }
            CommonClass.pCLM.Write("Load Form Completed");
        }

        private void OnUpdateCurrentMeasurement(object sender, YoonFactory.Viewer.MeasureArgs e)
        {
            //
        }

        private void OnUpdateCurrentPixel(object sender, YoonFactory.Viewer.PixelArgs e)
        {
            //
        }

        private void OnUpdateCurrentPoint(object sender, YoonFactory.Viewer.PointArgs e)
        {
            //
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CommonClass.pCLM.Write("Program Exit");
            CommonClass.pCamera = null;
            CommonClass.pCLM.Dispose();
        }

        private void OnUpdateCameraImage(object sender, FrameArgs e)
        {
            if (CommonClass.pCamera == null) return;
            switch (m_nSelectedCamera)
            {
                case eYoonCamera.BaslerColor:
                    imageViewer_Main.InputImage = ImageFactory.Converter.ToBitmap24BitWithColorParallel(e.pAddressBuffer, CommonClass.pCamera.ImageWidth, CommonClass.pCamera.ImageHeight);
                    break;
                case eYoonCamera.RealsenseColor:
                    imageViewer_Main.InputImage = ImageFactory.Converter.ToBitmap24BitWithColorMixed(e.pAddressBuffer, CommonClass.pCamera.ImageWidth, CommonClass.pCamera.ImageHeight);
                    break;
                case eYoonCamera.BaslerMono:
                    imageViewer_Main.InputImage = ImageFactory.Converter.ToBitmap8Bit(e.pAddressBuffer, CommonClass.pCamera.ImageWidth, CommonClass.pCamera.ImageHeight);
                    break;
            }
            imageViewer_Main.DrawImage();
        }

        private void button_Live_Click(object sender, EventArgs e)
        {
            if (CommonClass.pCamera == null || !CommonClass.pCamera.IsStartCamera) return;
            if (CommonClass.pCamera.IsLiveOn)
                CommonClass.pCamera.LiveOff();
            else
                CommonClass.pCamera.LiveOn();
        }

        private void button_Capture_Click(object sender, EventArgs e)
        {
            if (CommonClass.pCamera == null || !CommonClass.pCamera.IsStartCamera) return;
            if (CommonClass.pCamera.IsLiveOn)
                CommonClass.pCamera.LiveOff();

            CommonClass.pCamera.GetImage(1000);
        }

        private void button_Open_Click(object sender, EventArgs e)
        {
            //
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            //
        }

        private void button_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button_SetCamera_Click(object sender, EventArgs e)
        {
            if (CommonClass.pCamera != null)
            {
                CommonClass.pCamera.LiveOff();
                CommonClass.pCamera = null;
            }

            m_nSelectedCamera = (eYoonCamera)Enum.Parse(typeof(eYoonCamera), comboBox_Camera.SelectedItem.ToString());
            imageViewer_Main.IsEnabledDraw = true;
            switch (m_nSelectedCamera)
            {
                case eYoonCamera.BaslerMono:
                    CommonClass.pCamera = new YoonBasler();
                    imageViewer_Main.InputImage = new Bitmap(CommonClass.pCamera.ImageWidth, CommonClass.pCamera.ImageHeight, PixelFormat.Format8bppIndexed);
                    break;
                case eYoonCamera.BaslerColor:
                    CommonClass.pCamera = new YoonBasler();
                    imageViewer_Main.InputImage = new Bitmap(CommonClass.pCamera.ImageWidth, CommonClass.pCamera.ImageHeight, PixelFormat.Format24bppRgb);
                    break;
                case eYoonCamera.RealsenseColor:
                    CommonClass.pCamera = new YoonRealsense(eYoonRSCaptureMode.RGBColor);
                    imageViewer_Main.InputImage = new Bitmap(CommonClass.pCamera.ImageWidth, CommonClass.pCamera.ImageHeight, PixelFormat.Format24bppRgb);
                    break;
                default:
                    return;
            }
            try
            {
                CommonClass.pCamera.OpenCamera(0);
                CommonClass.pCamera.StartCamera();
                CommonClass.pCamera.OnCameraImageUpdateEvent += OnUpdateCameraImage;
                imageViewer_Main.SetImageSize(CommonClass.pCamera.ImageWidth, CommonClass.pCamera.ImageHeight);
                imageViewer_Main.SetDoubleBuffering();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                CommonClass.pCamera = null;
            }
        }
    }
}
