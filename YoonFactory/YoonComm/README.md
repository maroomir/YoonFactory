# YoonComm

- YoonSerial
    
    RS232 통신으로 Encoder, Controller 등의 장치와 string으로 Serial 통신을 수행할 수 있습니다.
    
- YoonServer
    
    TCP/IP 연결에서 Server 기능을 수행할 때 사용하며, Port 번호를 지정하면 해당 Port를 열고 상대 Client의 연결을 기다립니다.
    자체적인 Retry Thread를 갖고 있어서 Listen이 이뤄지지 않을 시 자동으로 다시 Listen을 시킵니다.
    
- YoonClient
    
    TCP/IP 연결에서 Client 기능을 수행할 때 사용하며, Server의 IP와 Port 번호를 지정하면 Client 접속할 때 까지 대기합니다.
    자체적인 Retry Thread를 갖고 있어서 Server와의 연결이 이뤄지지 않을 시 자동으로 다시 Connect를 시도합니다.