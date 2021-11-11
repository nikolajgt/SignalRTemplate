using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetcore.Interfaces
{
    public interface ITHub
    {
        Task Send(string message);
    }
}
