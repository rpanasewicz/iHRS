using iHRS.Application.Exceptions;
using System;
using System.Net;

namespace iHRS.Api.Exceptions
{
    public class ErrorResult
    {
        public string Message { get; }
        public string Code { get; }
        public HttpStatusCode StatusCode { get; }

        public ErrorResult(string message, string code, HttpStatusCode statusCode)
        {
            Message = message;
            Code = code;
            StatusCode = statusCode;
        }

        public static ErrorResult MapFromException(Exception ex)
        {
            return ex switch
            {
                AppException stsException => new ErrorResult(stsException.Message, stsException.Code, stsException.StatusCode),
                _ => new ErrorResult(ex.Message, "no_code", HttpStatusCode.InternalServerError)
            };
        }
    }
}