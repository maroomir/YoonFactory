using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using YoonFactory;

namespace YoonFactory.Image
{
    public class ScreenMouseEventTask : IDisposable
    {
        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                    m_screenImage.Dispose();
                }
                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        // ~ImageProcess() {
        //   // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
        //   Dispose(false);
        // }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            // GC.SuppressFinalize(this);
        }
        #endregion

        #region 변수 선언부
        // Get Mouse IYoonVector 사용 변수
        protected int m_originRectX1;
        protected int m_originRectX2;
        protected int m_originRectY1;
        protected int m_originRectY2;
        protected Int32 m_prevMousePositionX;
        protected Int32 m_prevMousePositionY;
        protected Int32 m_centerPositionX;
        protected Int32 m_centerPositionY;
        protected eYoonMouseMode m_mouseMode;
        protected eYoonMouseMode m_drawMode;
        protected YoonVector2N m_mousePosition;
        protected YoonVector2N m_currMousePosition;
        protected bool m_isClickMouseLeft;
        protected bool m_isClickMouseRight;
        protected Bitmap m_screenImage;
        protected YoonRect2N m_pCurrentRect;

        public bool IsClickESC { get; private set; }
        #endregion

        public ScreenMouseEventTask(ref Bitmap pScreenImage)
        {
            m_screenImage = pScreenImage;

            ////  Mouse 관련 변수 초기화
            IsClickESC = false;
            m_mouseMode = eYoonMouseMode.GetNone;
        }

        #region 영상처리를 위한 Mouse 조작 Event 처리
        //  Window 상에 Mouse로 그린 사각형을 가져온다.
        public IYoonRect GetWindowRect(double dZoom)
        {
            if (m_screenImage == null)
            {
                MessageBox.Show("Setting Area Error!");
                return new YoonRect2N(0, 0, 0, 0);
            }

            double tempZoom = 1.0;
            YoonVector2N mousePoint;
            YoonRect2N resultRect;
            IsClickESC = false;
            m_mouseMode = eYoonMouseMode.GetPosition;
            ////  1. Image Screen 상에서 Mouse IYoonVector 가져오기.  (함수 실행 時 동작대기 Delay 있음)
            mousePoint = ProcessIYoonVectorMove(tempZoom) as YoonVector2N;
            if (mousePoint.X < 0 || mousePoint.Y < 0)
                return new YoonRect2N(0, 0, 0, 0);
            m_pCurrentRect = new YoonRect2N(mousePoint.X, mousePoint.Y, 0, 0);
            ////  2. 가져온 시작위치의 Mouse IYoonVector 그리기 (영점부터 Mouse 시작점 위치까지)
            ImageFactory.Draw.DrawRect(ref m_screenImage, m_pCurrentRect, 2, Color.Yellow, 1.0);
            m_originRectX1 = m_pCurrentRect.Left;
            m_originRectY1 = m_pCurrentRect.Top;
            m_originRectX2 = m_pCurrentRect.Right;
            m_originRectY2 = m_pCurrentRect.Bottom;
            ////  3. 본격적으로 Mouse의 커서 상태를 설정한다.
            m_mouseMode = eYoonMouseMode.GetRectangle;
            m_drawMode = eYoonMouseMode.GetWidth;
            ////  4. Mouse 움직임으로 사각형을 가져온다.
            ////     참고로 Mouse가 움직이는 동안 계속 사각형을 그린다.
            resultRect = ProcessDrawIYoonRect() as YoonRect2N;
            resultRect.CenterPos.X = (int)Math.Round(resultRect.CenterPos.X / (dZoom));
            resultRect.CenterPos.Y = (int)Math.Round(resultRect.CenterPos.Y / (dZoom));
            resultRect.Width = (int)Math.Round(resultRect.Width / (dZoom));
            resultRect.Height = (int)Math.Round(resultRect.Height / (dZoom));
            return resultRect;
        }

        //  Mouse 클릭.
        public void OnMouseDownEvent(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                OnMouseLeftButtonDownEvent(sender, e);
            else
                OnMouseRightButtonDownEvnet(sender, e);
        }

        public void OnMouseLeftButtonDownEvent(object sender, MouseEventArgs e)
        {
            if (m_screenImage == null)
                return;
            int width, height;
            Point mousePoint;
            width = m_screenImage.Width;
            height = m_screenImage.Height;
            ////  Mouse 설정에 따라 누를 때 동작이 달라짐.
            switch (m_mouseMode)
            {
                //////  마우스 위치 얻기 時 IYoonVector 위치에 XOR(반전) 형태로 십자가가 그려진다.
                case eYoonMouseMode.GetPosition:
                    ImageFactory.Draw.DrawLine(ref m_screenImage, m_prevMousePositionX, 0, m_prevMousePositionX, height, 2, Color.Yellow, 1.0);
                    ImageFactory.Draw.DrawLine(ref m_screenImage, 0, m_prevMousePositionY, width, m_prevMousePositionY, 2, Color.Yellow, 1.0);
                    m_mousePosition.X = e.X;
                    m_mousePosition.Y = e.Y;
                    m_mouseMode = eYoonMouseMode.GetNone;
                    break;
                //////  원형 그리기,  현재는 동작 없음.
                case eYoonMouseMode.GetCircle:
                    m_mousePosition.X = e.X;
                    m_mousePosition.Y = e.Y;
                    m_mouseMode = eYoonMouseMode.GetNone;
                    break;
                //////  사각형 그리기. Mouse Mode에 따라 위치가 달라진다.
                case eYoonMouseMode.GetRectangle:
                    //////  예전에 그린 사각형의 중심 위치로 커서가 옮겨진다.
                    if (m_drawMode == eYoonMouseMode.GetWidth)
                    {
                        m_drawMode = eYoonMouseMode.GetCenter;
                        mousePoint = new Point((int)(m_pCurrentRect.Left + m_pCurrentRect.Right) / 2, (int)(m_pCurrentRect.Top + m_pCurrentRect.Bottom) / 2);
                        //////  Cursor 위치를 옮긴다.
                        Cursor.Current = Cursors.Cross;
                        Cursor.Position = mousePoint;
                    }
                    ////  예전에 그린 사각형의 끝부분 위치로 커서가 옮겨진다.
                    else
                    {
                        m_drawMode = eYoonMouseMode.GetWidth;
                        mousePoint = new Point((int)m_pCurrentRect.Right, (int)m_pCurrentRect.Bottom);
                        //////  Cursor 위치를 옮긴다.
                        Cursor.Current = Cursors.Cross;
                        Cursor.Position = mousePoint;
                    }
                    break;
                default:
                    break;
            }
            m_isClickMouseLeft = true;
        }

        public void OnMouseRightButtonDownEvnet(object sender, MouseEventArgs e)
        {
            if (m_screenImage == null)
                return;
            ////  오직 사각형 그리기 Mode에서만 동작한다.
            switch (m_mouseMode)
            {
                case eYoonMouseMode.GetRectangle:
                    Cursor.Current = Cursors.Default;
                    ImageFactory.Draw.DrawRect(ref m_screenImage, m_pCurrentRect, 1, Color.Aqua, 1.0);
                    m_mouseMode = eYoonMouseMode.GetNone;
                    break;
                default:
                    break;
            }
            m_isClickMouseRight = true;
        }

        public void OnMouseMoveEvent(int X, int Y)
        {
            if (m_screenImage == null)
                return;
            int width, height;
            int dx, dy, radius;
            int centerX, centerY;
            int differX, differY;
            YoonRect2N rect;
            Point mousePoint;
            width = m_screenImage.Width;
            height = m_screenImage.Height;
            ////  그리기를 위한 Mouse 움직임에서 동작한다.
            switch (m_mouseMode)
            {
                case eYoonMouseMode.GetPosition:
                    //////  Mouse가 움직일때마다 십자가를 그린다.
                    ImageFactory.Draw.DrawLine(ref m_screenImage, m_prevMousePositionX, 0, m_prevMousePositionX, height, 1, Color.Yellow, 1.0);
                    ImageFactory.Draw.DrawLine(ref m_screenImage, 0, m_prevMousePositionY, width, m_prevMousePositionY, 1, Color.Yellow, 1.0);
                    m_prevMousePositionX = X;
                    m_prevMousePositionY = Y;
                    ImageFactory.Draw.DrawLine(ref m_screenImage, m_prevMousePositionX, 0, m_prevMousePositionX, height, 1, Color.Yellow, 1.0);
                    ImageFactory.Draw.DrawLine(ref m_screenImage, 0, m_prevMousePositionY, width, m_prevMousePositionY, 1, Color.Yellow, 1.0);
                    break;
                case eYoonMouseMode.GetCircle:
                    dx = X - m_centerPositionX;
                    dy = Y - m_centerPositionY;
                    ////// 두 점 사이 거리를 구하기 위한 공식(Hypot)
                    radius = (int)Math.Sqrt(Math.Pow((double)dx, 2.0) + Math.Pow((double)dy, 2.0));
                    if (m_prevMousePositionX > -4 && m_prevMousePositionY > -4)
                        ImageFactory.Draw.DrawLine(ref m_screenImage, m_centerPositionX, m_centerPositionY, m_prevMousePositionX, m_prevMousePositionY, 1, Color.Yellow, 1.0);
                    m_prevMousePositionX = X;
                    m_prevMousePositionY = Y;
                    ImageFactory.Draw.DrawLine(ref m_screenImage, m_centerPositionX, m_centerPositionY, m_prevMousePositionX, m_prevMousePositionY, 1, Color.Yellow, 1.0);
                    break;
                case eYoonMouseMode.GetRectangle:
                    //////  Mouse 움직임에 따라 Width와 Height가 변해야한다.
                    if (m_drawMode == eYoonMouseMode.GetWidth)
                    {
                        rect = new YoonRect2N(m_originRectX1, m_originRectY1, m_originRectX2 - m_originRectX1, m_originRectY2 - m_originRectY1);
                        ImageFactory.Draw.DrawRect(ref m_screenImage, rect, 1, Color.Yellow, 1.0);
                        //////  우선 현재의 Rect를 그리고...
                        rect.Width = X - rect.Left;
                        rect.Height = Y - rect.Top;
                        differX = rect.Right - rect.Left;
                        differY = rect.Bottom - rect.Top;
                        if (differX < 0)
                            rect.Width = 2;
                        if (differY < 0)
                            rect.Height = 2;
                        if (differX < 0 || differY < 0)
                        {
                            mousePoint = new Point((int)rect.Right, (int)rect.Bottom);
                            Cursor.Position = mousePoint;
                        }
                        //////  움직인 후의 Rect를 다시 그린다.
                        ImageFactory.Draw.DrawRect(ref m_screenImage, rect, 1, Color.Yellow, 1.0);
                        m_originRectX2 = rect.Right;
                        m_originRectY2 = rect.Bottom;
                        m_pCurrentRect.Width = rect.Right - rect.Left;
                        m_pCurrentRect.Height = rect.Bottom - rect.Top;
                    }
                    ////// Mouse 움직임에 따라 Center 위치가 변해야한다.
                    else if (m_drawMode == eYoonMouseMode.GetCenter)
                    {
                        rect = new YoonRect2N(m_originRectX1, m_originRectY1, m_originRectX2 - m_originRectX1, m_originRectY2 - m_originRectY1);
                        ImageFactory.Draw.DrawRect(ref m_screenImage, rect, 1, Color.Yellow, 1.0);
                        //////  우선 현재의 Rect를 그리고...
                        centerX = (m_pCurrentRect.Left + m_pCurrentRect.Right) / 2;      // centerX -= centerX%4;
                        centerY = (m_pCurrentRect.Top + m_pCurrentRect.Bottom) / 2;     // centerY -= centerY%4;
                        m_pCurrentRect = new YoonRect2N(m_pCurrentRect.Left + X - centerX, m_pCurrentRect.Top + Y - centerY,
                            m_pCurrentRect.Width + X - centerX, m_pCurrentRect.Height + Y - centerY);
                        //////  움직인 후의 Rect를 다시 그린다.
                        ImageFactory.Draw.DrawRect(ref m_screenImage, m_pCurrentRect, 1, Color.Yellow, 1.0);
                        m_originRectX1 = m_pCurrentRect.Left;
                        m_originRectY1 = m_pCurrentRect.Top;
                        m_originRectX2 = m_pCurrentRect.Right;
                        m_originRectY2 = m_pCurrentRect.Bottom;
                    }
                    break;
                default:
                    break;
            }
            m_currMousePosition.X = X;   // 외부에서 마우스의 현재 위치를 얻기 위해서.
            m_currMousePosition.Y = Y;
        }

        //  ESC를 누르기 전까지 화면을 Display하며, 최종적으로 확정된 IYoonVector를 Return한다.
        private IYoonVector ProcessIYoonVectorMove(double pZoom)
        {
            YoonVector2N pos = new YoonVector2N(0, 0);
            ////  Mouse의 초기 설정 :  Cross 형태 커서, Mouse Position 가져옴.
            IsClickESC = false;
            Cursor.Current = Cursors.Cross;
            m_mouseMode = eYoonMouseMode.GetPosition;
            m_prevMousePositionX = -5;
            m_prevMousePositionY = -5;
            ////  ESC를 누르거나 Mouse Click 등의 Event로 Mode가 변경될 경우 동작이 Break 된다.
            do
            {
                if (IsClickESC)
                {
                    Cursor.Current = Cursors.Default;
                    m_mousePosition = new YoonVector2N(-1, -1);
                    IsClickESC = false;
                    break;
                }

            } while (m_mouseMode == eYoonMouseMode.GetPosition);
            ////  Mouse의 설정 변화 :  일반 커서, Mouse Mode Default.
            m_mouseMode = eYoonMouseMode.GetNone;
            Cursor.Current = Cursors.Default;
            pos.X = (int)(m_mousePosition.X / (pZoom));
            pos.Y = (int)(m_mousePosition.Y / (pZoom));
            return pos;
        }

        //  ESC를 누르기 전까지 화면을 Display하며, 최종적으로 확정된 IYoonRect를 Return한다.
        private IYoonRect ProcessDrawIYoonRect()
        {
            ////  ESC를 누르거나 Mouse Click 등의 Event로 Mode가 변경될 경우 동작이 Break 된다.
            do
            {
                if (IsClickESC)
                {
                    IsClickESC = false;
                    m_pCurrentRect = new YoonRect2N(0, 0, 0, 0);
                    break;
                }
            } while (m_mouseMode == eYoonMouseMode.GetRectangle);
            return m_pCurrentRect;   //RectWin;
        }
        #endregion

    }

    /* byte 및 int 배열 Data를 Show 가능한 형태(Bitmap)로 변환시킴 */
    public class DisplayManager : IDisposable
    {
        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                    m_bitmap8Bit.Dispose();
                    m_bitmap24Bit.Dispose();
                }
                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        // ~ImageProcess() {
        //   // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
        //   Dispose(false);
        // }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            // GC.SuppressFinalize(this);
        }
        #endregion

        #region 변수 선언부
        // Image Contain 변수
        protected Bitmap m_bitmap8Bit;
        protected Bitmap m_bitmap24Bit;
        #endregion

        public DisplayManager()
        {
            ////  Image 초기화
            ////  Files\\ 위치에 해당 Bitmap이 있지 않은 경우, Error 발생해서 기본 초기화함.
            string imagePath;
            int defaultWidth = 640;
            int defaultHeight = 480;
            FileInfo exeFileInfo = new FileInfo(Application.ExecutablePath);
            try
            {
                imagePath = exeFileInfo.Directory.FullName.ToString() + "\\Files\\Default8.bmp";
                m_bitmap8Bit = new Bitmap(imagePath);
                imagePath = exeFileInfo.Directory.FullName.ToString() + "\\Files\\Default24.bmp";
                m_bitmap24Bit = new Bitmap(imagePath);
            }
            catch
            {
                m_bitmap8Bit = new Bitmap(defaultWidth, defaultHeight, PixelFormat.Format8bppIndexed);
                m_bitmap24Bit = new Bitmap(defaultWidth, defaultHeight, PixelFormat.Format24bppRgb);
            }
        }

        #region Buffer 내용 그리기
        //  Buffer에 저장된 Image를 화면상에 보여준다.
        public Bitmap DisplayBuffer(YoonRect2N destArea, byte[] pBuffer, int width, int height, YoonRect2N sourceArea)
        {
            int x, y;
            int sourceWidth, sourceHeight;
            byte[] pByte;
            sourceWidth = sourceArea.Width;
            sourceHeight = sourceArea.Height;

            if (m_bitmap8Bit.Width != sourceWidth || m_bitmap8Bit.Height != sourceHeight)
            {
                m_bitmap8Bit = new Bitmap(sourceWidth, sourceHeight, PixelFormat.Format8bppIndexed);
            }
            ////  pBuffer를 pByte에 채운다.
            for (int j = 0; j < sourceHeight; j++)
            {
                y = (int)sourceArea.Top + j;
                if (y >= height) continue;
                pByte = new byte[sourceWidth];
                for (int i = 0; i < sourceWidth; i++)
                {
                    x = (int)sourceArea.Left + i;
                    if (x >= width) continue;
                    pByte[i] = pBuffer[y * width + x];
                }
                MemoryControl.PrintLine(pByte, ref m_bitmap8Bit, j);
            }
            return BitmapFactory.CopyBitmap(m_bitmap8Bit);
        }

        public Bitmap DisplayBuffer(byte[] pBuffer, int width, int height, IYoonRect dispArea)
        {
            byte[] pByte;
            if (m_bitmap8Bit.Width != width || m_bitmap8Bit.Height != height)
            {
                m_bitmap8Bit = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            }
            ////  pBuffer를 pByte에 채운다.
            for (int j = 0; j < height; j++)
            {
                pByte = new byte[width];
                for (int i = 0; i < width; i++)
                {
                    pByte[i] = Math.Max((byte)0, Math.Min(pBuffer[j * width + i], (byte)255));
                }
                MemoryControl.PrintLine(pByte, ref m_bitmap8Bit, j);
            }
            return BitmapFactory.CopyBitmap(m_bitmap8Bit);
        }

        public Bitmap DisplayBuffer(ref int[] pBuffer, int width, int height, IYoonRect dispArea, int offset)
        {
            byte[] pByte;
            if (m_bitmap8Bit.Width != width || m_bitmap8Bit.Height != height)
            {
                m_bitmap8Bit = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            }
            ////  pBuffer를 pByte에 채운다.
            for (int j = 0; j < height; j++)
            {
                pByte = new byte[width];
                for (int i = 0; i < width; i++)
                {
                    pByte[i] = (byte)Math.Max(0, Math.Min(pBuffer[j * width + i] + offset, 255));
                }
                MemoryControl.PrintLine(pByte, ref m_bitmap8Bit, j);
            }
            return BitmapFactory.CopyBitmap(m_bitmap8Bit);
        }

        public Bitmap DisplayBuffer(ref float[] pBuffer, int width, int height, IYoonRect dispArea, float offset)
        {
            float value;
            byte[] pByte;
            if (m_bitmap8Bit.Width != width || m_bitmap8Bit.Height != height)
            {
                m_bitmap8Bit = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            }
            ////  pBuffer를 pByte에 채운다.
            for (int j = 0; j < height; j++)
            {
                pByte = new byte[width];
                for (int i = 0; i < width; i++)
                {
                    value = pBuffer[j * width + i] + offset;
                    if (value < 0) value = 0;
                    if (value > 255) value = 255;
                    pByte[i] = (byte)value;
                }
                MemoryControl.PrintLine(pByte, ref m_bitmap8Bit, j);
            }
            return BitmapFactory.CopyBitmap(m_bitmap8Bit);
        }

        public Bitmap DisplayBuffer(ref byte[] pRed, ref byte[] pGreen, ref byte[] pBlue, int width, int height, IYoonRect dispArea)
        {
            byte[] pByte;
            if (m_bitmap24Bit.Width != width || m_bitmap24Bit.Height != height)
            {
                m_bitmap24Bit = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            }
            ////  pBuffer를 pByte에 채운다.
            for (int j = 0; j < height; j++)
            {
                pByte = new byte[width * 3];
                for (int i = 0; i < width; i++)
                {
                    pByte[3 * i + 0] = pBlue[j * width + i];
                    pByte[3 * i + 1] = pGreen[j * width + i];
                    pByte[3 * i + 2] = pRed[j * width + i];
                }
                MemoryControl.PrintLine(pByte, ref m_bitmap24Bit, j);
            }
            return BitmapFactory.CopyBitmap(m_bitmap24Bit);
        }
        #endregion

    }

}
