using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using YoonFactory.Image;

namespace YoonFactory.Viewer
{
    public partial class ImageViewer : UserControl
    {
        protected YoonViewerEventTask m_pEventTask = null;
        protected YoonImage m_pSourceImage = null; 
        private Bitmap m_pImageBuffered = null;
        private Graphics m_pGraphicsBuffered = null;
        private double m_dZoom = 0.0;

        public YoonImage ViewImage
        {
            get => m_pSourceImage;
            set
            {
                if (m_pEventTask != null)
                    m_pEventTask.Dispose();
                m_pSourceImage = value;
                m_pEventTask = new YoonViewerEventTask(m_pSourceImage);
            }
        }

        public double Zoom
        {
            get => m_dZoom;
            set
            {
                vScrollBar.Value = 0;
                hScrollBar.Value = 0;
                m_dZoom = value;
            }
        }

        public YoonVector2N ScrollPosition { get; set; }

        public YoonVector2N RealMousePosition
        {
            get
            {
                Point Pos = this.PointToClient(Control.MousePosition);
                return new YoonVector2N((int)(Pos.X / m_dZoom + 0.5) + hScrollBar.Value, (int)(Pos.Y / m_dZoom + 0.5) + vScrollBar.Value);
            }
        }

        public YoonVector2N StartPosition { get; set; }

        public YoonVector2N CurrentPos { get; set; }

        public YoonRect2N ROI { get; set; }

        public YoonVector2N GetInscreenPosition(YoonVector2N pRealPos)
        {
            return new YoonVector2N((int)((pRealPos.X - hScrollBar.Value) * m_dZoom + 0.5), (int)((pRealPos.Y - vScrollBar.Value) * m_dZoom + 0.5));
        }

        public YoonRect2N GetInscreenArea(YoonRect2N realRect)
        {
            return new YoonRect2N(GetInscreenPosition(realRect.CenterPos as YoonVector2N), (int)(realRect.Width * m_dZoom * 0.5), (int)(realRect.Height * m_dZoom * 0.5));
        }

        public ImageViewer()
        {
            InitializeComponent();

            vScrollBar.Enabled = false;
            hScrollBar.Enabled = false;

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
        }

        private void ImageViewer_Paint(object sender, PaintEventArgs e)
        {
            // Initialize Dummy Image
            if (m_pImageBuffered == null)
                InitDoubleBuffingImage();
            e.Graphics.DrawImage(m_pImageBuffered, new Point(0, 0));
        }

        private void InitDoubleBuffingImage()
        {
            m_pImageBuffered = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            m_pGraphicsBuffered = Graphics.FromImage(m_pImageBuffered);
        }

        public void DrawImage()
        {
            Rectangle pRectDrawing = new Rectangle(0, 0, m_pSourceImage.Width, m_pSourceImage.Height);
            Rectangle pRectDC = ClientRectangle;
            // Normalize Zoom Value (Fitting Image)
            if (m_dZoom <= 0.0)
            {
                vScrollBar.Value = 0;
                hScrollBar.Value = 0;
                double dZoomX = ClientSize.Width / m_pSourceImage.Width;
                double dZoomY = ClientSize.Height / m_pSourceImage.Height;
                m_dZoom = (dZoomX <= dZoomY) ? dZoomX : dZoomY;
                vScrollBar.Enabled = false;
                hScrollBar.Enabled = false;
            }
            else
            {
                Size pSizeDrawing = new Size();
                pSizeDrawing.Width = (int)(pRectDrawing.Width * m_dZoom + .5);
                pSizeDrawing.Height = (int)(pRectDrawing.Height * m_dZoom + .5);
                // If client area is larger than image size
                if (pSizeDrawing.Width <= ClientSize.Width)
                {
                    pRectDC.Width = pSizeDrawing.Width;
                    hScrollBar.Enabled = false;
                }
                else
                    hScrollBar.Enabled = true;
                if (pSizeDrawing.Height <= ClientSize.Height)
                {
                    pRectDC.Height = pSizeDrawing.Height;
                    vScrollBar.Enabled = false;
                }
                else
                    vScrollBar.Enabled = true;
            }
            pRectDrawing.X = hScrollBar.Value;
            pRectDrawing.Y = vScrollBar.Value;
            pRectDrawing.Width = (int)(pRectDC.Width / m_dZoom + 0.5);
            pRectDrawing.Height = (int)(pRectDC.Height / m_dZoom + 0.5);
            hScrollBar.Minimum = 0;
            hScrollBar.LargeChange = pRectDrawing.Width;
            hScrollBar.Maximum = m_pSourceImage.Width;
            vScrollBar.Minimum = 0;
            vScrollBar.LargeChange = pRectDrawing.Height;
            vScrollBar.Maximum = m_pSourceImage.Height;
            // Buffered Image
            m_pGraphicsBuffered.DrawImage(m_pSourceImage.Bitmap, pRectDC, pRectDrawing, GraphicsUnit.Pixel);
            // Refresh Controls;
            Invalidate(false);
        }

        private void OnButtonZoomClick(object sender, EventArgs e)
        {
            toolStripMenuItem_FixToScreen.Checked = false;
            toolStripMenuItem_Zoom10.Checked = false;
            toolStripMenuItem_Zoom50.Checked = false;
            toolStripMenuItem_Zoom100.Checked = false;
            toolStripMenuItem_Zoom200.Checked = false;
            toolStripMenuItem_Zoom400.Checked = false;
            if (sender is ToolStripMenuItem pItem)
            {
                pItem.Checked = true;
                int nIndex = int.Parse((string)pItem.Tag);
                if (nIndex == 0) m_dZoom = 0.0; // Fix To Screen
                if (nIndex == 1) m_dZoom = 0.1; // X0.1
                if (nIndex == 2) m_dZoom = 0.5; // X0.6
                if (nIndex == 3) m_dZoom = 1.0; // X1
                if (nIndex == 4) m_dZoom = 2.0; // X2
                if (nIndex == 5) m_dZoom = 4.0; // X4
                //// Init Double Buffered Image
                InitDoubleBuffingImage();
                DrawImage();
            }
        }

        private void OnButtonROIClick(object sender, EventArgs e)
        {
            //if (m_pEventTask != null)
            //    m_pEventTask.GetWindowRect(m_dZoom);
        }
        
        private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            hScrollBar.Value = e.NewValue;
            DrawImage();
        }

        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            vScrollBar.Value = e.NewValue;
            DrawImage();
        }

        private void ImageViewer_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_pEventTask != null)
                m_pEventTask.OnMouseMoveEvent(e.X, e.Y);
        }

        private void ImageViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_pEventTask != null)
                m_pEventTask.OnMouseDownEvent(sender, e);
        }

        private void ImageViewer_KeyDown(object sender, KeyEventArgs e)
        {
            if(m_pEventTask != null)
            {
                if ((Keys)e.KeyValue == Keys.Escape)
                    m_pEventTask.IsClickESC = true;
            }
        }
    }
}
