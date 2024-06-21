using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ballastlane.Blog.Application.Models
{
    public class Result
    {
        public bool IsSuccess { get; protected set; }
        public string Message { get; protected set; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static Result Success(string? message) => new Result(true, message ?? string.Empty);

        public static Result Failure(string errorMessage) => new Result(false, errorMessage);
    }
}
