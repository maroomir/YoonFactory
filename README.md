# YoonFactory #

YoonFactory는 제조 장비용 Machine Vision Software의 기능들을 Module화 시킨 C# 기반 DLL Library 입니다.

Windows 기반의 Machine Vision Software를 쉽고 빠르게 개발해보려는 목적으로 위 Library들을 설계했습니다.

[개발자](maroomir@gmail.com)가 7년간 제조 장비 및 Robot 업체에서 개발하며 쌓은 경험과 개인적으로 공부한 지식을 토대로 만들었습니다.

해당 프로젝트는 15개의 DLL 모듈과 4개의 Sample Program으로 이뤄져있습니다.

## Feature and Desing Concept ##

YoonFactory는 쉽고, 빠르고, 일관된 방법으로 Vision Software를 제작할 수 있는 Library 입니다.

### Module Integration ###

공통된 하나의 Interface로 여러 Module들을 통합해서 실행할 수 있습니다.
```
예를 들어 IYoonFile는 YoonCSV, YoonXml, YoonIni, YoonImage 등이 상속받아서 사용하고 있기 때문에
SaveFile() 이라는 함수만으로도 위 File 형식들을 저장하는 기능을 지원할 수 있었습니다.
```

### Easy Using ###

YoonFactory를 사용하지 않을시 복잡하게 구현해야 할 기능들을 쉽고 간단하게 구현할 수 있습니다.
```
예를 들어 TCP 통신을 하고 str이라는 string을 보낸 후에 닫기 위해선 Server, Client 관계없이 다음 함수만 실행하면 됩니다.

IYoonTcpIp pTcp = new YoonServer();
pTcp.Port = "5000";
pTcp.Open();
pTcp.Send(str);
pTcp.Close();
```

### Easy Management ###

YoonFactory에 구현된 Interface와 Module을 사용하면 Parameter 관리, Log 기록 등을 비교적 간단하게 수행할 수 있습니다.

## DLL Module Introduction ##

### YoonCommon ###

YoonCommon은 Matrix, Vector, Joint 좌표 등 Vision과 Robotics에 관련된 Component와 Interface, 그리고 Enum을 구현하고 있습니다.

YoonFactory의 Base를 이루는 Interface와 Component들을 갖고있기 때문에 모든 Module과 Sample들이 기본적으로 이 DLL을 참조합니다.

- MathFactory
    기초 수학 연산식을 담은 Static class 입니다.
- YoonMatrix
    행렬 연산, 역행렬, 여인수 등 행렬 연산의 기본 기능들을 지원합니다.
    Vector 연산 또는 좌표 계산에서 사용됩니다.
- YoonVector
    각종 Vector 연산 및 8 방향에 따른 방향 Vector 산출을 지원합니다.
    Vision Module을 포함한 모든 Library에서 사용하고 있습니다.
- YoonLine
    2개의 Point를 잇는 Line을 저장하는 Component입니다.
    Cognex 결과물로 산출된 Line 위치를 저장하는데 사용합니다.
- YoonRect
    4개의 Point를 잇는 Rect를 저장하는 Component입니다.
    영상처리를 위한 영역 지정 외에 다양한 용도로 사용합니다.
- YoonJoint
    수직다관절 로봇의 Joint 좌표를 저장하는 Component입니다.

### YoonFile ###

YoonFile은 INI, CSV, XML, JSON 등의 확장자 File에 접근해서 Data를 저장하거나 불러오는 목적으로 사용합니다.

YoonFactory를 통해 Parameter 관리를 하려면 해당 DLL을 사용하면 됩니다.

YoonParameter, YoonImage, YoonComm, YoonRobot 등의 내장 DLL에서도 자체 Parameter를 관리하는 목적으로 사용하고 있습니다.

- FileFactory
    File 존재 여부 확인, Directory 확인 등 File 관련한 기능을 담은 Static class 입니다.
- YoonCSV
    CSV File에서 Text Block을 기록하거나 가져올 수 있는 기능을 지원합니다.
- YoonIni
    INI File에서 특정 Key에 들어가는 Text를 기록하거나 가져오는 기능을 지원합니다.
- YoonXml
    XML File에서 Class 내 Parameter 전체를 기록하거나 가져오는 기능을 지원합니다.
- YoonJson
    JSON File에서 Class 내 Parameter 전체를 기록하거나 가져오는 기능을 지원합니다.
    위 기능은 JSON.Net을 통해 지원됩니다.
- YoonZip
    Method 및 Recipe에 대한 Zip 파일로의 압축을 지원합니다.
	
### YoonLog ###

YoonLog는 Software 실행 중에 발생하는 각종 Log를 Console 또는 Display 상에 기록하는 목적으로 사용합니다.

- YoonConsoler
    Log 사항을 Console 창에 표기 및 기록하며 이는 YoonFactory 폴더 내에 날짜별로 저장됩니다.
- YoonDisplayer
    Log 사항을 Display 상에 표기 및 기록하며 이는 YoonFactory 폴더 내에 날짜별로 저장됩니다.

### YoonImage ###

YoonImage는 영상처리 Algorithm을 C#에서 가능한 범위 내에 구현해 낸 Library입니다.

기초적이고 단순한 영상처리만 구현했으며, 복잡하고 신속함이 요구되는 Processing은 YoonCognex 또는 YoonCV를 통해 구할 수 있도록 기능을 보완할 예정입니다.

- YoonImage
    Image가 포함된 Pointer 주소 및 Buffer 값을 C#에서 호환 가능한 형식으로 저장하며 Crop, Drawing, Histogram 추츨 등 간단한 영상처리 기술들을 지원합니다.
	이 Component는 YoonFile을 상속하고 있기 때문에 Local Directory 상에서 bmp, jpg 등의 Image를 불러와서 저장하는 작업도 가능합니다.
	YoonCamera Module을 통해 각종 Camera에서 받아온 Pointer 주소 또는 Buffer 값을 영상처리 가능하도록 변환해주는 기능을 지원합니다.
- YoonObject
    영상처리 결과로 출력되는 YoonRect, YoonLine 등의 Figure와 Label, Score 값 등을 담고있는 Component 입니다.
- ImageFactory
    Image Factory는 YoonImage 및 Raw Buffer data를 사용해서 각종 영상처리 Algorithm이 구현된 Static class 입니다.
	- `Converter를 통해 32bit Buffer를 8bit Buffer로 변환하거나 분할합니다.`
	- `PatternMatch를 통해 Source Image 상에서 Pattern을 찾아 그 결과를 출력합니다.`
	- `TwoImageProcess를 통해 2개의 Source Image를 병합하거나 뺄 수 있습니다.`
	- `Filter를 통해 Source Image를 Soften 또는 Sharpen하게 만들거나 Edge만 출력시킬 수 있습니다.`
	- `Fill을 통해 Source Image에서 특정 Point 및 Feature를 지우거나 내부를 채우게 할 수 있습니다.`
	- `ObjectDetection을 통해 Source Image에서 주어진 조건을 만족시키는 Object를 찾아낼 수 있습니다.`
	- `Binary를 통해 Source Image를 특정 Threshold 조건에 맞게 이진화시킬 수 있습니다.`
	- `Morphology를 통해 Source Image의 암부를 침식시키거나 팽창시킬 수 있습니다.`
	- `Sort를 통해 간단한 Object들을 조건에 맞게 정렬시킬 수 있습니다.`
	- `Scanner를 통해 Buffer에서 Threshold 조건에 맞는 Pixel의 시작 위치를 알아낼 수 있습니다.`
	- `PixelInspector를 통해 Source Image의 Gray Level 조건들을 분석할 수 있습니다.`
	- `Draw를 통해 C#에서 지원하는 Bitmap Component 위에 각종 Figure를 그릴 수 있습니다.`
	- `Transform을 통해 Source Image를 Resizing 하거나 변형시킬 수 있습니다.`
	
### YoonCamera ###

YoonCamera는 산업용 Camera에서 Image Buffer를 가져오려는 목적으로 사용됩니다.

현재 개인 목적으로 Test할 수 있는 장치의 한계로 Basler 및 Intel Realsense만 지원하고 있으나, 향후 Matrox, Sentech을 비롯한 다양한 Camera를 지원할 예정입니다.

- YoonBasler
    Basler Camera에서 Image의 주소값(IntPtr)을 불러올 수 있습니다.
	이 Component의 구현을 위해 Basler에서 지원하는 Pylon.Net을 사용했습니다.
- YoonRealsense
    Realsense 장치에서 Image 및 Depth Buffer의 주소값을 불러올 수 있습니다.
	이 Component의 구현을 위해 Intel에서 제공하는 Intel.Realsense Wrapper를 사용했습니다.
	
### YoonParameter ###

YoonParameter는 Class 객체로 저장되는 Parameter 값을 쉽게 저장, 불러오기, 생성 및 적층을 하기 위한 목적으로 사용합니다.

YoonFile의 함수들을 사용해서 Parameter 값을 XML로 저장하고, 각종 Parameter를 하나로 묶은 Template 정보를 INI로 저장이 가능합니다.

- YoonParameter
    IYoonParameter Interface를 상속받는 외부 Parameter Class 객체의 내부 값들을 저장하거나 불러올 수 있습니다.
- YoonContainer
    C#에서 지원하는 Collection을 활용해 다수의 YoonParameter들을 저장하거나 불러올 수 있습니다.
- YoonTemplate
    YoonContainer의 기능을 확장해서 다수의 YoonParameter들을 하나의 Template로 묶고, 이를 한번에 저장하거나 불러오는 등의 관리를 할 수 있습니다.
- CommonTemplate
    YoonParameter를 지원하지 않으나 IYoonContainer Interface를 상속받는 외부 Container들을 YoonTemplate와 유사하게 사용할 수 있습니다.

### YoonComm ###

YoonComm은 다른 Robot, 장비 또는 장치들과 통신을 하기 위한 목적으로 사용됩니다.

현재 YoonComm은 TCP/IP, RS232 통신을 지원하고 있으며, 필요시 Nuget Package를 활용해서 Modbus TCP 등의 추가도 고려중입니다.

- YoonSerial
    RS232 통신으로 Encoder, Controller 등의 장치와 string으로 Serial 통신을 수행할 수 있습니다.
- YoonServer
    TCP/IP 연결에서 Server 기능을 수행할 때 사용하며, Port 번호를 지정하면 해당 Port를 열고 상대 Client의 연결을 기다립니다.
	자체적인 Retry Thread를 갖고 있어서 Listen이 이뤄지지 않을 시 자동으로 다시 Listen을 시킵니다.
- YoonClient
    TCP/IP 연결에서 Client 기능을 수행할 때 사용하며, Server의 IP와 Port 번호를 지정하면 Client 접속할 때 까지 대기합니다.
	자체적인 Retry Thread를 갖고 있어서 Server와의 연결이 이뤄지지 않을 시 자동으로 다시 Connect를 시도합니다.

### YoonRobot ###

YoonRobot은 협동로봇을 원격 제어를 하기 위한 목적으로 제작되었으며, YoonComm의 TCP 연결 기능을 사용한 확장 Library입니다.

YoonRobot을 통해 UR(Universial Robotics) 제품과 TM(Techman) 제품을 원격 제어할 수 있습니다.

### YoonCognex ###

YoonCognex는 Cognex VisionPro Library로 전문적인 Machine Vision 영상처리를 수행하고자 할 때 사용합니다.

YoonCognex를 정상적으로 사용하기 위해선 Cognex VisionPro를 사용하기 위한 Dongle Key와 VisionPro 9.6의 설치가 필요합니다.

해당 Dongle Key의 구입 가능여부 및 가격은 Cognex 대리점에 문의바랍니다.

- CognexFactory
    YoonImage의 ImageFactory에 대응하는 Library로서 각종 영상처리 Algorithm이 Wrapping 된 Static class입니다.
	YoonImage Component를 사용해서도 접근 가능하도록 기능 개선 예정입니다.
	- `Converter를 통해 ICogImage를 상속하는 Cognex 전용 Image 객체를 변환시키거나 생성할 수 있습니다.`
	- `PatternMatch를 통해 Cognex의 장점인 PatMax를 사용한 Pattern Matching을 수행할 수 있습니다.`
	- `Editor를 통해 검사 조건에 맞게 ICogImage 객체를 변경할 수 있습니다.`
	- `Draw를 통해 CogDisplay 객체에 검사 영역을 그릴 수 있습니다.`
	- `Transform을 통해 ICogImage 객체를 Crop하거나 Resizing할 수 있습니다.`
	- `TwoImageProcess를 통해 두 개의 CogImage를 합치거나 차영상을 구할 수 있습니다.`
- ToolFactory
    Cognex 고유 기능인 ICogTool을 사용해서 쉽고 간편하게 영상처리 작업을 구현할 수 있는 Static class 입니다.
- CognexMapping
    Cognex Library를 사용한 좌표 Calibration을 지원하는 Module 입니다.

### YoonCV ###

YoonCV는 영상처리용 Open Source인 OpenCV를 Wrapping한 OpenCVSharp Package를 사용해서 전문척인 영상처리를 수행할 목적으로 제작되고 있습니다.

현재 기능 개발을 위한 준비 중입니다.

### YoonAlign ###

YoonAlign은 Machine Vision의 대표적인 용도 중 하나인 Vision Align 기능을 지원하는 Library입니다.

YoonCalibration과 연동해서 Mapping 된 중심점과 좌표값을 사용해 Align을 합니다.

다만 YoonAlign Module의 대부분의 기능들을 YoonVector로 흡수시킬 계획이며, 조만간 삭제될 예정입니다.

### YoonCalibration ###

YoonCalibration은 단일의 또는 다수의 Camera들을 사용해 Vision 좌표계와 실제 좌표계를 일치시키기 위해 사용합니다.

- YoonRotationCalib
    영상을 취득하는 Camera 또는 대상체가 놓여진 Stage 등이 회전이 가능할 경우 사용하는 Calibration 방법입니다.
- YoonStaticCalib
    영상을 취득하는 Camera 또는 대상체가 고정된 상태에서 Feature Point들과 실측값으로 Calibration 하는 방법입니다. 

### YoonViewer ###

YoonViewer는 WinForm 기반에서 사용 가능한 Image Viewer 전용 Component입니다.

YoonImage와의 연동을 위해 추가 개발을 진행할 예정입니다.

### YoonWindows ###

YoonWindows는 WinForm 또는 WPF에서 사용하는 Component들을 지원하는 Library 입니다.

현재는 DatatableFactory만 존재하며 향후 개발 소요가 있을 경우 계속 추가해 나갈 계획입니다.

### YoonMono ###

YoonMono는 Mono 기반에서 사용하는 Component들을 지원하는 Library 입니다.

YoonFactory Library의 Multi-Platform 지원과 GtkSharp Component의 기능을 보완하는 목적으로 사용합니다.

- GtkFactory
    Gtk Component의 기능을 보완하는 Static class 입니다.
- ColorFactory
    Gtk.Color 객체와 System.Drawing 객체로 상속받는 색깔값을 Matching 시킴으로서 YoonLog와 같은 Library를 Mono 환경에서도 사용할 수 있게 해줍니다.
- FontFactory
    Gtk Component의 불편한 Font 설정 기능을 개선하는 Static class 입니다.
