using System;
using System.Linq;
using AutoMapper;
using FootballAnalyzes.Data.Models;
using FootballAnalyzes.Services.Models;
using FootballAnalyzes.Services.Models.Games;
using FootballAnalyzes.Services.Models.Teams;
using FootballAnalyzes.UpdateDatabase.BindingModels;

namespace FootballAnalyzes.Web.Infrastructure.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<GameStatistic, GameStatisticSM>();
            this.CreateMap<GameResult, GameResultSM>();
            this.CreateMap<Team, TeamSM>();
            this.CreateMap<League, LeagueSM>();
            this.CreateMap<League, LeagueListingSM>()
                .ForMember(l => l.LeagueGamesCount, cfg =>
                    cfg.MapFrom(c => c.Games.Count()));
            this.CreateMap<FootballGame, FootballGameSM>();

            
            //var allTypes = AppDomain
            //    .CurrentDomain
            //    .GetAssemblies()
            //    .Where(a => a.GetName().Name.Contains("FootballAnalyzes"))
            //    .SelectMany(a => a.GetTypes());

            //allTypes
            //    .Where(t => t.IsClass && !t.IsAbstract && t
            //        .GetInterfaces()
            //        .Where(i => i.IsGenericType)
            //        .Select(i => i.GetGenericTypeDefinition())
            //        .Contains(typeof(IMapFrom<>)))
            //    .Select(t => new
            //    {
            //        Destination = t,
            //        Source = t
            //            .GetInterfaces()
            //            .Where(i => i.IsGenericType)
            //            .Select(i => new
            //            {
            //                Definition = i.GetGenericTypeDefinition(),
            //                Arguments = i.GetGenericArguments()
            //            })
            //            .Where(i => i.Definition == typeof(IMapFrom<>))
            //            .SelectMany(i => i.Arguments)
            //            .First(),
            //    })
            //    .ToList()
            //    .ForEach(mapping => this.CreateMap(mapping.Source, mapping.Destination));

            //allTypes
            //    .Where(t => t.IsClass
            //        && !t.IsAbstract
            //        && typeof(IHaveCustomMapping).IsAssignableFrom(t))
            //    .Select(Activator.CreateInstance)
            //    .Cast<IHaveCustomMapping>()
            //    .ToList()
            //    .ForEach(mapping => mapping.ConfigureMapping(this));
        }
    }
}
