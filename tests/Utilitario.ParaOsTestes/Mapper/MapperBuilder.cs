using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.Automapper;
using Utilitario.ParaOsTestes.Hashids;

namespace Utilitario.ParaOsTestes.Mapper;

public class MapperBuilder
{
    public static IMapper Instancia()
    {
        var hashids = HashidsBuilder.Instance().Build();

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperConfiguracao(hashids));
        });
        return mockMapper.CreateMapper();
    }
}
