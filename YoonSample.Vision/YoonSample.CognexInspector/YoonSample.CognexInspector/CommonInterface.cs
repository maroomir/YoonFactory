using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoonFactory;

namespace YoonSample.CognexInspector
{
    public interface IInspectionTab : ICogToolTab
    {
        void OnInspectionParameterDownload(object sender, EventArgs e); // Common -> Tab
        void OnInspectionParameterUpdate(object sender, EventArgs e); // Tab -> Common
    }

    public interface ICogToolTab
    {
        void OnCognexToolUpdate(object sender, CogToolArgs e);  // Tab -> Common
        void OnCognexToolDownload(object sender, CogToolArgs e);    // Common -> Tab
        void OnCognexImageUpdate(object sender, CogImageArgs e);   // Tab -> Common
        void OnCognexImageDownload(object sender, CogImageArgs e);    // Common -> Tab

        event PassImageCallback OnUpdateResultImageEvent;
    }
    public interface IParameterInspection : IYoonParameter
    {
        bool IsUse { get; set; }
    }
}
