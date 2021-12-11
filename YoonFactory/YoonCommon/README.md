# YoonCommon

YoonCommon은 Matrix, Vector, Joint 좌표 등 Vision과 Robotics에 관련된 Component와 Interface, 그리고 Enum들을 구현하고 있습니다. YoonFactory의 Base를 이루는 Interface와 Component들을 갖고있기 때문에 모든 Module과 Sample들이 기본적으로 이 Module을 참조합니다.

사용하는 namespace는 YoonFactory이며, 위 기능을 사용하기 위해선 Header 부에 다음처럼 선언해야합니다.

```csharp
using YoonFactory;
```

### MathFactory

기초 수학 연산식을 담은 Class 입니다. Static이기 때문에 namespace를 선언해두면 MathFactory라는 접두어로 어디서든 접근할 수 있습니다.

```csharp
// In-line
dScore = MathFactory.GetCorrelationCoefficient(pPatternBuffer, pTempBuffer, nPatternWidth, nPatternHeight);
```

- GetCorrelationCoefficient

Source 및 Object의 유사도를 구하는 함수입니다. Pattern matching처럼 두 Buffer 간의 유사도를 구하기 위해 사용합니다.

```csharp
// YoonFactory/YoonImage/YoonImage/ImageFactory.cs
// Find the coefficient
double dCoefficient = MathFactory.GetCorrelationCoefficient(pPatternBuffer, pInspectBuffer, nPatternWidth, nPatternHeight);
```

- LeastSquare

여러 Point들을 지나는 직선의 공식을 구하는 최소 자승법입니다. Line matching처럼 Matching 쌍들에서 최적의 직선을 찾아서 표현할 때 주로 사용합니다.

```csharp
// YoonFactory/YoonCommon/YoonLine.cs
if (!MathFactory.LeastSquare(ref dSlope, ref dConstant, pList.Count, pX, pY))
    return;
```

- Lagrange

N개의 Point를 만족시키는 N-1차 함수를 구하기 위해 사용하는 Lagrange 승수법입니다. 여러 점의 Calibration point를 만족하는 함수를 구할 때 주로 사용합니다.

- Get3PointToCircle

3점을 지나는 원의 방정식 구하는 함수입니다.

- SortInteger

int 형식으로 이루어진 List를 Increase(오름차순), 또는 Decrease(내림차순) 방향으로 정렬하는 함수입니다.

- SortVector

YoonVector로 이루어진 List를 8가지 방향(Top, Top-Right, Right, Bottom-Right, Bottom, Bottom-Left, Left, Top-Left)을 기준으로 정렬하는 함수입니다. 영상처리 과정을 통해 검출된 여러 Vector들을 정렬해야할 때 사용합니다.

```csharp
// YoonFactory/YoonCommon/YoonLine.cs
List<YoonVector2N> pListSorted = new List<YoonVector2N>(pList);
MathFactory.SortVector(ref pListSorted, nDirArrange);
```

- SortRectangle

YoonRect로 이루어진 List를 특정 방향(Top-Left, Top-Right, Left, Right)을 기준으로 정렬하는 함수입니다.

- Combine

    $nCr$

순열 조합 함수입니다. n은 number, r은 root란 이름으로 들어갑니다.

- GetHistogramPeak

Histogram 상에서 최고 지점을 선택해주는 함수입니다. 영상처리가 전후에 조건을 만족시키는 Gray Level를 선출하기 위해 사용됩니다.

### YoonMatrix

행렬 연산, 역행렬, 여인수 등 행렬 연산의 기본 기능들을 지원합니다.
Vector 연산 또는 좌표 계산에서 사용됩니다.

### YoonVector

각종 Vector 연산 및 8 방향에 따른 방향 Vector 산출을 지원합니다.
Vision Module을 포함한 모든 Library에서 사용하고 있습니다.

### YoonLine

2개의 Point를 잇는 Line을 저장하는 Component입니다.
Cognex 결과물로 산출된 Line 위치를 저장하는데 사용합니다.

### YoonRect

4개의 Point를 잇는 Rect를 저장하는 Component입니다.
영상처리를 위한 영역 지정 외에 다양한 용도로 사용합니다.

### YoonJoint

수직다관절 로봇의 Joint 좌표를 저장하는 Component입니다.