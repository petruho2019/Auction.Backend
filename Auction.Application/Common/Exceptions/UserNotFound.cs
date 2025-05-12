using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Common.Exceptions
{
    class UserNotFound : Exception
    {
        public UserNotFound(string message) : base(message) { }
    }
}
