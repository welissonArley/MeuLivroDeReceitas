using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Domain.Extension;
using MeuLivroDeReceitas.Domain.Repositorios.Conexao;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;

namespace MeuLivroDeReceitas.Application.UseCases.Dashboard;
public class DashboardUseCase : IDashboardUseCase
{
    private readonly IConexaoReadOnlyRepositorio _conexoesRepositorio;
    private readonly IReceitaReadOnlyRepositorio _repositorio;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IMapper _mapper;

    public DashboardUseCase(
        IReceitaReadOnlyRepositorio repositorio,
        IUsuarioLogado usuarioLogado,
        IMapper mapper,
        IConexaoReadOnlyRepositorio conexoesRepositorio)
    {
        _mapper = mapper;
        _repositorio = repositorio;
        _usuarioLogado = usuarioLogado;
        _conexoesRepositorio = conexoesRepositorio;
    }

    public async Task<RespostaDashboardJson> Executar(RequisicaoDashboardJson requisicao)
    {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

        var receitas = await _repositorio.RecuperarTodasDoUsuario(usuarioLogado.Id);

        receitas = Filtrar(requisicao, receitas);

        var receitasUsuariosConctados = await ReceitasUsuariosConectados(requisicao, usuarioLogado);

        receitas = receitas.Concat(receitasUsuariosConctados).ToList();

        return new RespostaDashboardJson
        {
            Receitas = _mapper.Map<List<RespostaReceitaDashboardJson>>(receitas)
        };
    }

    private async Task<IList<Domain.Entidades.Receita>> ReceitasUsuariosConectados(RequisicaoDashboardJson requisicao, Domain.Entidades.Usuario usuarioLogado)
    {
        var conexoes = await _conexoesRepositorio.RecuperarDoUsuario(usuarioLogado.Id);

        var usuariosConectados = conexoes.Select(c => c.Id).ToList();
        var receitasUsuariosConctados = await _repositorio.RecuperarTodasDosUsuarios(usuariosConectados);

        return Filtrar(requisicao, receitasUsuariosConctados);
    }

    private static IList<Domain.Entidades.Receita> Filtrar(RequisicaoDashboardJson requisicao, IList<Domain.Entidades.Receita> receitas)
    {
        if (receitas is null)
            return new List<Domain.Entidades.Receita>();

        var receitasFiltradas = receitas;

        if (requisicao.Categoria.HasValue)
        {
            receitasFiltradas = receitas.Where(r => r.Categoria == (Domain.Enum.Categoria)requisicao.Categoria.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(requisicao.TituloOuIngrediente))
        {
            receitasFiltradas = receitas.Where(r => r.Titulo.CompararSemConsiderarAcentoUpperCase(requisicao.TituloOuIngrediente) || r.Ingredientes.Any(ingrediente => ingrediente.Produto.CompararSemConsiderarAcentoUpperCase(requisicao.TituloOuIngrediente))).ToList();
        }

        return receitasFiltradas.OrderBy(c => c.Titulo).ToList();
    }
}
