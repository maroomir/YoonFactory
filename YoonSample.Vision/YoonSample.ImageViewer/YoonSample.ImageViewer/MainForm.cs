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
using YoonFactory;
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
            CommonClass.pCLM.Write("Load Form Completed");
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CommonClass.pCLM.Write("Program Exit");
            CommonClass.pCamera = null;
            CommonClass.pImage.Dispose();
            CommonClass.pCLM.Dispose();
        }

        private void OnUpdateCameraImage(object sender, FrameArgs e)
        {
            if (CommonClass.pCamera == null) return;
            switch (m_nSelectedCamera)
            {
                case eYoonCamera.BaslerColor:
                    CommonClass.pImage = new YoonImage(e.pAddressBuffer, (int)e.Width, (int)e.Height, 3);
                    imageViewer_Main.ViewImage = CommonClass.pImage;
                    break;
                case eYoonCamera.RealsenseColor:
                    CommonClass.pImage = new YoonImage(e.pAddressBuffer, (int)e.Width, (int)e.Height, 3, eYoonRGBMode.Mixed);
                    break;
                case eYoonCamera.BaslerMono:
                    CommonClass.pImage = new YoonImage(e.pAddressBuffer, (int)e.Width, (int)e.Height, 1);
                    break;
            }
            imageViewer_Main.ViewImage = CommonClass.pImage;
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
            OpenFileDialog pDlg = new OpenFileDialog();
            pDlg.Filter = "bitmap files (*.bmp)|*.bmp|jpeg files (*.jpg)|*.jpg";
            if (pDlg.ShowDialog() == DialogResult.OK)
            {
                if (CommonClass.pImage != null)
                    CommonClass.pImage.Dispose();

                CommonClass.pImage.LoadImage(pDlg.FileName);
                imageViewer_Main.ViewImage = CommonClass.pImage;
                imageViewer_Main.DrawImage();
            }
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            if (CommonClass.pImage == null) return;
            SaveFileDialog pDlg = new SaveFileDialog();
            pDlg.Filter = "bitmap files (*.bmp)|*.bmp|jpeg files (*.jpg)|*.jpg";
            if (pDlg.ShowDialog() == DialogResult.OK)
            {
                CommonClass.pImage.SaveImage(pDlg.FileName);
            }
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
            switch (m_nSelectedCamera)
            {
                case eYoonCamera.BaslerMono:
                    CommonClass.pCamera = new YoonBasler();
                    imageViewer_Main.ViewImage = new YoonImage(CommonClass.pCamera.ImageWidth, CommonClass.pCamera.ImageHeight, PixelFormat.Format8bppIndexed);
                    break;
                case eYoonCamera.BaslerColor:
                    CommonClass.pCamera = new YoonBasler();
                    imageViewer_Main.ViewImage = new YoonImage(CommonClass.pCamera.ImageWidth, CommonClass.pCamera.ImageHeight, PixelFormat.Format24bppRgb);
                    break;
                case eYoonCamera.RealsenseColor:
                    CommonClass.pCamera = new YoonRealsense(eYoonRSCaptureMode.RGBColor);
                    imageViewer_Main.ViewImage = new YoonImage(CommonClass.pCamera.ImageWidth, CommonClass.pCamera.ImageHeight, PixelFormat.Format24bppRgb);
                    break;
                default:
                    return;
            }
            try
            {
                CommonClass.pCamera.OpenCamera(0);
                CommonClass.pCamera.StartCamera();
                CommonClass.pCamera.OnCameraImageUpdateEvent += OnUpdateCameraImage;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                CommonClass.pCamera = null;
            }
        }
    }
}
