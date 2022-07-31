using MeuLivroDeReceitas.Api.WebSockets;
using MeuLivroDeReceitas.Application.UseCases.Conexao.GerarQRCode;
using MeuLivroDeReceitas.Application.UseCases.Conexao.QRCodeLido;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Moq;
using Utilitario.ParaOsTestes.Image;
using Utilitario.ParaOsTestes.Respostas;
using WebApi.Test.V1.Conexao.Builder;
using Xunit;

namespace WebApi.Test.V1.Conexao;
public class QRCodeLidoTeste
{
    [Fact]
    public async Task Validar_Sucesso()
    {
        var codigoGeradoParaConexao = Guid.NewGuid().ToString();
        var usuarioParaSeConectar = RespostaUsuarioConexaoBuilder.Construir();

        (var mockHubContext, var mockClientProxy, var mockClients, var mockHubContextCaller) = MockWebSocketConnectionsBuilder.Construir();

        var useCaseQRCodeLido = QRCodeLidoUseCaseBuilder(usuarioParaSeConectar, codigoGeradoParaConexao);
        var useCaseGerarQRCode = GerarQRCodeUseCaseBuilder();

        var hub = new AdicionarConexao(mockHubContext.Object, useCaseGerarQRCode, useCaseQRCodeLido, null, null)
        {
            Context = mockHubContextCaller.Object,
            Clients = mockClients.Object
        };

        await hub.GetQRCode();
        
        await hub.QRCodeLido(codigoGeradoParaConexao);

        mockClientProxy.Verify(
            clientProxy => clientProxy.SendCoreAsync("ResultadoQRCodeLido",
            It.Is<object[]>(resposta => resposta != null
                && resposta.Length == 1
                && (resposta.First() as RespostaUsuarioConexaoJson).Nome.Equals(usuarioParaSeConectar.Nome)
                && (resposta.First() as RespostaUsuarioConexaoJson).Id.Equals(usuarioParaSeConectar.Id)), default), Times.Once);
    }

    [Fact]
    public async Task Validar_Erro_Desconhecido()
    {
        var codigoGeradoParaConexao = Guid.NewGuid().ToString();
        var usuarioParaSeConectar = RespostaUsuarioConexaoBuilder.Construir();

        (var mockHubContext, var mockClientProxy, var mockClients, var mockHubContextCaller) = MockWebSocketConnectionsBuilder.Construir();

        var useCaseQRCodeLido = QRCodeLidoUseCase_ErroDesconhecidoBuilder(usuarioParaSeConectar, codigoGeradoParaConexao);
        var useCaseGerarQRCode = GerarQRCodeUseCaseBuilder();

        var hub = new AdicionarConexao(mockHubContext.Object, useCaseGerarQRCode, useCaseQRCodeLido, null, null)
        {
            Context = mockHubContextCaller.Object,
            Clients = mockClients.Object
        };

        await hub.GetQRCode();

        await hub.QRCodeLido(codigoGeradoParaConexao);

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

        var useCaseQRCodeLido = QRCodeLidoUseCase_ErroUsuarioNaoEncontradoBuilder(usuarioParaSeConectar, codigoGeradoParaConexao);

        var hub = new AdicionarConexao(mockHubContext.Object, null, useCaseQRCodeLido, null, null)
        {
            Context = mockHubContextCaller.Object,
            Clients = mockClients.Object
        };

        await hub.QRCodeLido(codigoGeradoParaConexao);

        mockClientProxy.Verify(
            clientProxy => clientProxy.SendCoreAsync("Erro",
            It.Is<object[]>(resposta => resposta != null
                && resposta.Length == 1
                && resposta.First().Equals(ResourceMensagensDeErro.USUARIO_NAO_ENCONTRADO)), default), Times.Once);
    }

    private static IQRCodeLidoUseCase QRCodeLidoUseCaseBuilder(RespostaUsuarioConexaoJson respostaJson, string qrCode)
    {
        var useCaseMock = new Mock<IQRCodeLidoUseCase>();

        useCaseMock.Setup(c => c.Executar(qrCode)).ReturnsAsync((respostaJson, "IdUsuario"));

        return useCaseMock.Object;
    }

    private static IQRCodeLidoUseCase QRCodeLidoUseCase_ErroDesconhecidoBuilder(RespostaUsuarioConexaoJson respostaJson, string qrCode)
    {
        var useCaseMock = new Mock<IQRCodeLidoUseCase>();

        useCaseMock.Setup(c => c.Executar(qrCode)).ThrowsAsync(new ArgumentOutOfRangeException(string.Empty));

        return useCaseMock.Object;
    }

    private static IQRCodeLidoUseCase QRCodeLidoUseCase_ErroUsuarioNaoEncontradoBuilder(RespostaUsuarioConexaoJson respostaJson, string qrCode)
    {
        var useCaseMock = new Mock<IQRCodeLidoUseCase>();

        useCaseMock.Setup(c => c.Executar(qrCode)).ReturnsAsync((respostaJson, "IdInvalido"));

        return useCaseMock.Object;
    }    

    private static IGerarQRCodeUseCase GerarQRCodeUseCaseBuilder()
    {
        var useCaseMock = new Mock<IGerarQRCodeUseCase>();

        useCaseMock.Setup(c => c.Executar()).ReturnsAsync((ImageBase64Builder.Construir(), "IdUsuario"));

        return useCaseMock.Object;
    }
}
