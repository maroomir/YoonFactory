# YoonFactory

YoonFactory는 제조 장비용 Machine vision software의 기능들을 Module화 시킨 C# 기반 DLL Library 입니다. Windows 기반의 Machine vision software를 쉽고 빠르게 개발해보려는 목적으로 위 Library들을 설계했습니다. [개발자](notion://www.notion.so/maroomir@gmail.com)가 7년간 제조 장비 및 Robot 업체에서 개발하며 쌓은 경험과 개인적으로 공부한 지식을 토대로 만들었습니다.

해당 프로젝트는 15개의 DLL Module과 4개의 Sample Program으로 이뤄져있습니다.

## Repackage Library

위 YoonFactory는 현재 산업용 현장에서 많이 쓰이는 .NetFramework 4.7.2의 C# 언어로 작성되었으며, Windows 환경에 최적화 되었습니다. 다만 시대가 바뀌면서 Machine Vision 외에 다양한 용도로 사용해야할 필요성을 느끼면서, 각 기능들을 Python, Java 및 새로운 .Net 환경에 최적화시키는 Repackage Project를 수행하고 있습니다.

각 Repackage library에 대한 자세한 설명은 하단을 참조하시기 바랍니다.

### [YoonFactory.Net](https://github.com/maroomir/YoonFactory.Net)

.Net 5.0 이상을 지원하는 Package입니다. Xamarin 및 향후 MAUI 출시시 Linux, Mac, Windows 동시 지원성을 강화했으며, Net Framework 4.X 대에서 지원이 끊겨진 Cognex의 핵심 기능들을 대체할 수 있도록 OpenCVSharp과의 연동성을 강화시킬 예정입니다.

### [YoonFactory.Java](https://github.com/maroomir/YoonFactory.Java)

최신 사양의 Java(OpenJDK, 15 이상)를 지원하는 Package입니다. Java가 갖고 있는 가장 큰 장점인 범용성과 수많은 Open source library를 적극적으로 활용해서, 어느 환경에서든 실행시킬 수 있는 Package를 만드는 것이 목표입니다.

### [YoonFactory.Droid](https://github.com/maroomir/YoonFactory.Droid)

Java 언어와 Google API를 사용해서 Android에 최적화되어진 Package 입니다. 향후 Android 개발을 본격적으로 할 때까지는 Private 상태로 둘 예정입니다.

### [YoonPytory](https://github.com/maroomir/YoonPytory)

YoonPytory는 제가 대학원에서 Deep-Learning을 공부하며 얻은 Insight를 정리한 Package입니다. 부모인 YoonFactory와 전혀 다른 목적으로 설계되었지만, 여기서 얻은 정보와 지식들을 YoonFactory와 Repackage Library에 횡전개할 예정입니다.

## Feature and Desing Concept

YoonFactory는 쉽고, 빠르고, 일관된 방법으로 각 기능들에 접근할 수 있습니다. Python Package를 사용하며 얻은 경험을 토대로 생각한 기능들을 쉽고 확실하게 사용을 할 수 있게 만들고자 노력했습니다.

### Module Integration

공통된 하나의 Interface를 사용해서 여러 Module들을 통합해서 실행할 수 있습니다. 예를 들어 YoonCSV, YoonXml, YoonIni, YoonImage가 IYoonFile을 상속받아서 사용하고 있기 때문에 SaveFile() 이라는 함수만으로도 csv, xml, ini, bmp, jpg 형식을 저장하는 기능을 지원할 수 있습니다.

> 다른 기능들과 달리 bmp와 jpeg를 관리하는 YoonImage는 YoonImage library 내에 있습니다.
> 

```csharp
IYoonFile pFile = new YoonXml("sample.xml");
pFile.LoadFile(out object pParam, typeof(List<string>));
pFile.SaveFile(pParam, typeof(List<string>));
```

### Easy Using

마치 Python Package처럼 YoonFactory를 통해 복잡하게 구현해야 할 기능들을 쉽고 간단하게 구현할 수 있습니다. 예를 들어 TCP 통신을 하면서 Message를 보낸 후에 닫기 위해선 아래처럼 간단하게 함수를 실행하면 됩니다. 만약 TCP 연결이 끊긴다면 Module 내에서 자동으로 Retry Thread를 생성해서 실행합니다.

> Client의 경우 Server에 대한 Address가 추가로 필요합니다.
> 

```csharp
IYoonTcpIp pTcp = new YoonServer();
pTcp.Port = "5000";
pTcp.Open();
pTcp.Send("Send Message");
pTcp.Close();
```

### Easy Management

YoonFactory에 구현된 Interface와 Module을 사용해서 Parameter 관리, Log 기록 등을 비교적 간단하게 수행할 수 있습니다. 예를 들어 Software가 실행되는 위치에서 90일간의 기록을 저장할 수 있는  Log 관리자는 아래처럼 쉽게 만들 수 있습니다.

```csharp
YoonConsoler pCLM = new YoonConsoler(90);
pCLM.Write("Generate Log Manager");
```

## DLL Module Introduction

### [YoonCommon](/YoonFactory/YoonCommon/README.md)

YoonCommon은 Matrix, Vector, Joint 좌표 등 Vision과 Robotics에 관련된 Component와 Interface, 그리고 Enum을 구현하고 있습니다. YoonFactory의 Base를 이루는 Interface와 Component들을 갖고있기 때문에 모든 Module과 Sample들이 기본적으로 이 DLL을 참조합니다.


### [YoonFile](/YoonFactory/YoonFile/README.md)

YoonFile은 ini, csv, xml, json 확장자 File에 접근해서 Data를 저장하거나 불러오기 위한 Interface와 Class들을 구현하고 있습니다. YoonFactory 내에서 Data 관리를 담당하는 Module이기 때문에 Socket 통신이나 Robot 제어같은 고차원적인 Library에서도 사용되고 있습니다.


### [YoonLog](/YoonFactory/YoonLog/README.md)

YoonLog는 Software 실행 중에 발생할 수 있는 각종 Log를 Tracing해서 Console 또는 Display 상에 표시함과 동시에 저장하는 목적으로 사용합니다. 관리 가능한 Local 저장소에 기록하기 때문에 Software의 동작 상태를 외부에서도 관측할 수 있도록 txt 확장자로 남길 때 유용합니다.


### [YoonImage](/YoonFactory/YoonImage/README.md)

YoonImage는 Machine vision에서 주로 사용되는 영상처리 Algorithm을 C# 환경에서 구현한 Library 입니다. 기초적이고 단순한 영상처리만 구현했으며, 복잡하고 신속함이 요구되는 Processing은 YoonCognex 또는 YoonCV를 통해 사용할 수 있습니다.


### [YoonCamera](/YoonFactory/YoonCamera/README.md)

YoonCamera는 산업용 Camera에서 Image Buffer를 가져올 때 쉽게 사용할 수 있도록 구성되었습니다. 현재는 개인적으로 구입한 장치인 Basler와 Intel Realsense만 지원하고 있으나, 향후 Sentech을 비롯한 다양한 Camera를 지원할 예정입니다.


### [YoonParameter](/YoonFactory/YoonParameter/README.md)

YoonParameter는 Class 객체로 저장되는 Parameter 값을 쉽게 저장, 불러오기, 생성 및 적재를 하기 위한 목적으로 사용합니다. IYoonParameter를 상속하는 Class라면 Parameter 값을 xml로 저장하기 및 불러오기, 그리고 각종 Parameter를 하나로 묶어 Template으로 만드는 작업이 간편해집니다.


### [YoonComm](/YoonFactory/YoonComm/README.md)

YoonComm은 다른 Robot 및 장비들과 통신을 하기 위한 목적으로 사용됩니다. 현재는 TCP/IP, RS232 통신을 지원하고 있으며, 필요시 외부 Package를 활용해서 Modbus TCP 등을 추가할 예정입니다.


### YoonRobot

YoonRobot은 협동로봇을 원격으로 제어할 목적으로 제작되었으며, YoonComm의 TCP/IP 연결 기능을 협동로봇에 맞게 Wrapping한 Module 입니다. 현재는 Universial Robotics 제품과 Techman 제품을 원격 제어할 수 있습니다.

### [YoonCognex](/YoonFactory/YoonCognex/README.md)

YoonCognex는 미국 Cognex 사의 Machine vision 전문 Library인 VisionPro로 더욱 전문적인 산업용 영상처리를 수행할 목적으로 제작했습니다. YoonCognex를 정상적으로 사용하기 위해선 Cognex VisionPro를 사용하기 위한 Dongle key와 VisionPro 9.6의 설치가 필요합니다. 해당 Dongle의 구입 가능여부 및 가격은 Cognex 대리점에 문의바랍니다. Module 및 성능에 대한 설명은 [Cognex](https://www.cognex.com/) 홈페이지를 참고해주세요.


### YoonCV

YoonCV는 영상처리용 Open source인 OpenCV를 C#에 맞게 Wrapping한 OpenCVSharp Package를 사용해서 전문척인 영상처리를 수행할 목적으로 제작했습니다. 다만 YoonCognex가 있는만큼 YoonFactory library에서는 기초적인 Interface만 개발했으며, 향후 YoonFactory.Net에서 Build-up한 관련 source들을 여기로 다시 횡전개할 예정입니다. 

### YoonAlign

YoonAlign은 Machine vision의 주로 쓰이는 용도 중 하나인 Align 기능을 지원하는 Library입니다. YoonCalibration과 연동해서 Mapping 된 중심점과 좌표값을 사용해 Object로 인식된 Point를 Target 위치로 Align 합니다. 다만 현재 YoonAlign은 Legacy를 위해 남겨놓은 것이며, Repackage line-up에서는 대부분의 기능들을 YoonVector 또는 YoonImage로 흡수할 예정입니다.

### [YoonCalibration](/YoonFactory/YoonCalibration/README.md)

YoonCalibration은 단일 또는 다수의 Camera를 사용해 Vision 좌표계와 실제 좌표계를 일치시키기 위해 사용합니다.


### YoonViewer

YoonViewer는 WinForm 기반에서 사용 가능한 Image Viewer 전용 Component입니다. YoonImage를 실제 화면상에 보여주기 위한 Interface들이 작업되어있습니다.

### YoonWindows

YoonWindows는 WinForm 또는 WPF에서 사용하는 Component들을 지원하는 Library 입니다. 향후 개발 소요가 있을 경우 계속 추가해 나갈 예정이나, 향후 Linux와 MacOS로 완전 이주를 할 예정이므로 WinForm과 WPF에 대한 수요가 있을지 모르겠습니다.

### [YoonMono](/YoonFactory/YoonMono/README.md)

YoonMono는 Mono 기반에서 사용하는 Component들을 지원하는 Library 입니다. YoonFactory의 Multi-Platform 지원과 GtkSharp Component의 기능을 보완하는 목적으로 사용합니다. 기존 .Net Framework가 지원하는 System namespace과 연동성이 적은 Mono Framework를 최대한 쉽게 사용할 수 있도록 보완한 겁니다. 다만 Mono 자체가 .Net 5.0 전환 이후 사장되고 있으므로 결국 .Net Framework 4에서의 Legacy source를 지원하는 정도로만 사용할 예정입니다.
