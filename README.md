# GoldMine

![](https://github.com/elrha/GoldMine/blob/master/doc/poster.jpg?raw=true)

##게임 설명

금광에 펼쳐진 자원을 단단한 블럭을 피하여 최대한 빠르게 수집하는 알고리즘을 구현 해 주세요.
가볍게는 최단거리 알고리즘, 깊게는, 상대방의 움직임 예측을 통한 심리전이 포함되는 고급진 구현을 요하는 퍼즐이라 보셔도 좋습니다.

_본 대회는 반복적인 비즈니스 로직 작성에서 벗어나, 잠들어 가고있는 뉴런의 시냅스에 기분좋은 전기 자극을 주어 A10 신경 활성화, 베타엔돌핀 분비를 촉진 시켜 기억력 향상 및 의욕 향상, 궁극적으로는 많은 직원들로부터 더 나은 양질의 코드 작성을 이끌어 내 회사에 긍정적인 작용을 하도록 하는것에 취지가 있습니다. 단순한 오락거리로 오해하지 않으셨으면 합니다._

![](https://github.com/elrha/GoldMine/blob/master/doc/example-1.png?raw=true)

![](https://github.com/elrha/GoldMine/blob/master/doc/example-2.png?raw=true)

상단에서 보이는것과 같이, 맵 상에 존재하는 자원을 상대방보다 먼저 수집하여 높은 점수를 취득하는 참가자가 우승을 하는 방식의 게임 입니다.

게임 중, 무작위로 드랍되는 아이템을 취득하여, 게임을 보다 유리하게 이끌어 갈 수 있습니다.

##게임 룰

####자원 수집

-자원 아이템으로 다이아몬드, 금, 은, 동이 존재하며 각각 수집시 5, 3, 2, 1 point를 취득하게 됩니다.
-같은 자원을 동시에 수집 할 경우, 동시 수집자 모두가 point를 취득 합니다.
-맵에서 모든 자원이 수집 될 때 게임이 종료 됩니다.

####장애물

-1 ~ 6 내구성을 가지는 돌들이 맵에 펼쳐집니다.
-한 턴에 본인의 power 만큼 돌의 내구성을 깎을 수 있고, 돌의 내구도가 0 이하가 되면 제거 됩니다.
-power는 아이템을 통하여 증가 시킬 수 있습니다.

####아이템

*아이템은 3 level로 구성 되어 있습니다.
(파랑색 LV3, 녹색 LV2, 회색 LV1)

-곡갱이 : 사용자의 power를 증가 시켜 줍니다. LV3는 3, LV2는 2, LV1은 1의 power를 증가 시킵니다. 증가된 power는 해당 판에서 영구적으로 유지 됩니다. (새로 시작시 초기화)
-시간정지 : 본인을 제외한 다른 유저들을 행동불능 상태에 빠지게 합니다.
LV3 : sqrt((맵의 가로 블럭갯수^2) + (맵의 세로 블럭 갯수^2)) 입니다. (대각선 블럭 갯수 만큼의 턴)
LV2 : sqrt((맵의 가로 블럭갯수^2) + (맵의 세로 블럭 갯수^2)) 입니다. (대각선 블럭 갯수 만큼의 턴) * (2/3)
LV1 : sqrt((맵의 가로 블럭갯수^2) + (맵의 세로 블럭 갯수^2)) 입니다. (대각선 블럭 갯수 만큼의 턴) * (1/3)
-산사태 : 본인을 제외한 다른 유저들의 주변에 산사태가 일어납니다. LV3은 주변 5칸반경, LV2는 주변 3칸 반경, LV1은 주변 2칸 반경으로 돌이 무너집니다.


##대회 일정

##### 일시 : 2016-02-05 18:00 (회사 일정에 따른 변경이 발생 할 수 있습니다)
##### 장소 : N3N 사무실
##### 우승 조건 : 총 3판의 게임 후, Score의 누적 총 합이 가장 많은 참가자.
##### 우승 상품 : 추후 재공지 드리겠습니다.
##### 대회 방식 : 4인전  토너먼트 (참여 인원에 따른 세부 사항 변경이 있을 수 있습니다.)

*대회 전 2016-01-29일 사전 테스트가 있을 예정입니다. 본인이 작성한 로직 랭크를 대회 전 체크할수있는 좋은 기회이니, 많은 참여 바랍니다.
*2016-01-29일 까지는 치명적인 버그나 밸런스 조정을 위한 소스 재배포가 있을 수 있습니다.
*참여 인원 상황에 따라, 중급 난이도의 봇이 참가 될 수 있습니다.




##참여 방법

각 언어 사용자별로 가이드를 참고 하시기 바랍니다.
GoldMine Player는 C++, C#, JavaScript 3가지 언어로 구현 가능합니다.

####모든 언어 공통

본인이 작성 하시던 결과물을 수행 하시고 싶으시면 아래와 같이 처음 화면에서,

![](https://github.com/elrha/GoldMine/blob/master/doc/example-3.png?raw=true)
본인의 DLL 혹은 JS를 선택 해 주시고, 최 하단의 Start Game을 클릭 해 주시면 됩니다.

JS의 경우는, 파일 수정 / 저장 후 Start Game을 다시 누르시면 새로 저장된 파일을 재 로드하여 실행 하도록 되어 있으니, 굳이 프로그램을 종료후 재시작 하지 않으셔도 됩니다.

가능하면 결과물은, 본인영문이름.dll 형태로 바꾸어서 보내주세요.

####Native (C++)

![](https://github.com/elrha/GoldMine/blob/master/doc/example-4.png?raw=true)

솔루션을 여신 후, 빨간 동그라미가 쳐진 Template_Native 프로젝트를 통하여 dll를 작성 해 주시면 됩니다.

만일 빌드시 문제가 있으실 경우 프로젝트 우측버튼 클릭 -> Properties 클릭 후

![](https://github.com/elrha/GoldMine/blob/master/doc/example-5.png?raw=true)
본인 개발 환경에서 지원하는 Platform Toolset으로 변경 후 작업 해주시면 됩니다.

디버깅 방법은, Template_Native 프로젝트 마우스 우클릭 -> Set as Startup Project 클릭, F5를 누르시면 정상적으로 디버깅이 가능 해 집니다.


![](https://github.com/elrha/GoldMine/blob/master/doc/example-6.png?raw=true)
구현을 완료 하시면, 출력 폴더의 Template_Native.dll 파일을 저에게 메일로 보내 주시면 됩니다.
(출력물 메일 보내 주실때 사용하신 toolset version 함께 알려주세요)


####C#

![](https://github.com/elrha/GoldMine/blob/master/doc/example-7.png?raw=true)
솔루션을 여신 후, Template_DotNet 프로젝트를 구현 하시면 됩니다.

![](https://github.com/elrha/GoldMine/blob/master/doc/example-8.png?raw=true)
구현이 완료되면, 출력 폴더의 Template_DotNet.dll을 복사 하시어 저에게 보내 주시면 됩니다.


####JavaScript

Player 폴더 내의 Template_JS.js 파일을 열어 구현 해 주시면 됩니다.
JS의 경우는 Debugger Attach를 통한 BreakePoint 제어가 좀 까다롭습니다.
공교롭게도 디버깅 환경을 log를 통한 환경밖에 제공하지 못하게 되었습니다.

![](https://github.com/elrha/GoldMine/blob/master/doc/example-9.png?raw=true)
윗 화면에서 보이듯, Console.Info(stringstring) 명령으로 Log폴더 내에 로깅을 남길 수 있습니다.

JS 경우는, 작성하신 Template_JS.js를 그대로 보내 주시면 됩니다.

####Block Code

전 코드 공통으로, Map Field 값의 의미는 아래와 같습니다.

ITEM_9 = -1009
ITEM_8 = -1008
ITEM_7 = -1007
ITEM_6 = -1006
ITEM_5 = -1005
ITEM_4 = -1004
ITEM_3 = -1003
ITEM_2 = -1002
ITEM_1 = -1001
GEM_5 = -5
GEM_3 = -3
GEM_2 = -2
GEM_1 = -1
NONE = 0
ROCK_1 = 1
ROCK_2 = 2
ROCK_3 = 3
ROCK_4 = 4
ROCK_5 = 5
ROCK_6 = 6

####Return Code From Player

Process Function이 Return해야 하는 값은 아래와 같습니다.

0 : 상, 1 : 우, 2 : 하, 3 : 좌

*이동 대상 블럭이 돌 블럭일 경우, 돌의 내구도를 본인의 power만큼 상쇄


**ps. 본 대회는, 회사 업무와는 무관한 자율적인 컨텐츠 입니다. 회사 업무시간에 지장을 주지 않는 선에서 시간 할애를 해주시길 부탁드립니다.**
