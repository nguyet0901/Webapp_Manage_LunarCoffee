using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace CoffeePage.Models
{
    public class AsyncPageFilter : IAsyncPageFilter
    {
        private readonly IConfiguration _config;
        public AsyncPageFilter(IConfiguration config)
        {
            _config = config;
        }
        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            return Task.CompletedTask;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context ,PageHandlerExecutionDelegate next)
        {

            await next.Invoke();
        }
    }
}

