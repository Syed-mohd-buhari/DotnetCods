using Microsoft.Extensions.DependencyInjection;
using System;

namespace CAM.WebAPI.Helper
{
    public class LazyService<T> : Lazy<T> where T : class
    {
        public LazyService(IServiceProvider provider)
            : base(() => provider.GetRequiredService<T>())
        { }
    }

}
