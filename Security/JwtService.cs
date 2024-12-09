namespace ApiUser.Security
{
    public class JwtService
    {

        public string GetToken(string username)
        {
            return JwtUitls.GenerateToken(username);
        }

    }
}
