# YoonCognex

- CognexFactory
    
    YoonImage의 ImageFactory에 대응하는 Library로서 각종 영상처리 Algorithm이 Wrapping 된 Static class입니다.
    YoonImage Component를 사용해서도 접근 가능하도록 기능이 개선되었습니다.
    
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