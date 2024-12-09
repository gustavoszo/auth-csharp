
using ApiUser.Security;
using ApiUser.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UsersApi.Data;
using UsersApi.Models;
using UsersApi.Services;

namespace ApiUser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<JwtService>();

            var connectionString = builder.Configuration.GetConnectionString("dbConnection");

            builder.Services.AddDbContext<UserDbContext>(opts => opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            //  Esta linha configura o Identity do ASP.NET Core, que � um sistema completo para gerenciamento de autentica��o e autoriza��o. Ele lida com usu�rios, roles (fun��es), autentica��o, gerenciamento de senhas, entre outros.
            builder.Services
                .AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<UserDbContext>() //  Essa linha diz ao Identity para usar o UserDbContext para armazenar os dados do Identity no banco de dados. Ou seja, ele vai usar o UserDbContext para criar tabelas como AspNetUsers (para usu�rios), AspNetRoles (para roles), AspNetUserRoles (associa��o de usu�rios e roles), etc.
                .AddDefaultTokenProviders();

            // Essas op��es s�o definidas para o middleware de autentica��o e s�o fundamentais para garantir que os tokens sejam v�lidos, confi�veis e seguros.
            builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true, // Valida o emissor do token
                   ValidateAudience = false, // Valida o p�blico alvo do token
                   ValidateLifetime = true, // Valida o tempo de expira��o do token
                   ValidateIssuerSigningKey = true, // Valida a chave de assinatura do token
                   ValidIssuer = "auth-api",
                   IssuerSigningKey = JwtUitls.GetSymmetricSecurityKey() // Chave usada para assinar o token
               };

               // Configura��o de eventos personalizados
               options.Events = new JwtBearerEvents
               {
                   OnAuthenticationFailed = context =>
                   {
                       context.NoResult(); // Evita outros middlewares de alterar o resultado
                       context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                       context.Response.ContentType = "application/json";
                       return context.Response.WriteAsync(JsonUtil.ToJson(new
                       {
                           error = "Token inv�lido ou expirado."
                       }));
                   },
                   OnChallenge = context =>
                   {
                       if (!context.Response.HasStarted)
                       {
                           context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                           context.Response.ContentType = "application/json";
                           return context.Response.WriteAsync(JsonUtil.ToJson(new
                           {
                               error = "Token ausente ou inv�lido."
                           }));
                       }
                       return Task.CompletedTask;
                   }
               };
           });

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
