using AutoMapper;
using HashidsNet;

namespace MeuLivroDeReceitas.Application.Servicos.Automapper;

public class AutoMapperConfiguracao : Profile
{
    private readonly IHashids _hashids;

    public AutoMapperConfiguracao(IHashids hashids)
    {
        _hashids = hashids;

        RequisicaoParaEntidade();
        EntidadeParaResposta();
    }

    private void RequisicaoParaEntidade()
    {
        CreateMap<Comunicacao.Requisicoes.RequisicaoRegistrarUsuarioJson, Domain.Entidades.Usuario>()
            .ForMember(destino => destino.Senha, config => config.Ignore());

        CreateMap<Comunicacao.Requisicoes.RequisicaoReceitaJson, Domain.Entidades.Receita>();
        CreateMap<Comunicacao.Requisicoes.RequisicaoIngredienteJson, Domain.Entidades.Ingrediente>();
    }

    private void EntidadeParaResposta()
    {
        CreateMap<Domain.Entidades.Receita, Comunicacao.Respostas.RespostaReceitaJson>()
            .ForMember(destino => destino.Id, config => config.MapFrom(origem => _hashids.EncodeLong(origem.Id)));

        CreateMap<Domain.Entidades.Ingrediente, Comunicacao.Respostas.RespostaIngredienteJson>()
            .ForMember(destino => destino.Id, config => config.MapFrom(origem => _hashids.EncodeLong(origem.Id)));

        CreateMap<Domain.Entidades.Receita, Comunicacao.Respostas.RespostaReceitaDashboardJson>()
            .ForMember(destino => destino.Id, config => config.MapFrom(origem => _hashids.EncodeLong(origem.Id)))
            .ForMember(destino => destino.QuantidadeIngredientes, config => config.MapFrom(origem => origem.Ingredientes.Count));

        CreateMap<Domain.Entidades.Usuario, Comunicacao.Respostas.RespostaPerfilUsuarioJson>();
        
        CreateMap<Domain.Entidades.Usuario, Comunicacao.Respostas.RespostaUsuarioConectadoJson>()
            .ForMember(destino => destino.Id, config => config.MapFrom(origem => _hashids.EncodeLong(origem.Id)));
    }
}
