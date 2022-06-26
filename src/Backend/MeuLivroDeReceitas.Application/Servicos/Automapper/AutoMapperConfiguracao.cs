using AutoMapper;

namespace MeuLivroDeReceitas.Application.Servicos.Automapper;

public class AutoMapperConfiguracao : Profile
{
    public AutoMapperConfiguracao()
    {
        CreateMap<Comunicacao.Requisicoes.RequisicaoRegistrarUsuarioJson, Domain.Entidades.Usuario>()
            .ForMember(destino => destino.Senha, config => config.Ignore());
    }
}
