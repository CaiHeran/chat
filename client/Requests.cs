using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Client
{
    using DB = Database;

    internal static class Requests
    {
        public static async Task<JsonNode> RegisterAsync(string name)
        {
            return await Server.PostAsync(new JsonObject() {
                ["_type"] = 1,
                ["name"] = name
            }).ConfigureAwait(false);
        }

        public static async Task<JsonNode> SetMyInfoAsync(string name)
        {
            return await Server.PostAsync(new JsonObject() {
                ["_type"] = 10,
                ["name"] = name
            }).ConfigureAwait(false);
        }

        public static async Task<JsonNode> CreateRoomAsync()
        {
            return await Server.PostAsync(20, """{"_type":20}""").ConfigureAwait(false);
        }

        public static async Task<JsonNode> JoinRoomAsync(int id)
        {
            return await Server.PostAsync(21, $$"""{"_type":21,"roomid":{{id}}}""").ConfigureAwait(false);
        }

        public static async Task<JsonNode> SendRoomMessageAsync(string message)
        {
            return await Server.PostAsync(new JsonObject() {
                ["_type"] = 22,
                ["time"] = DateTime.Now,
                ["roomid"] = DB.Room!.Id,
                ["userid"] = DB.Me!.Id,
                ["message"] = message
            }).ConfigureAwait(false);
        }
        
        public static async Task<JsonNode> LeaveRoomAsync(int room_id)
        {
            return await Server.PostAsync(new JsonObject() {
                ["_type"] = 29,
                ["roomid"] = room_id
            }).ConfigureAwait(false);
        }
    }
}
