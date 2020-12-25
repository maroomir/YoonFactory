using System;
using Gtk;
using Pango;
using YoonFactory.Mono;
namespace YoonSample.Mono
{
    public class ActionPanel : IDisposable
    {
        public int Order { get; private set; }
        public string Name { get; private set; }
        public string Value { get; private set; }
        public HBox ParameterHBox { get; private set; }

        private CheckButton checkButton_IsSelected;
        private Entry entry_ParamName;
        private Entry entry_ParamValue;

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제하고 아래의 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                ParameterHBox.Remove(checkButton_IsSelected);
                ParameterHBox.Remove(entry_ParamName);
                ParameterHBox.Remove(entry_ParamValue);

                checkButton_IsSelected.Dispose();
                entry_ParamName.Dispose();
                entry_ParamValue.Dispose();

                checkButton_IsSelected = null;
                entry_ParamName = null;
                entry_ParamValue = null;
                ParameterHBox = null;

                disposedValue = true;
            }
        }

        public ActionPanel(int nNo)
        {
            Order = nNo;

            checkButton_IsSelected = new CheckButton(Order.ToString());
            entry_ParamName = new Entry();
            entry_ParamValue = new Entry();
            SetComponentName(Order);

            ParameterHBox = new HBox();
            {
                int iItem = 0;
                Box pBoxParam = ParameterHBox as Box;
                ParameterHBox.Spacing = 6;
                GtkFactory.SetWidgetInBoxLayout(ref pBoxParam, checkButton_IsSelected, iItem++);
                GtkFactory.SetWidgetInBoxLayout(ref pBoxParam, entry_ParamName, iItem++);
                GtkFactory.SetWidgetInBoxLayout(ref pBoxParam, entry_ParamValue, iItem++);

                ParameterHBox.Show();
            }
        }

        public ActionPanel(int nNo, string strName, string strValue)
        {
            Order = nNo;
            Name = strName;
            Value = strValue;

            checkButton_IsSelected = new CheckButton(Order.ToString());
            entry_ParamName = new Entry();
            entry_ParamValue = new Entry();
            SetComponentName(Order);
            SetComponentValue();

            ParameterHBox = new HBox();
            {
                int iItem = 0;
                Box pBoxParam = ParameterHBox as Box;
                ParameterHBox.Spacing = 6;
                GtkFactory.SetWidgetInBoxLayout(ref pBoxParam, checkButton_IsSelected, iItem++);
                GtkFactory.SetWidgetInBoxLayout(ref pBoxParam, entry_ParamName, iItem++);
                GtkFactory.SetWidgetInBoxLayout(ref pBoxParam, entry_ParamValue, iItem++);

                ParameterHBox.Show();
            }
        }

        // TODO: 위의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다.
        ~ActionPanel()
        {
            Dispose(false);
        }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 위의 Dispose(bool disposing)에 정리 코드를 입력하세요.
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            //GC.SuppressFinalize(this);
        }
        #endregion

        private void SetComponentName(int nNo)
        {
            checkButton_IsSelected.Name = string.Format("checkButton_IsSelected_Param{0}", nNo);
            entry_ParamName.Name = string.Format("entry_ParamName_Param{0}", nNo);
            entry_ParamValue.Name = string.Format("entry_ParamValue_Param{0}", nNo);
        }

        public void SetComponentValue()
        {
            checkButton_IsSelected.Active = false;
            entry_ParamName.Text = Name;
            entry_ParamValue.Text = Value;
        }

        public bool GetSelctedState()
        {
            return checkButton_IsSelected.Active;
        }

        public void SetEntryInterface(int nWidth, int nHeight, FontDescription pFont)
        {
            entry_ParamName.SetSizeRequest(nWidth, nHeight);
            entry_ParamValue.SetSizeRequest(nWidth, nHeight);
            GtkFactory.ChangeWidgetFont(entry_ParamName, pFont, GtkColor.Black);
            GtkFactory.ChangeWidgetFont(entry_ParamValue, pFont, GtkColor.Black);
        }

        public void SetCheckboxInterface(int nWidth, int nHeight, FontDescription pFont)
        {
            checkButton_IsSelected.SetSizeRequest(nWidth, nHeight);
            GtkFactory.ChangeContainerFont(checkButton_IsSelected, pFont, GtkColor.Black);
        }
    }
}
