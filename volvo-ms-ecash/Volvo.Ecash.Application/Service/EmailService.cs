using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Linq;
using Volvo.Ecash.Application.Utils;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Application.Service
{
    public class EmailService : IEmailService
    {


        private readonly IConfiguration _configuration;

        public EmailService(EmailSettings emailSettings, IConfiguration configuration)
        {

            _configuration = configuration;

        }

        public Task SendEmailAsync(AuthenticationResultDto authenticationResultDto, ResetPasswordDto resetPasswordDto, string newPassword, string token)
        {
            throw new NotImplementedException();
        }

        //public async Task SendEmailAsync(AuthenticationResultDto authenticationResultDto, ResetPasswordDto resetPasswordDto, string newPassword, string token)
        //{
        //    var objEmail = new UserEmailDto
        //    {
        //        User = resetPasswordDto.Login,
        //        Password = newPassword,
        //        Email = resetPasswordDto.Email

        //    };

        //    var urlUser = string.Format("{0}/{1}", _configuration["Integration:UserApi"], _configuration["Integration:UrlGetUser"]);
        //    urlUser = urlUser + resetPasswordDto.Login;
        //    var resultUser = await GetIntegration(urlUser, token);

        //    if (resultUser.IsSuccessStatusCode)
        //    {

        //        var user = JsonConvert.DeserializeObject<List<UserDto>>(
        //           resultUser.Content.ReadAsStringAsync().Result
        //       );


        //        if (user == null)
        //        {
        //            throw new ArgumentException("Usuario não encontrado");
        //        }

        //        objEmail.Email = user.FirstOrDefault().Email;

        //    }

        //    var url = string.Format("{0}/{1}", _configuration["EmailSettings:EmailApi"], _configuration["EmailSettings:UrlEmailResetPassword"]);
        //    var result = await PostIntegration(objEmail, token, url);

        //    if (!result.IsSuccessStatusCode)
        //        throw new ArgumentException("N�o foi possivel enviar o email");
        //}

        //private async Task<HttpResponseMessage> PostIntegration(object obj, string token, string url)
        //{
        //    var client = new HttpClient
        //    {
        //        BaseAddress = new Uri(url)
        //    };
        //    var request = new HttpRequestMessage(HttpMethod.Post, "");
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //    var json = JsonConvert.SerializeObject(obj);
        //    var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
        //    request.Content = stringContent;

        //    return await client.SendAsync(request);
        //}

        //private async Task<HttpResponseMessage> GetIntegration(string url, string token)
        //{

        //    var client = new HttpClient
        //    {
        //        BaseAddress = new Uri(url)
        //    };
        //    var request = new HttpRequestMessage(HttpMethod.Get, "");
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //    var stringContent = new StringContent("", Encoding.UTF8, "application/json");
        //    request.Content = stringContent;

        //    return await client.SendAsync(request);
        //}
    }
}
