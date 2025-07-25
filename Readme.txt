# 개요

인프런 (InfLearn) 에서 루키스(Rookiss)님 유니티 뱀파이어 서바이벌 장르 모작 강의의 학습 내역입니다.

게임을 만드는 데에 있어서의 상속 구조 활용과 데이터 파싱, UI 연동 등을 학습 할 수 있었습니다.


# 상세 설명

강의 자체는 약 1개월에 걸쳐 수료하였으며 코드를 최대한 이해하고 습득하기 위해 단순히 코드를 따라치는게 아닌

코드를 분석하고 분석한 내용을 주석으로 정리하며 학습을 진행하였습니다.

또한, 강의 완료 후에 완성에 가까운 코드를 분석하기보다는 토대를 이해하고 활용하여 직접 쌓아올리는 방식을 채택하였고

기존에 강의의 진행을 위해 방치된 버그들을 직접 수정하였습니다.

제공된 UI 리소스들을 활용하여 이를 배치하고 원활히 작동할 수 있게끔 작업하였으며

코드 내의 확장 편의성과 개발 지속성을 위해 여러 작업들을 수행했습니다.

게임은 Title - Lobby - Game 의 3가지 씬으로 구성되어 동작합니다.

타이틀에서 게임에 필요한 리소스를 사전에 로드하여 게임 내에서 실시간으로 로드에 부과되는 시스템적 부하를 방지합니다.

로딩이 끝나면 클릭(터치)를 통해 로비로 이동이 가능합니다.

UI는 스택으로써 관리됩니다

로비는 크게 3종류로 구분할 수 있으며 각각은 다음과 같습니다.

1. 스테이지 선택

스테이지는 현재 5스테이지까지 구현된 상태이며 스테이지 입장 전 스테이지의 등장 몬스터와 클리어 여부 그리고 진행도를 확인 가능합니다.

스테이지들은 스크롤 오브젝트로 등록되어 선택이 가능하며

원하는 스테이지를 선택하고 게임 시작 버튼을 누르면 게임 씬으로 이동합니다.

2. 장비

3. 상점

게임은 뱀파이어 서바이벌의 플레이 방식을 채용하여 구현하였으며 동작 방식은 다음과 같습니다.

1. 시점은 2D 탑뷰 방식이며 플레이어는 상하좌우로 이동할 수 있습니다.

2. 플레이어와 어느정도 거리가 떨어진 곳에서 몬스터가 짧은 간격을 두고 생성됩니다.

3. 몬스터는 죽으면서 경험치 오브젝트를 필드에 남기며 플레이어는 해당 오브젝트에 접근시 경험치 오브젝트를 획득합니다.

4. 경험치를 레벨별 요구량 이상을 획득 시 레벨업을 하며 플레이어는 3가지 스킬중에 하나를 선택합니다.

5. 스킬은 최대 6종류만이 획득 가능하며 이미 가지고 있는 스킬의 경우 스킬 레벨을 최대 6까지 상승시킵니다.

6. 스킬을 이미 6종류 획득하였다면 레벨 업 시 이미 획득한 스킬만 등장합니다.

7. 몬스터는 정해진 수량만큼만 등장하며 마지막 몬스터는 특수한 스킬을 사용하는 보스 몬스터가 등장합니다.

8. 보스 몬스터를 처치할 시 스테이지는 승리로써 종료되며 로비 씬으로 돌아갑니다.

9. 플레이어의 체력이 0이 되거나 스테이지의 제한 시간을 초과하면 플레이어는 패배하며 로비 씬으로 돌아갑니다.