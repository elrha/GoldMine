using PlayerInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NewPlayer
{
    public class Player : IPlayer
    {
        public void Dispose()
        {
        }

        // note : 본인의 이름을 반환 해 줍니다.
        public string GetName()
        {
            return "DotNetPlayer";
        }

        // note : myNumber : 내 번호 (0 ~ 3 값)
        // note : totalPlayerCount : 플레이어 수
        // note : col : Map 너비
        // note : row : Map 높이
        // note : 본 함수는 시작과 함께 1회만 호출 됩니다.
        public void Initialize(int myNumber, int totalPlayerCount, int col, int row)
        {
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
        public Arrow Process(GameInfo gameInfo, BlockType[] mineField)
        {
            switch ((new Random(DateTime.UtcNow.Millisecond)).Next() % 4)
            {
                case 0:
                    return Arrow.UP;
                case 1:
                    return Arrow.RIGHT;
                case 2:
                    return Arrow.DOWN;
                default:
                    return Arrow.LEFT;
            }
        }
    }
}