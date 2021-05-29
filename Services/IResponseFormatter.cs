using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform.Services
{
    public interface IResponseFormatter
    {
        Task Format(HttpContext context, string content);
    }
}
