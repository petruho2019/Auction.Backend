using Auction.Application.Common.Models;
using Auction.Application.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Features
{
    public class BaseComponentHandler
    {
        public readonly IAuctionContext _dbContext;
        public readonly IMapper _mapper;
        public readonly ICurrentUserService _currentUserService;
        public BaseComponentHandler(
            IAuctionContext dbContext, 
            IMapper mapper, 
            ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public Result<T> CreateSuccessResult<T>(T data)
        {
            return new Result<T>()
            {
                Data = data,
                IsSuccess = true
            };
        }
        public Result<T> CreateFailureResult<T>(string errorMessage)
        {
            return new Result<T>()
            {
                ErrorMessage = errorMessage,
                IsSuccess = false
            };
        }

        public Result CreateFailureResult(string errorMessage)
        {
            return new Result()
            {
                ErrorMessage = errorMessage,
                IsSuccess = false
            };
        }

        public Result CreateSuccessResult()
        {
            return new();
        }
    }
}
