﻿using AutoMapper;

namespace Auction.Application.Common.Mappings
{
    public interface IMapWith<T>
    {
        void Mapping(Profile profile)
            => profile.CreateMap(typeof(T), GetType());
    }
}
