using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volvo.Ecash.Api.ExceptionHandling
{
    /// <summary>
    /// 
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            this.next = next;
            _logger = logger;
        }

        /// <summary>
        /// invoke
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {

                _logger.LogError($"==> Invoke. Erro: {ex.Message} - {DateTime.Now.ToLongTimeString()} - Class : ExceptionHandlingMiddleware");
                _logger.LogError(ex, $"An unhandled exception has occurred while executing the request. Url: {context.Request.GetDisplayUrl()}. Request Data: " + GetRequestData(context));

                await HandleExceptionAsync(context, ex);
            }
        }

        private static string GetRequestData(HttpContext context)
        {
            var sb = new StringBuilder();

            if (context.Request.HasFormContentType && context.Request.Form.Any())
            {
                sb.Append("Form variables:");
                foreach (var x in context.Request.Form)
                {
                    sb.AppendFormat("Key={0}, Value={1}<br/>", x.Key, x.Value);
                }
            }

            sb.AppendLine("Method: " + context.Request.Method);

            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="objResult"></param>
        /// <param name="ex"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task ResultResponseListValidation(int code, object objResult, Exception ex, HttpContext context)
        {


            _logger.LogError($"error {ex.Message}");

            _logger.LogError($"==> ValidationException. Erro: {ex.Message} - {DateTime.Now.ToLongTimeString()} - Object resquest : {JsonConvert.SerializeObject(objResult)} Class : ExceptionHandlingMiddleware");
            var result = JsonConvert.SerializeObject(objResult);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;
            return context.Response.WriteAsync(result);
        }

        /// <summary>
        /// ResultResponse
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="context"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public Task ResultResponse(Exception ex, HttpContext context, int code)
        {

            _logger.LogError($"error {ex.Message}");
            _logger.LogError($"==> ResultResponse. Erro: {ex.Message} - {DateTime.Now.ToLongTimeString()} - Object resquest : {JsonConvert.SerializeObject(new { error = ex.Message })} Class : ExceptionHandlingMiddleware");
            var result = JsonConvert.SerializeObject(new { error = ex.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;
            return context.Response.WriteAsync(result);
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            List<object> listValidation = new List<object>();
            int code = 0;


            if (ex is ArgumentException)
                return ResultResponse(ex, context, StatusCodes.Status400BadRequest);
            else if (ex is ValidationException)
            {

                var erros = ((FluentValidation.ValidationException)ex).Errors;

                foreach (var failure in erros)
                {

                    listValidation.Add(new { Message = failure.ErrorMessage, Field = failure.PropertyName });
                    _logger.LogWarning($"Property {failure.PropertyName} failed validation.Error was: {failure.ErrorMessage}");
                }


                code = StatusCodes.Status400BadRequest;
                return ResultResponseListValidation(code, listValidation, ex, context);


            }
            else if (ex != null)
                return ResultResponse(ex, context, StatusCodes.Status500InternalServerError);


            return ResultResponse(ex, context, StatusCodes.Status500InternalServerError);



        }
    }
}
