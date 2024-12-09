namespace ApiUser.Utils
{
    public static class JsonUtil
    {
        public static string ToJson(this object obj)
        {
            return System.Text.Json.JsonSerializer.Serialize(obj);
        }
    }
}
