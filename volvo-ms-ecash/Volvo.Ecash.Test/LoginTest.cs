using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Volvo.Ecash.Application.Utils.Security;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Repository.Interface;
using Xunit;
using System.Net.Http;
using Volvo.Ecash.Infrastructure.AccessLogQueue;
using System.Net;
using Newtonsoft.Json;
using FluentValidation;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Application.Service;
using Volvo.Ecash.Application.Validator;

namespace Volvo.Ecash.Test
{

    public class LoginTest
    {

        private readonly IConfiguration _configuration;


        
        private readonly TokenConfiguration tokenConfiguration;
        private readonly TokenValidationParameters tokenValidationParameters;
        private readonly LogQueueConfiguration _logQueueConfiguration;
        public PermissionDto itempermissionDto = null;
        public LoginTest()
        {
            var settings = new List<KeyValuePair<string, string>>
                                {
                                new KeyValuePair<string, string>("TokenConfigurations:Issuer", "Volvo.Ecash.issuer"),
                                new KeyValuePair<string, string>("TokenConfigurations:Audience", "Volvo.Ecash.audience"),
                                new KeyValuePair<string, string>("TokenConfigurations:SecurityKey", "2a89585ddec@#23@$as1f2*5499b63dc740dc8@%%212s196ccf145fe4314275aec8d*%2#3we51206ad69eab7361cfa"),
                                new KeyValuePair<string, string>("TokenConfigurations:Seconds", "1200"),
                                new KeyValuePair<string, string>("UserSettings:UrlApi", "https://user.dev.saedigital.io"),
                                new KeyValuePair<string, string>("UserSettings:ProfileByUserEndpoint", "api/User/v1/GetUser")
                                };

            var builder = new ConfigurationBuilder().AddInMemoryCollection(settings);
            this._configuration = builder.Build();

            tokenConfiguration = new TokenConfiguration
            {
                Issuer = this._configuration["TokenConfigurations:Issuer"],
                Audience = this._configuration["TokenConfigurations:Audience"],
                SecurityKey = this._configuration["TokenConfigurations:SecurityKey"],
                Seconds = int.Parse(this._configuration["TokenConfigurations:Seconds"])
            };

            var sing = new SigninConfigurations(_configuration);

            tokenValidationParameters = new TokenValidationParameters
            {
                ValidAudience = tokenConfiguration.Audience,
                ValidIssuer = tokenConfiguration.Issuer,
                IssuerSigningKey = sing.Key,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };

            _logQueueConfiguration = new LogQueueConfiguration();
        }

        [Fact]
        public void ShouldBeMessageSucess()
        {

            List<PermissionDto> permissionDtos = new List<PermissionDto>();
            itempermissionDto = new PermissionDto()
            {
                Id = 1,
                Menu = "Cadastro Usuario",
                Title = "Cadastro Usuario"
            };

            UserDto user = new UserDto();
            user.Id = 1;
            user.ProfileId = 1;
            user.ProfileCode = "MASTER";

            var serviceEmail = new Mock<IEmailService>();
            var loginRepositoryMock = new Mock<ILoginRepository>();
            loginRepositoryMock.Setup(c => c.SelectByLogin(It.IsAny<LoginDto>())).ReturnsAsync(new LoginDto() { Login = "UserAdmin", Password = "teste@123", Active = true });
            var integrationRepositoryMock = new Mock<IIntegrationRepository>();
            integrationRepositoryMock.Setup(x => x.SendPost(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage());
            integrationRepositoryMock.Setup(x => x.GetUrlApi(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(user))
            });

            integrationRepositoryMock.Setup(c => c.GetIntegration(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(permissionDtos))
            });

            var sing = new SigninConfigurations(_configuration);

            var logger = new Mock<ILogger<LoginService>>();

            var loginDto = new LoginDto()
            {
                Login = "UserAdmin",
                Password = "teste@123"
            };

                       

            var serviceLogin = new LoginService(loginRepositoryMock.Object, integrationRepositoryMock.Object, sing, tokenConfiguration, logger.Object, tokenValidationParameters, _logQueueConfiguration, _configuration, serviceEmail.Object);

            var result = serviceLogin.GetByLogin<LoginValidator>(loginDto).Result;

            string message = result.Message;

            string expected = "Successfully generated token";
            string actual = message;

            Assert.Equal(expected, actual, StringComparer.CurrentCultureIgnoreCase);


        }


        [Fact]
        public void ShouldBeGeneteretedToken()
        {
            List<PermissionDto> permissionDtos = new List<PermissionDto>();
            PermissionDto permissionDto = new PermissionDto
            {
                Id = 1,
                Menu = "Cadastro Usuario",
                Title = "Cadastro Usuario"
            };

            permissionDtos.Add(permissionDto);

            UserDto user = new UserDto();
            user.Id = 1;
            user.ProfileId = 1;
            user.ProfileCode = "MASTER";

            var loginRepositoryMock = new Mock<ILoginRepository>();

            loginRepositoryMock.Setup(c => c.SelectByLogin(It.IsAny<LoginDto>())).ReturnsAsync(new LoginDto() { Login = "UserAdmin", Password = "teste@123", Active = true });


            var integrationRepositoryMock = new Mock<IIntegrationRepository>();

            integrationRepositoryMock.Setup(c => c.GetIntegration(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(permissionDtos))
            });



            integrationRepositoryMock.Setup(x => x.SendPost(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage());

            integrationRepositoryMock.Setup(x => x.GetUrlApi(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(user))
            });

            var sing = new SigninConfigurations(_configuration);

            var logger = new Mock<ILogger<LoginService>>();

            var loginDto = new LoginDto()
            {
                Login = "UserAdmin",
                Password = "teste@123"
            };
            var serviceEmail = new Mock<IEmailService>();
            var serviceLogin = new LoginService(loginRepositoryMock.Object, integrationRepositoryMock.Object, sing, tokenConfiguration, logger.Object, tokenValidationParameters, _logQueueConfiguration, _configuration, serviceEmail.Object);
            var result = serviceLogin.GetByLogin<LoginValidator>(loginDto).Result;


            string token = result.Token;
            Assert.NotEmpty(token);
        }


        [Fact]
        public void ShouldBeLoginInvalid()
        {

            var loginRepositoryMock = new Mock<ILoginRepository>();
            loginRepositoryMock.Setup(c => c.SelectByLogin(It.IsAny<LoginDto>())).ReturnsAsync(new LoginDto() { Login = "UserAdmin", Password = "teste@123", Active = true });
            var integrationRepositoryMock = new Mock<IIntegrationRepository>();
            integrationRepositoryMock.Setup(x => x.SendPost(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage());

            var sing = new SigninConfigurations(_configuration);

            var logger = new Mock<ILogger<LoginService>>();

            var loginDto = new LoginDto()
            {
                Login = "",
                Password = "teste@123"
            };
            var serviceEmail = new Mock<IEmailService>();
            var serviceLogin = new LoginService(loginRepositoryMock.Object, integrationRepositoryMock.Object, sing, tokenConfiguration, logger.Object, tokenValidationParameters, _logQueueConfiguration, _configuration, serviceEmail.Object);



            var result = Assert.ThrowsAsync<ValidationException>(() => serviceLogin.GetByLogin<LoginValidator>(loginDto)).Result;



            var message = result.Errors.FirstOrDefault().ErrorMessage;
            var messageResult = "É necessário informar o campo login.";

            Assert.Equal(message, messageResult);


        }

        [Fact]
        public void ShouldInvalidateToken()
        {
            var loginRepositoryMock = new Mock<ILoginRepository>();
            loginRepositoryMock.Setup(c => c.SelectByLogin(It.IsAny<LoginDto>())).ReturnsAsync(new LoginDto() { Login = "UserAdmin", Password = "teste@123", Active = true });
            var integrationRepositoryMock = new Mock<IIntegrationRepository>();
            integrationRepositoryMock.Setup(x => x.SendPost(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage());

            var sing = new SigninConfigurations(_configuration);

            var logger = new Mock<ILogger<LoginService>>();

            var loginDto = new LoginDto()
            {
                Id = 1,
                Login = "UserAdmin",
                Password = "teste@123"
            };
            var serviceEmail = new Mock<IEmailService>();
            var serviceLogin = new LoginService(loginRepositoryMock.Object, integrationRepositoryMock.Object, sing, tokenConfiguration, logger.Object, tokenValidationParameters, _logQueueConfiguration, _configuration,serviceEmail.Object);

            var result = serviceLogin.InvalidateRefreshToken(loginDto).Result;

            var message = result.Message;
            var messageResult = "Refresh Token invalidado com sucesso";
            Assert.True(result.Success);
            Assert.Equal(message, messageResult);
        }

        [Fact]
        public void ShouldUpdateLogin()
        {
            var loginRepositoryMock = new Mock<ILoginRepository>();

            loginRepositoryMock.Setup(c => c.UpdateLoginAsync(It.IsAny<int>(), It.IsAny<LoginDto>())).ReturnsAsync(new LoginDto() { Id = 1, Login = "UserAdmin", Password = "teste@123" });
            var integrationRepositoryMock = new Mock<IIntegrationRepository>();
            integrationRepositoryMock.Setup(x => x.SendPost(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage());

            var sing = new SigninConfigurations(_configuration);

            var logger = new Mock<ILogger<LoginService>>();

            var login = new LoginDto()
            {
                Id = 1,
                Login = "UserAdmin",
                Password = "teste@123"
            };
            var serviceEmail = new Mock<IEmailService>();
            var serviceLogin = new LoginService(loginRepositoryMock.Object, integrationRepositoryMock.Object, sing, tokenConfiguration, logger.Object, tokenValidationParameters, _logQueueConfiguration, _configuration,serviceEmail.Object);

            dynamic result = serviceLogin.UpdateLogin(1, login).Result;

            var messageResult = "Login atualizado com sucesso.";
            Assert.Equal(1, result.Id);
            Assert.Equal(messageResult, result.Message);
        }

        [Fact]
        public void ShouldDeleteLogin()
        {
            var loginRepositoryMock = new Mock<ILoginRepository>();

            loginRepositoryMock.Setup(c => c.DeleteAsync(It.IsAny<int>())).ReturnsAsync(true);
            var integrationRepositoryMock = new Mock<IIntegrationRepository>();
            integrationRepositoryMock.Setup(x => x.SendPost(It.IsAny<object>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage());

            var sing = new SigninConfigurations(_configuration);

            var logger = new Mock<ILogger<LoginService>>();

            var serviceEmail = new Mock<IEmailService>();

            var serviceLogin = new LoginService(loginRepositoryMock.Object, integrationRepositoryMock.Object, sing, tokenConfiguration, logger.Object, tokenValidationParameters, _logQueueConfiguration, _configuration,serviceEmail.Object);

            dynamic result = serviceLogin.DeleteLogin(1).Result;

            var messageResult = "Login excluído com sucesso.";
            Assert.Equal(1, result.Id);
            Assert.Equal(messageResult, result.Message);
        }
    }
}