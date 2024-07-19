namespace Info
{
    public record Info
    (
    );

    // 01 用户成功连接服务器并注册后，服务器分配 user_id
    public record Login
    (
        int id,
        string name
    ) : Info;

    // 10 用户设置个人信息
    public record UserInfo
    (
        string name
    ) : Info;

    // 20 用户创建房间，服务器分配 room_id
    public record RoomCreate
    (
        int id // room id
    ) : Info;

    // 21 用户尝试进入房间
    public record TryJoinRoom
    (
        int ec,
        int id // room id
    ) : Info;

    // 21 进入房间成功/失败
    public record JoinRoom
    (
        int id,
        int ec,
        int num,
        Dictionary<int, UserBriefInfo> parts
    ) : Info;

    public record UserBriefInfo
    (
        int id,
        string name
    ) : Info;

    public record Message
    (
        DateTime time,
        int sender,
        string message
    );

    // 22 在房间中发送消息
    public record RoomMessage
    (
        DateTime time,
        int sender,
        int room,
        string message
    ) : Message(time, sender, message);

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