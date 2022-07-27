using MeuLivroDeReceitas.Api.WebSockets;
using MeuLivroDeReceitas.Application.UseCases.Conexao.GerarQRCode;
using MeuLivroDeReceitas.Application.UseCases.Conexao.RecusarConexao;
using MeuLivroDeReceitas.Exceptions;
using Moq;
using Utilitario.ParaOsTestes.Respostas;
using WebApi.Test.V1.Conexao.Builder;
using Xunit;

namespace WebApi.Test.V1.Conexao;
public class ConexaoRecusadaTeste
{
    [Fact]
    public async Task Validar_Sucesso()
    {
        var codigoGeradoParaConexao = Guid.NewGuid().ToString();
        var usuarioParaSeConectar = RespostaUsuarioConexaoBuilder.Construir();

        (var mockHubContext, var mockClientProxy, var mockClients, var mockHubContextCaller) = MockWebSocketConnectionsBuilder.Construir();

        var useCaseGerarQRCode = GerarQRCodeUseCaseBuilder(codigoGeradoParaConexao);
        var useCaseConexaoRecusada = GerarConexaoRecusadaUseCaseBuilder();

        var hub = new AdicionarConexao(mockHubContext.Object, useCaseGerarQRCode, null, useCaseConexaoRecusada, null)
        {
            Context = mockHubContextCaller.Object,
            Clients = mockClients.Object
        };

        await hub.GetQRCode();

        await hub.RecusarConexao();

        mockClientProxy.Verify(
            clientProxy => clientProxy.SendCoreAsync("OnConexaoRecusada",
            It.Is<object[]>(resposta => resposta != null && resposta.Length == 0), default), Times.Once);
    }

    [Fact]
    public async Task Validar_Erro_Desconhecido()
    {
        var codigoGeradoParaConexao = Guid.NewGuid().ToString();
        var usuarioParaSeConectar = RespostaUsuarioConexaoBuilder.Construir();

        (var mockHubContext, var mockClientProxy, var mockClients, var mockHubContextCaller) = MockWebSocketConnectionsBuilder.Construir();

        var useCaseGerarQRCode = GerarQRCodeUseCaseBuilder(codigoGeradoParaConexao);
        var useCaseConexaoRecusada = GerarConexaoRecusada_ErroDesconhecidoUseCaseBuilder();

        var hub = new AdicionarConexao(mockHubContext.Object, useCaseGerarQRCode, null, useCaseConexaoRecusada, null)
        {
            Context = mockHubContextCaller.Object,
            Clients = mockClients.Object
        };

        await hub.GetQRCode();

        await hub.RecusarConexao();

        mockClientProxy.Verify(
            clientProxy => clientProxy.SendCoreAsync("Erro",
            It.Is<object[]>(resposta => resposta != null
                && resposta.Length == 1
                && resposta.First().Equals(ResourceMensagensDeErro.ERRO_DESCONHECIDO)), default), Times.Once);
    }

    [Fact]
    public async Task Validar_Erro_MeuLivroReceitasException()
    {
        var codigoGeradoParaConexao = Guid.NewGuid().ToString();
        var usuarioParaSeConectar = RespostaUsuarioConexaoBuilder.Construir();

        (var mockHubContext, var mockClientProxy, var mockClients, var mockHubContextCaller) = MockWebSocketConnectionsBuilder.Construir();

        var useCaseGerarQRCode = GerarQRCodeUseCaseBuilder(codigoGeradoParaConexao);
        var useCaseConexaoRecusada = GerarConexaoRecusadaUseCaseBuilder();

        var hub = new AdicionarConexao(mockHubContext.Object, useCaseGerarQRCode, null, useCaseConexaoRecusada, null)
        {
            Context = mockHubContextCaller.Object,
            Clients = mockClients.Object
        };

        await hub.RecusarConexao();

        mockClientProxy.Verify(
            clientProxy => clientProxy.SendCoreAsync("Erro",
            It.Is<object[]>(resposta => resposta != null
                && resposta.Length == 1
                && resposta.First().Equals(ResourceMensagensDeErro.USUARIO_NAO_ENCONTRADO)), default), Times.Once);
    }

    private static IGerarQRCodeUseCase GerarQRCodeUseCaseBuilder(string qrCode)
    {
        var useCaseMock = new Mock<IGerarQRCodeUseCase>();

        useCaseMock.Setup(c => c.Executar()).ReturnsAsync((qrCode, "IdUsuario"));

        return useCaseMock.Object;
    }

    private static IRecusarConexaoUseCase GerarConexaoRecusadaUseCaseBuilder()
    {
        var useCaseMock = new Mock<IRecusarConexaoUseCase>();

        useCaseMock.Setup(c => c.Executar()).ReturnsAsync("IdUsuario");

        return useCaseMock.Object;
    }

    private static IRecusarConexaoUseCase GerarConexaoRecusada_ErroDesconhecidoUseCaseBuilder()
    {
        var useCaseMock = new Mock<IRecusarConexaoUseCase>();

        useCaseMock.Setup(c => c.Executar()).ThrowsAsync(new ArgumentOutOfRangeException());

        return useCaseMock.Object;
    }
}
