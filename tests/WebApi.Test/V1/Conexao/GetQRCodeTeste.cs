using MeuLivroDeReceitas.Api.WebSockets;
using MeuLivroDeReceitas.Application.UseCases.Conexao.GerarQRCode;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Moq;
using Utilitario.ParaOsTestes.Image;
using WebApi.Test.V1.Conexao.Builder;
using Xunit;

namespace WebApi.Test.V1.Conexao;
public class GetQRCodeTeste
{
    [Fact]
    public async Task Validar_Sucesso()
    {
        (var mockHubContext, var mockClientProxy, var mockClients, var mockHubContextCaller) = MockWebSocketConnectionsBuilder.Construir();

        var useCaseGerarQRCode = GerarQRCodeUseCaseBuilder();

        var hub = new AdicionarConexao(mockHubContext.Object, useCaseGerarQRCode, null, null, null)
        {
            Context = mockHubContextCaller.Object,
            Clients = mockClients.Object
        };

        await hub.GetQRCode();

        mockClientProxy.Verify(
            clientProxy => clientProxy.SendCoreAsync("ResultadoQRCode",
            It.Is<object[]>(resposta => resposta != null && resposta.Length == 1 && resposta.First() is byte[]), default), Times.Once);
    }

    [Fact]
    public async Task Validar_Erro_Desconhecido()
    {
        (var mockHubContext, var mockClientProxy, var mockClients, var mockHubContextCaller) = MockWebSocketConnectionsBuilder.Construir();

        var useCaseGerarQRCode = GerarQRCodeUSeCase_ErroDesconhecidoBuilder();

        var hub = new AdicionarConexao(mockHubContext.Object, useCaseGerarQRCode, null, null, null)
        {
            Context = mockHubContextCaller.Object,
            Clients = mockClients.Object
        };

        await hub.GetQRCode();

        mockClientProxy.Verify(
            clientProxy => clientProxy.SendCoreAsync("Erro",
            It.Is<object[]>(resposta => resposta != null && resposta.Length == 1 && resposta.First().Equals(ResourceMensagensDeErro.ERRO_DESCONHECIDO)), default), Times.Once);
    }

    [Fact]
    public async Task Validar_Erro_MeuLivroReceitas()
    {
        var codigoGeradoParaConexao = Guid.NewGuid().ToString();

        (var mockHubContext, var mockClientProxy, var mockClients, var mockHubContextCaller) = MockWebSocketConnectionsBuilder.Construir();

        var useCaseGerarQRCode = GerarQRCodeUseCase_MeuLivroReceitasExceptionBuilder(ResourceMensagensDeErro.VOCE_NAO_PODE_EXECUTAR_ESTA_OPERACAO);

        var hub = new AdicionarConexao(mockHubContext.Object, useCaseGerarQRCode, null, null, null)
        {
            Context = mockHubContextCaller.Object,
            Clients = mockClients.Object
        };

        await hub.GetQRCode();

        mockClientProxy.Verify(
            clientProxy => clientProxy.SendCoreAsync("Erro",
            It.Is<object[]>(resposta => resposta != null && resposta.Length == 1 && resposta.First().Equals(ResourceMensagensDeErro.VOCE_NAO_PODE_EXECUTAR_ESTA_OPERACAO)), default), Times.Once);
    }

    private static IGerarQRCodeUseCase GerarQRCodeUseCaseBuilder()
    {
        var useCaseMock = new Mock<IGerarQRCodeUseCase>();

        useCaseMock.Setup(c => c.Executar()).ReturnsAsync((ImageBase64Builder.Construir(), "IdUsuario"));

        return useCaseMock.Object;
    }

    private static IGerarQRCodeUseCase GerarQRCodeUSeCase_ErroDesconhecidoBuilder()
    {
        var useCaseMock = new Mock<IGerarQRCodeUseCase>();

        useCaseMock.Setup(c => c.Executar()).ThrowsAsync(new ArgumentNullException());

        return useCaseMock.Object;
    }

    private static IGerarQRCodeUseCase GerarQRCodeUseCase_MeuLivroReceitasExceptionBuilder(string mensagemErro)
    {
        var useCaseMock = new Mock<IGerarQRCodeUseCase>();

        useCaseMock.Setup(c => c.Executar()).ThrowsAsync(new MeuLivroDeReceitasException(mensagemErro));

        return useCaseMock.Object;
    }
}
