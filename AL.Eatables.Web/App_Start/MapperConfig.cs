using AL.Eatables.Web.Models;
using AutoMapper;

namespace AL.Eatables.Web.App_Start
{
    public class MapperConfig
    {
        private static volatile bool _isInitialized;
        private static readonly object _lock = new object();

        public static void Configure()
        {
            if (!_isInitialized)
            {
                lock (_lock)
                {
                    if (!_isInitialized)
                    {
                        WebToDomain();
                        _isInitialized = true;
                    }
                }
            }
        }

        private static void WebToDomain()
        {
            Mapper.Initialize(config => {
                config.CreateMap<EatableInput, Eatable>()
                    .ForMember(dest => dest.Id, action => action.Ignore());
            });
        }
    }
}