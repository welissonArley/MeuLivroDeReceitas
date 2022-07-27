using HashidsNet;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Domain.Repositorios.Codigo;

namespace MeuLivroDeReceitas.Application.UseCases.Conexao.GerarQRCode;
public class GerarQRCodeUseCase : IGerarQRCodeUseCase
{
    private readonly ICodigoWriteOnlyRepositorio _repositorio;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
    private readonly IHashids _hashIds;

    public GerarQRCodeUseCase(ICodigoWriteOnlyRepositorio repositorio, IUsuarioLogado usuarioLogado, IUnidadeDeTrabalho unidadeDeTrabalho, IHashids hashIds)
    {
        _unidadeDeTrabalho = unidadeDeTrabalho;
        _repositorio = repositorio;
        _usuarioLogado = usuarioLogado;
        _hashIds = hashIds;
    }
    
    public async Task<(string qrCode, string idUsuario)> Executar()
    {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();

        var codigo = new Domain.Entidades.Codigos
        {
            Codigo = Guid.NewGuid().ToString(),
            UsuarioId = usuarioLogado.Id
        };

        await _repositorio.Registrar(codigo);

        await _unidadeDeTrabalho.Commit();

        return (codigo.Codigo, _hashIds.EncodeLong(usuarioLogado.Id));
    }
}
