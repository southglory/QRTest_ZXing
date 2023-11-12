# QRTest

This is QR Scanner for Unity using ZXing.Net Nuget package.
Come and Follow my Blog!

https://blog.naver.com/devramyun/223262882409

추가 업데이트!

1. 웹캠 영상이 올바른 방향대로 회전되어 화면에 출력될 것.
2. 출력되는 화면이 Canvas를 벗어나지 않도록 축소할 것.

기록

https://blog.naver.com/devramyun/223262970104
https://gist.github.com/southglory/35cf2a94cbe65e15c408b6af0e126e3a
https://youtu.be/ygrIvhZBbIA
https://youtube.com/shorts/UwV-2bBGiyw?feature=share

추가 없데이트!

1. 웹캠 촬영 시작과 중단을 제어하는 메서드 추가.
2. 'LiveQRScanner' 버튼 클릭 이벤트 처리.
3. 앱의 생명주기 이벤트 처리.
4. QR 코드 인식 후 촬영 중단.
5. 앱이 중단, 포커스 없음, 종료, '뒤로가기 버튼 클릭' 시 웹챔 촬영이 중단되도록 이벤트 처리.

추가 업데이트!

1. QR 코드를 스캔하고 결과를 UI 패널에 표시하며, 스캔된 내용이 유효한 URL인 경우에만 버튼을 통해 URL을 열 수 있도록 함.
2. 웹캠 촬영 재시작 로직 'RestartScanning' 메서드 추가
3. openUrlButton의 클릭 이벤트 리스너를 초기화

기록

https://youtu.be/xE1BYI99Nt4

추가 업데이트!

1. 다양한 상황에 맞는 텍스트 출력  
- URL이 아닌 경우: "QR코드가 올바르지 않습니다. 다시 시도해주세요" 메시지를 표시합니다.  
- URL이면서 "dhlottery.co.kr"을 포함하는 경우: "당첨번호 확인을 위해 동행복권 공식 웹사이트로 이동합니다." 메시지를 표시합니다.  
- URL이지만 "dhlottery.co.kr"을 포함하지 않는 경우: "(주의!) 동행복권(dhlottery) 웹사이트 주소가 아닙니다." 메시지를 빨간색 글자로 표시합니다.  

기록

https://youtu.be/w-QzMBF8_z0

