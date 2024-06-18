namespace Info
{
    // 01 用户成功连接服务器后，服务器分配 user_id
    public struct Login
    {
        public int id;
    }

    // 02 用户设置个人信息
    struct UserInfo
    {
        public string name;
    }

    // 10 用户创建房间，服务器分配 room_id
    struct RoomCreate
    {
        public int id;
    }

    // 11 用户尝试进入房间
    struct TryJoinRoom
    {
        public int id;
    }

    // 11 进入房间成功/失败
    struct JoinRoom
    {
        public int ec;
        public int id;
        public int num;
        public Dictionary<int, UserBriefInfo> parts;
    }

    struct UserBriefInfo
    {
        public int id;
        public string name;
    }

    // 11 在房间中发送消息
    struct RoomMessage
    {
        public string message;
    }
}