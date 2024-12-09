using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Text;

namespace ApiUser.Security
{
    public class JwtUitls
    {
        // Essa chave é essencial para garantir que o token não foi alterado por terceiros. Tanto quem emite o token quanto quem valida precisam conhecer essa chave.
        public static string SECRET_KEY = "GSDFGSFDGfsadfasdfasdfsdgdPADASGdashdk";

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));
        }

        public static SigningCredentials GetSigningCredentials()
        {
            // Converte a chave secreta (SECRET_KEY) em um formato que a biblioteca de segurança do .NET pode usar.
            var key = GetSymmetricSecurityKey();

            // Combina a chave gerada com um algoritmo de assinatura, neste caso, o HmacSha256. Esse algoritmo é usado para garantir a integridade do token.
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }

        // Quando o token é assinado, o processo gera uma assinatura digital que é anexada ao token. Essa assinatura é essencial para:
        // Garantir a autenticidade: Comprova que o token foi gerado por quem possui a chave secreta.
        public static string GenerateToken(string username)
        {
            var token = new JwtSecurityToken
                (
                issuer: "auth-api",
                expires: DateTime.Now.AddMinutes(20),
                claims: new Claim[] { new Claim("username", username) },
                signingCredentials: GetSigningCredentials()
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
