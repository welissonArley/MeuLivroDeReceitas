using MeuLivroDeReceitas.Api.WebSockets;
using MeuLivroDeReceitas.Application.UseCases.Conexao.AceitarConexao;
using MeuLivroDeReceitas.Application.UseCases.Conexao.GerarQRCode;
using MeuLivroDeReceitas.Exceptions;
using Moq;
using Utilitario.ParaOsTestes.Image;
using Utilitario.ParaOsTestes.Respostas;
using WebApi.Test.V1.Conexao.Builder;
using Xunit;

namespace WebApi.Test.V1.Conexao;
public class ConexaoAceitaTeste
{
    [Fact]
    public async Task Validar_Sucesso()
    {
        var usuarioParaSeConectar = RespostaUsuarioConexaoBuilder.Construir();

        (var mockHubContext, var mockClientProxy, var mockClients, var mockHubContextCaller) = MockWebSocketConnectionsBuilder.Construir();

        var useCaseGerarQRCode = GerarQRCodeUseCaseBuilder();
        var useCaseConexaoAceita = GerarConexaoAceitaUseCaseBuilder(usuarioParaSeConectar.Id);

        var hub = new AdicionarConexao(mockHubContext.Object, useCaseGerarQRCode, null, null, useCaseConexaoAceita)
        {
            Context = mockHubContextCaller.Object,
            Clients = mockClients.Object
        };

        await hub.GetQRCode();

        await hub.AceitarConexao(usuarioParaSeConectar.Id);

        mockClientProxy.Verify(
            clientProxy => clientProxy.SendCoreAsync("OnConexaoAceita",
            It.Is<object[]>(resposta => resposta != null && resposta.Length == 0), default), Times.Once);
    }

    [Fact]
    public async Task Validar_Erro_Desconhecido()
    {
        var codigoGeradoParaConexao = Guid.NewGuid().ToString();
        var usuarioParaSeConectar = RespostaUsuarioConexaoBuilder.Construir();

        (var mockHubContext, var mockClientProxy, var mockClients, var mockHubContextCaller) = MockWebSocketConnectionsBuilder.Construir();

        var useCaseConexaoAceita = GerarConexaoAceitaUseCase_ErroDesconhecidoBuilder(usuarioParaSeConectar.Id);

        var hub = new AdicionarConexao(mockHubContext.Object, null, null, null, useCaseConexaoAceita)
        {
            Context = mockHubContextCaller.Object,
            Clients = mockClients.Object
        };

        await hub.AceitarConexao(usuarioParaSeConectar.Id);

        mockClientProxy.Verify(
            clientProxy => clientProxy.SendCoreAsync("Erro",
            It.Is<object[]>(resposta => resposta != null && resposta.Length == 1 && resposta.First().Equals(ResourceMensagensDeErro.ERRO_DESCONHECIDO)), default), Times.Once);
    }

    private static IAceitarConexaoUseCase GerarConexaoAceitaUseCase_ErroDesconhecidoBuilder(string idUsuarioParaSeConectar)
    {
        var useCaseMock = new Mock<IAceitarConexaoUseCase>();

        useCaseMock.Setup(c => c.Executar(idUsuarioParaSeConectar)).ThrowsAsync(new ArgumentOutOfRangeException(string.Empty));

        return useCaseMock.Object;
    }

    private static IAceitarConexaoUseCase GerarConexaoAceitaUseCaseBuilder(string idUsuarioParaSeConectar)
    {
        var useCaseMock = new Mock<IAceitarConexaoUseCase>();

        useCaseMock.Setup(c => c.Executar(idUsuarioParaSeConectar)).ReturnsAsync("IdUsuario");

        return useCaseMock.Object;
    }

    private static IGerarQRCodeUseCase GerarQRCodeUseCaseBuilder()
    {
        var useCaseMock = new Mock<IGerarQRCodeUseCase>();

        useCaseMock.Setup(c => c.Executar()).ReturnsAsync((ImageBase64Builder.Construir(), "IdUsuario"));

        return useCaseMock.Object;
    }
}
