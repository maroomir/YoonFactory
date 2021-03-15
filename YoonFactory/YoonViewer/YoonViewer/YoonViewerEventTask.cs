using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using YoonFactory;

namespace YoonFactory.Image
{
    public class YoonViewerEventTask : IDisposable
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
                    if(m_screenImage != null)
                        m_screenImage.Dispose();
                }
                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        // ~YoonScreenEventTask() {
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
        protected bool m_isClickMouseLeft;
        protected bool m_isClickMouseRight;
        protected eYoonMouseMode m_mouseMode;
        protected eYoonMouseMode m_drawMode;
        protected YoonVector2N m_mousePosition;
        protected YoonVector2N m_currMousePosition = new YoonVector2N();
        protected YoonVector2N m_prevMousePosition = new YoonVector2N();
        protected YoonRect2N m_pCurrentRect;
        protected YoonVector2N m_pOriginRectTopLeft;
        protected YoonVector2N m_pOriginRectBottomRight;
        protected YoonVector2N m_pOriginCenter = new YoonVector2N();
        protected YoonImage m_screenImage;

        public bool IsClickESC { get; set; }
        #endregion

        public YoonViewerEventTask(YoonImage pScreenImage)
        {
            m_screenImage = pScreenImage;

            ////  Mouse 관련 변수 초기화
            IsClickESC = false;
            m_mouseMode = eYoonMouseMode.None;
        }

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
            mousePoint = ProcessPointMove(tempZoom) as YoonVector2N;
            if (mousePoint.X < 0 || mousePoint.Y < 0)
                return new YoonRect2N(0, 0, 0, 0);
            m_pCurrentRect = new YoonRect2N(mousePoint.X, mousePoint.Y, 0, 0);
            ////  2. 가져온 시작위치의 Mouse IYoonVector 그리기 (영점부터 Mouse 시작점 위치까지)
            m_screenImage.DrawRect(m_pCurrentRect, Color.Yellow, 2, 1.0);
            m_pOriginRectTopLeft.X = m_pCurrentRect.Left;
            m_pOriginRectTopLeft.Y = m_pCurrentRect.Top;
            m_pOriginRectBottomRight.X = m_pCurrentRect.Right;
            m_pOriginRectBottomRight.Y = m_pCurrentRect.Bottom;
            ////  3. 본격적으로 Mouse의 커서 상태를 설정한다.
            m_mouseMode = eYoonMouseMode.GetRectangle;
            m_drawMode = eYoonMouseMode.GetWidth;
            ////  4. Mouse 움직임으로 사각형을 가져온다.
            ////     참고로 Mouse가 움직이는 동안 계속 사각형을 그린다.
            resultRect = ProcessDrawRect() as YoonRect2N;
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
                    m_screenImage.DrawLine(m_prevMousePosition.X, 0, m_prevMousePosition.X, height, Color.Yellow, 2, 1.0);
                    m_screenImage.DrawLine(0, m_prevMousePosition.Y, width, m_prevMousePosition.Y, Color.Yellow, 2, 1.0);
                    m_mousePosition.X = e.X;
                    m_mousePosition.Y = e.Y;
                    m_mouseMode = eYoonMouseMode.None;
                    break;
                //////  원형 그리기,  현재는 동작 없음.
                case eYoonMouseMode.GetCircle:
                    m_mousePosition.X = e.X;
                    m_mousePosition.Y = e.Y;
                    m_mouseMode = eYoonMouseMode.None;
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
                    m_screenImage.DrawRect(m_pCurrentRect, Color.Aqua, 1, 1.0);
                    m_mouseMode = eYoonMouseMode.None;
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
                    m_screenImage.DrawLine(m_prevMousePosition.X, 0, m_prevMousePosition.X, height, Color.Yellow, 1, 1.0);
                    m_screenImage.DrawLine(0, m_prevMousePosition.Y, width, m_prevMousePosition.Y, Color.Yellow, 1, 1.0);
                    m_prevMousePosition.X = X;
                    m_prevMousePosition.Y = Y;
                    m_screenImage.DrawLine(m_prevMousePosition.X, 0, m_prevMousePosition.X, height, Color.Yellow, 1, 1.0);
                    m_screenImage.DrawLine(0, m_prevMousePosition.Y, width, m_prevMousePosition.Y, Color.Yellow, 1, 1.0);
                    break;
                case eYoonMouseMode.GetCircle:
                    dx = X - m_pOriginCenter.X;
                    dy = Y - m_pOriginCenter.Y;
                    ////// 두 점 사이 거리를 구하기 위한 공식(Hypot)
                    radius = (int)Math.Sqrt(Math.Pow((double)dx, 2.0) + Math.Pow((double)dy, 2.0));
                    if (m_prevMousePosition.X > -4 && m_prevMousePosition.Y > -4)
                        m_screenImage.DrawLine(m_pOriginCenter, m_prevMousePosition, Color.Yellow, 1, 1.0);
                    m_prevMousePosition.X = X;
                    m_prevMousePosition.Y = Y;
                    m_screenImage.DrawLine(m_pOriginCenter, m_prevMousePosition, Color.Yellow, 1, 1.0);
                    break;
                case eYoonMouseMode.GetRectangle:
                    //////  Mouse 움직임에 따라 Width와 Height가 변해야한다.
                    if (m_drawMode == eYoonMouseMode.GetWidth)
                    {
                        rect = new YoonRect2N(m_pOriginRectTopLeft, m_pOriginRectBottomRight);
                        m_screenImage.DrawRect(rect, Color.Yellow, 1, 1.0);
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
                        m_screenImage.DrawRect(rect, Color.Yellow, 1, 1.0);
                        m_pOriginRectBottomRight.X = rect.Right;
                        m_pOriginRectBottomRight.Y = rect.Bottom;
                        m_pCurrentRect.Width = rect.Right - rect.Left;
                        m_pCurrentRect.Height = rect.Bottom - rect.Top;
                    }
                    ////// Mouse 움직임에 따라 Center 위치가 변해야한다.
                    else if (m_drawMode == eYoonMouseMode.GetCenter)
                    {
                        rect = new YoonRect2N(m_pOriginRectTopLeft, m_pOriginRectBottomRight);
                        m_screenImage.DrawRect(rect, Color.Yellow, 1, 1.0);
                        //////  우선 현재의 Rect를 그리고...
                        centerX = (m_pCurrentRect.Left + m_pCurrentRect.Right) / 2;      // centerX -= centerX%4;
                        centerY = (m_pCurrentRect.Top + m_pCurrentRect.Bottom) / 2;     // centerY -= centerY%4;
                        m_pCurrentRect = new YoonRect2N(m_pCurrentRect.Left + X - centerX, m_pCurrentRect.Top + Y - centerY,
                            m_pCurrentRect.Width + X - centerX, m_pCurrentRect.Height + Y - centerY);
                        //////  움직인 후의 Rect를 다시 그린다.
                        m_screenImage.DrawRect(m_pCurrentRect, Color.Yellow, 1, 1.0);
                        m_pOriginRectTopLeft.X = m_pCurrentRect.Left;
                        m_pOriginRectTopLeft.Y = m_pCurrentRect.Top;
                        m_pOriginRectBottomRight.X = m_pCurrentRect.Right;
                        m_pOriginRectBottomRight.Y = m_pCurrentRect.Bottom;
                    }
                    break;
                default:
                    break;
            }
            m_currMousePosition.X = X;   // 외부에서 마우스의 현재 위치를 얻기 위해서.
            m_currMousePosition.Y = Y;
        }

        //  ESC를 누르기 전까지 화면을 Display하며, 최종적으로 확정된 IYoonVector를 Return한다.
        private IYoonVector ProcessPointMove(double pZoom)
        {
            YoonVector2N pos = new YoonVector2N(0, 0);
            ////  Mouse의 초기 설정 :  Cross 형태 커서, Mouse Position 가져옴.
            IsClickESC = false;
            Cursor.Current = Cursors.Cross;
            m_mouseMode = eYoonMouseMode.GetPosition;
            m_prevMousePosition.X = -5;
            m_prevMousePosition.Y = -5;
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
            m_mouseMode = eYoonMouseMode.None;
            Cursor.Current = Cursors.Default;
            pos.X = (int)(m_mousePosition.X / (pZoom));
            pos.Y = (int)(m_mousePosition.Y / (pZoom));
            return pos;
        }

        //  ESC를 누르기 전까지 화면을 Display하며, 최종적으로 확정된 IYoonRect를 Return한다.
        private IYoonRect ProcessDrawRect()
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
    }

}
