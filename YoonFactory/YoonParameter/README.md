# YoonParameter

- YoonParameter
    
    IYoonParameter Interface를 상속받는 외부 Parameter Class 객체의 내부 값들을 저장하거나 불러올 수 있습니다.
    
- YoonContainer
    
    C#에서 지원하는 Collection을 활용해 다수의 YoonParameter들을 저장하거나 불러올 수 있습니다.
    
- YoonTemplate
    
    YoonContainer의 기능을 확장해서 다수의 YoonParameter들을 하나의 Template로 묶고, 이를 한번에 저장하거나 불러오는 등의 관리를 할 수 있습니다.
    
- CommonTemplate
    
    YoonParameter를 지원하지 않으나 IYoonContainer Interface를 상속받는 외부 Container들을 YoonTemplate와 유사하게 사용할 수 있습니다.