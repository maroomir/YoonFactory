namespace YoonFactory.Log
{
    public interface IYoonLog
    {
        string RootDirectory { get; set; }
        LogSection Repository { get; }

        event LogProcessCallback OnProcessLogEvent;
        void Write(string strMessage, bool isSave);
        void Clear();
    }

}