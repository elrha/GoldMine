var test = 0;

// note : JS file 로드시 초기에 한번 호출 됩니다. 본인의 이름 혹은 닉네임 리턴 해주시면 됩니다.
var GetName = function () {
    return "JSPlayer";
}

// note : myNumber : 내 번호 (0 ~ 3 값)
// note : totalPlayerCount : 플레이어 수
// note : col : Map 너비
// note : row : Map 높이
// note : 본 함수는 시작과 함께 1회만 호출 됩니다.
var Initialize = function (myNumber, totalPlayerCount, col, row) {
}

// note : 게임 중 내내 호출 될 function 입니다.
// note : playerPosition[playerNumber] 하시면 해당 플레이어의 위치값을 알 수 있습니다.
// note : playerPower[playerNumber] 하시면 해당 플레이어의 곡갱이+ 값을 알 수 있습니다.
// note : playerStun[playerNumber] 하시면 해당 플레이어의 남은 스턴 턴 수를 알 수 있습니다.
// note : mapBlocks는 전체 맵을 나타내는 선형 int array입니다. mapBlocks가 가질 수 있는 값은 아래와 같습니다.
// note : ITEM_9 : 곡갱이 아이템 LV1, 본인의 Power를 +3만큼 증가시킨다. (Power 1당 1턴에 돌 블럭을 1만큼 제거한다.)
// note : ITEM_8 : 곡갱이 아이템 LV1, 본인의 Power를 +2만큼 증가시킨다.
// note : ITEM_7 : 곡갱이 아이템 LV1, 본인의 Power를 +1만큼 증가시킨다.
// note : ITEM_6 : 시간 정지 아이템 LV3, 본인을 제외한 플레이어들을 많은 턴수만큼 행동 불능으로 만든다. sqrt(col^2 + row^2)
// note : ITEM_5 : 시간 정지 아이템 LV2, 본인을 제외한 플레이어들을 중간 턴수만큼 행동 불능으로 만든다. sqrt(col^2 + row^2) * (2/3)
// note : ITEM_4 : 시간 정지 아이템 LV1, 본인을 제외한 플레이어들을 적은 턴수만큼 행동 불능으로 만든다. sqrt(col^2 + row^2) * (1/3)
// note : ITEM_3 : 산사태 아이템 LV3, 본인을 제외한 플레이어들의 주변 5칸을 무작위 돌블럭으로 메운다.
// note : ITEM_2 : 산사태 아이템 LV2, 본인을 제외한 플레이어들의 주변 3칸을 무작위 돌블럭으로 메운다.
// note : ITEM_1 : 산사태 아이템 LV1, 본인을 제외한 플레이어들의 주변 2칸을 무작위 돌블럭으로 메운다.
// note : GEM_5 : 다이아몬드 블럭 - 5 point
// note : GEM_3 : 금 블럭 - 3 point
// note : GEM_2 : 은 블럭 - 2 point 
// note : GEM_1 : 동 블럭 - 1 point
// note : NONE : 아무것도 없는 블럭
// note : ROCK_1 : 내구도 1인 돌 블럭
// note : ROCK_2 : 내구도 2인 돌 블럭
// note : ROCK_3 : 내구도 3인 돌 블럭
// note : ROCK_4 : 내구도 4인 돌 블럭
// note : ROCK_5 : 내구도 5인 돌 블럭
// note : ROCK_6 : 내구도 6인 돌 블럭
var Process = function (playerPosition, playerPower, playerStun, mapBlocks) {
    // note : Breakpoint 잡고 실시간 디버깅이 가능하도록 환경을 제공 하고 싶었으나,
    //      JS Engin 디버깅 환경 설정이 좀 번거롭네요. 아래 코드로 Log/Mines.log 파일에 로그를 남길 수 있습니다.
    Console.Info(test);

    return test++ % 4;
}
// note : 본인이 이동 하고 싶은 방향값을 반환 시켜 줍니다.
// note : 0 = 상, 1 = 우, 2 = 하, 3 = 좌
// note : 이동하려는 위치에 돌 블럭이 있을 경우, 본인이 가진 Power만큼 돌 블럭 가중치를 제거 합니다.
// note : Power가 3일때 6돌 블럭을 캘 경우, 6 - 3이 되어 3돌이 됩니다. 0값의 블럭으로만 이동이 가능 합니다.
// note : 맵의 가장자리로 이동 시, 제자리 걸음을 하게 되며 턴이 소모 됩니다.