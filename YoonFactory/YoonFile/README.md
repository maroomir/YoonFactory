# YoonFile

YoonFile은 ini, csv, xml, json 확장자 File에 접근해서 Data를 저장하거나 불러오기 위한 Interface와 Class들을 구현하고 있습니다. YoonFactory 내에서 Data 관리를 담당하는 Module이기 때문에 Socket 통신이나 Robot 제어같은 고차원적인 Library에서도 사용되고 있습니다.

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