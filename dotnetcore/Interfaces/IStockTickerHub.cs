using dotnetcore.Models;
using dotnetcore.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetcore.Interfaces
{
    public interface IStockTickerHub
    {
        Task OnConnectedAsync();
        Task OnDisconnectedAsync(Exception ex);
        IEnumerable<Stock> GetAllStocks();

    }
}
