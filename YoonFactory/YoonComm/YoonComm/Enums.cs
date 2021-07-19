namespace YoonFactory
{
    public enum eYoonCommType : int
    {
        None,
        RS232,
        RS422,
        TCPClient,
        TCPServer,
    }

    public enum eYoonBufferMode : int
    {
        String,
        ByteArray,
    }
}
