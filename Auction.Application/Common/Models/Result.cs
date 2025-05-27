using Auction.Application.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Auction.Application.Common.Models
{
    public record Error
    {
        public string ErrorMessage { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public IActionResult ToActionResult()
        {
            return new ObjectResult(ErrorMessage) { StatusCode = StatusCode.GetInt()};
        }
    }
    public record Success<T>
    {
        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public IActionResult ToActionResult()
        {
            return new ObjectResult(Data) { StatusCode = StatusCode.GetInt() };
        }
        public IActionResult ToActionResult(object newData)
        {
            return new ObjectResult(newData) { StatusCode = StatusCode.GetInt() };
        }
    }
    public record Success
    {
        public HttpStatusCode StatusCode { get; set; }
    }
    public class Result<T>
    {
        public bool IsSuccess { get; set; } = false;
        public Success<T>? Success { get; set; }
        public Error? Error { get; set; }

        public static Result<T> Created(T data)
        {
            return new Result<T>() { Success = new() { Data = data, StatusCode = HttpStatusCode.Created }, IsSuccess = true };
        }
        public static Result<T> Ok(T data)
        {
            return new Result<T>() { Success = new() { Data = data, StatusCode = HttpStatusCode.OK }, IsSuccess = true };
        }
        public static Result<T> NoContent(T data)
        {
            return new Result<T>() { Success = new() { Data = data, StatusCode = HttpStatusCode.NoContent }, IsSuccess = true };
        }

        public static Result<T> BadRequest(string errorMessage)
        {
            return new Result<T>() { Error = new() { ErrorMessage = errorMessage, StatusCode = HttpStatusCode.BadRequest } };
        }

    }
    public class Result
    {
        public bool IsSuccess { get; set; }
        public Success? Success { get; set; }
        public Error? Error { get; set; }

        public static Result NoContent()
        {
            return new Result() { Success = new() { StatusCode = HttpStatusCode.NoContent }, IsSuccess = true };
        }

        public static Result BadRequest(string errorMessage)
        {
            return new Result() { Error = new() { ErrorMessage = errorMessage, StatusCode = HttpStatusCode.BadRequest } };
        }
    }
}
