# YoonImage

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