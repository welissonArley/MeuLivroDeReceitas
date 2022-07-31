using HashidsNet;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Domain.Repositorios.Codigo;
using MeuLivroDeReceitas.Domain.Repositorios.Conexao;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;

namespace MeuLivroDeReceitas.Application.UseCases.Conexao.QRCodeLido;
public class QRCodeLidoUseCase : IQRCodeLidoUseCase
{
    private readonly IHashids _hashids;
    private readonly IConexaoReadOnlyRepositorio _repositorioConexao;
    private readonly ICodigoReadOnlyRepositorio _repositorio;
    private readonly IUsuarioLogado _usuarioLogado;

    public QRCodeLidoUseCase(
        ICodigoReadOnlyRepositorio repositorio,
        IUsuarioLogado usuarioLogado,
        IConexaoReadOnlyRepositorio repositorioConexao,
        IHashids hashids)
    {
        _repositorio = repositorio;
        _usuarioLogado = usuarioLogado;
        _repositorioConexao = repositorioConexao;
        _hashids = hashids;
    }


    public async Task<(RespostaUsuarioConexaoJson usuarioParaSeConectar, string idUsuarioQueGerouQRCode)> Executar(string codigoConexao)
    {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();
        var codigo = await _repositorio.RecuperarEntidadeCodigo(codigoConexao);

        await Validar(codigo, usuarioLogado);

        var usuarioParaSeConectar = new RespostaUsuarioConexaoJson
        {
            Id = _hashids.EncodeLong(usuarioLogado.Id),
            Nome = usuarioLogado.Nome
        };

        return (usuarioParaSeConectar, _hashids.EncodeLong(codigo.UsuarioId));
    }

    private async Task Validar(Domain.Entidades.Codigos codigo, Domain.Entidades.Usuario usuarioLogado)
    {
        if (codigo is null)
        {
            throw new MeuLivroDeReceitasException(ResourceMensagensDeErro.CODIGO_NAO_ENCONTRADO);
        }

        if (codigo.UsuarioId == usuarioLogado.Id)
        {
            throw new MeuLivroDeReceitasException(ResourceMensagensDeErro.VOCE_NAO_PODE_EXECUTAR_ESTA_OPERACAO);
        }

        var existeConexao = await _repositorioConexao.ExisteConexao(codigo.UsuarioId, usuarioLogado.Id);

        if (existeConexao)
        {
            throw new MeuLivroDeReceitasException(ResourceMensagensDeErro.ESTA_CONEXAO_JA_EXISTE);
        }
    }
}
