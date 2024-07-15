namespace Info
{
    public interface Info
    {
    }
    public struct TypeInfo
    {
        public int type;
    }

    // 01 用户成功连接服务器并注册后，服务器分配 user_id
    public struct Login : Info
    {
        public const int type = 1;
        public string name;
        public int id;//个人id
    }

    // 10 用户设置个人信息
    struct UserInfo : Info
    {
        public const int type = 10;
        public string name;
    }

    // 20 用户创建房间，服务器分配 room_id
    struct RoomCreate : Info
    {
        public const int type = 20;
        public int ec;
        public int id;//房间id
    }

    // 21 用户尝试进入房间
    struct TryJoinRoom : Info
    {
        public const int type = 21;
        public int id;//房间id
    }

    // 21 进入房间成功/失败
    struct JoinRoom : Info
    {
        public const int type = 21;
        public int id;
        public int ec;
        public int num;
        public Dictionary<int, UserBriefInfo> parts;
    }

    struct UserBriefInfo : Info
    {
        public int id;
        public string name;
    }

    // 22 在房间中发送消息
    struct RoomMessage : Info
    {
        public const int type = 22;
        public int id;  // room id
        public string message;
    }

    struct ChatRoomStart : Info
    {
        public const int type = 1001;
        // todo
    }
    struct ChatRoomSend : Info//发送消息
    {
        public const int type = 1002;
        public string msg;
        // todo
    }
    struct ChatRoomEnd : Info//房主退出
    {
        public const int type = 1003;
        // todo
    }
    struct GoBangStart : Info
    {
        public const int type = 2001;
        // Todo
    }
    struct GoBangMoment : Info
    {
        public const int type = 2002;
        public int posx, posy;//棋盘坐标行列
        // Todo
    }
    struct GoBangResult : Info
    {
        public const int type = 2003;
        // winner
        // todo
    }
}