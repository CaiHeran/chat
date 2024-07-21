using System.Net;

namespace Info
{
    public record Info
    (
    );

    // 01 用户成功连接服务器并注册后，服务器分配 user_id
    public record Login
    (
        int id, //user id
        string name
    ) : Info;

    // 10 用户设置个人信息
    public record UserInfo
    (
        string name
    ) : Info;

    public record UserFullInfo
    (
        int id,
        IPAddress ipe,
        UserInfo info
    ) : Info;

    // 20 用户创建房间，服务器分配 room_id
    public record RoomCreate
    (
        int room // room id
    ) : Info;

    // 21 用户尝试进入房间
    public record TryJoinRoom
    (
        int ec,
        int room // room id
    ) : Info;

    public record UserBriefInfo
    (
        int id, // user id
        string name
    ) : Info;

    // 21 进入房间成功/失败
    public record MyJoinRoom
    (
        int room,
        int ec,
        List<UserBriefInfo> list
    ) : Info;

    public record OtherJoinRoom
    (
        int room, // room id
        UserBriefInfo info
    ) : Info;

    public record Message
    (
        DateTime time,
        int id,
        string message
    );

    // 22 在房间中发送消息
    public record RoomMessage
    (
        DateTime time,
        int room,
        int id,
        string message
    ) : Message(time, id, message);

    // 23 房间成员退出房间
    public record LeaveRoom
    (
        int room,
        int id
    ) : Info;

    /*
    record ChatRoomStart : Info
    (
        public const int type = 1001;
        // todo
    );
    record ChatRoomSend : Info//发送消息
    (
        public const int type = 1002;
        public string msg;
        // todo
    );
    record ChatRoomEnd : Info//房主退出
    (
        public const int type = 1003;
        // todo
    );
    record GobangStart : Info
    (
        public const int type = 2001;
        // Todo
    );
    record GobangMoment : Info
    (
        public const int type = 2002;
        public int posx, posy;//棋盘坐标行列
        // Todo
    );
    record GobangResult : Info
    (
        public const int type = 2003;
        // winner
        // todo
    );
    */
}