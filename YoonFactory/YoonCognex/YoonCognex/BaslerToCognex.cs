using System;
using Cognex.VisionPro;
using System.Runtime.InteropServices;
using Basler.Pylon;

namespace YoonFactory.Cognex
{

    /// <summary>
    /// Cognex 상용 Library에서 Evnet 발생으로 Basler Camera 상에 Image를 동기화시킨다.
    /// </summary>
    class CogGrabEndEventArgs : EventArgs
    {
        public CogImage8Grey cogImage;

        public CogGrabEndEventArgs(CogImage8Grey _cogImage)
        {
            cogImage = _cogImage;
        }

    }

    /// <summary>
    /// Cognex 상용 라이브러리를 통해 Basler 사용을 위한 Class
    /// 위 함수를 사용하기 위해선 다음 namespace가 선언되고, 개체 브라우저에 다음 참조가 추가되어야 한다.
    ///    =>  Basler.Pylon
    ///    =>  Cognex.VisionPro
    /// </summary>
    class BaslerToCognex
    {
        public delegate void GrabEndEventHandler(CogGrabEndEventArgs v);
        public event GrabEndEventHandler OnGrabEndEvent;

        private Camera camera;
        private ICameraInfo cameraInfo = null;
        private PixelDataConverter converter = new PixelDataConverter();
        private CogImage8Grey _cogImage = new CogImage8Grey();
        ICogImage8PixelMemory _cogMemory;

        private Object GrabLock = new Object();

        public BaslerToCognex(ICameraInfo _cameraInfo)
        {
            cameraInfo = _cameraInfo;

            CameraInitialize();
        }

        /// <summary>
        /// Camera를 초기화한다.
        /// </summary>
        private void CameraInitialize()
        {
            try
            {
                camera = new Camera(cameraInfo);

                camera.Open();

                UserSetLoad();

                ////  정상적인 Cognex 변환이 안될 경우, 32의 배수로 변환시킬 것.
                //int width =(int) (checked((int)camera.Parameters[PLCamera.Width].GetValue()) / 32 ) * 32;
                int width = (int)camera.Parameters[PLCamera.Width].GetValue();
                int height = (int)camera.Parameters[PLCamera.Height].GetValue();

                _cogImage.Allocate(width, height);
                _cogMemory = _cogImage.Get8GreyPixelMemory(CogImageDataModeConstants.ReadWrite, 0, 0, width, height);

                camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;

                ////  Image를 32의 배수로 전환시켰을 경우 사용함.
                //camera.Parameters[PLCamera.Width].SetValue(width, IntegerValueCorrection.Nearest);
                //camera.Parameters[PLTransportLayer.HeartbeatTimeout].TrySetValue(10000, IntegerValueCorrection.Nearest);  // not use
                //Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "3000");

                camera.Parameters[PLCamera.GainAuto].TrySetValue(PLCamera.GainAuto.Off);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                camera.Close();
            }
        }

        public void UserSetLoad()
        {
            camera.Parameters[PLCamera.UserSetSelector].SetValue(PLGigECamera.UserSetSelector.UserSet1);
            camera.Parameters[PLCamera.UserSetLoad].Execute();
        }

        public void UserSetSave()
        {
            camera.Parameters[PLCamera.UserSetDefaultSelector].SetValue(PLGigECamera.UserSetSelector.UserSet1);
            camera.Parameters[PLCamera.UserSetSave].Execute();
        }

        /// <summary>
        /// Camera의 작동을 중단시킨다.
        /// </summary>
        public void Stop()
        {
            ////  Grab을 중단시킨다.
            try
            {
                camera.StreamGrabber.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public long GetExpoureTime()
        {
            long value = 0;
            try
            {
                value = camera.Parameters[PLCamera.ExposureTimeRaw].GetValue();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return value;
        }
        public long GetGain()
        {
            long value = 0;
            try
            {
                value = camera.Parameters[PLCamera.GainRaw].GetValue();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return value;
        }
        public void SetExpoureTime(long SetVal)
        {

            try
            {
                camera.Parameters[PLCamera.ExposureTimeAbs].SetValue(SetVal);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        public void SetGain(long SetVal)
        {
            try
            {
                camera.Parameters[PLCamera.GainRaw].SetValue(SetVal);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        /// <summary>
        /// Camera 연결을 끊는다.
        /// </summary>
        public void DestroyCamera()
        {
            ////  선언된 Camera 객체를 지운다.
            try
            {
                if (camera != null)
                {
                    camera.Close();
                    camera.Dispose();
                    camera = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 1회 촬영한다.
        /// </summary>
        public void OneShot()
        {
            try
            {
                ////  1장의 Frame을 취득한다.
                camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.SingleFrame);
                camera.StreamGrabber.Start(1, GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 연속 촬영한다.
        /// </summary>
        public void ContinuousShot()
        {
            try
            {
                ////  Grab을 중단할 때까지 Image를 계속 취득한다.
                camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.Continuous);
                camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);

                GC.Collect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Grab 후 BaslerToCognex Class를 통해 외부 Event를 발생시킨다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnImageGrabbed(Object sender, ImageGrabbedEventArgs e)
        {

            try
            {
                ////  Camera에서 Image를 받으며, 오직 마지막 Image 만 보여진다.
                ////  Camera가 Display 되는 속도보다 더 빨리 image를 취득할 수 있는 경우, Lock을 걸어둔다.
                lock (GrabLock)
                {
                    // Get the grab result.
                    IGrabResult grabResult = e.GrabResult;

                    // Check if the image can be displayed.
                    if (grabResult.IsValid)
                    {
                        CogImage8Grey cogImage = new CogImage8Grey();

                        cogImage.Allocate(grabResult.Width, grabResult.Height);
                        ICogImage8PixelMemory cogMemory = cogImage.Get8GreyPixelMemory(CogImageDataModeConstants.ReadWrite, 0, 0, grabResult.Width, grabResult.Height);

                        byte[] buffer = grabResult.PixelData as byte[];
                        Marshal.Copy(buffer, 0, cogMemory.Scan0, cogMemory.Width * cogMemory.Height);

                        cogImage.Copy(CogImageCopyModeConstants.CopyPixels);

                        cogMemory.Dispose();

                        //Event 발생 
                        CogGrabEndEventArgs gv = new CogGrabEndEventArgs(cogImage);
                        OnGrabEnd(gv);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                // Dispose the grab result if needed for returning it to the grab loop.
                e.DisposeGrabResultIfClone();
            }
        }

        /// <summary>
        /// Grab 후 Event를 실행시킨다.
        /// </summary>
        /// <param name="v"></param>
        protected virtual void OnGrabEnd(CogGrabEndEventArgs v)
        {
            OnGrabEndEvent?.Invoke(v);
        }
    }
}
