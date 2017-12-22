namespace FootballAnalyzes.Tests
{
    using AutoMapper;
    using FootballAnalyzes.Web.Infrastructure.Mapping;

    public class TestStartup
    {
        private static bool testsInitialized = false;

        public  static void Initialize()
        {
            if (!testsInitialized)
            {
                Mapper.Initialize(config => config.AddProfile<AutoMapperProfile>());
                testsInitialized = true;
            }
        }
    }
}
