using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Domain
{
    // TODO сделать hubs, по примеру из signalr chat
    // TODO спросить про UserAuctionVm, в Vm/Auctions/ нормально ли его так использовать?
    // понимаю что если надо будет создать новые поля для user то и этот класс
    // нельзя будет использовать везде, к примеру в getlist нужен phone
    // а в GetById нет

    // TODO так же спросить при getById Auction, можно ли возвращать его с 
    // историей bids, либо сделать отдельный метод который будет возвращать
    // bids по id аукциона
}
