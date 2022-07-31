using MeuLivroDeReceitas.Api.WebSockets;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace WebApi.Test.V1.Conexao.Builder;
public class MockWebSocketConnectionsBuilder
{
    public static (Mock<IHubContext<AdicionarConexao>> mockHubContext, Mock<IClientProxy> mockClientProxy, Mock<IHubCallerClients> mockClients, Mock<HubCallerContext> mockHubContextCaller) Construir()
    {
        var mockClientProxy = new Mock<IClientProxy>();

        var mockClients = new Mock<IHubCallerClients>();
        mockClients.Setup(c => c.Caller).Returns(mockClientProxy.Object);
        mockClients.Setup(c => c.Client(It.IsAny<string>())).Returns(mockClientProxy.Object);

        var mockHubContextCaller = new Mock<HubCallerContext>();
        mockHubContextCaller.Setup(c => c.ConnectionId).Returns(Guid.NewGuid().ToString());

        var mockHubClients = new Mock<IHubClients>();
        mockHubClients.Setup(c => c.Client(It.IsAny<string>())).Returns(mockClientProxy.Object);

        var mockHubContext = new Mock<IHubContext<AdicionarConexao>>();
        mockHubContext.Setup(c => c.Clients).Returns(mockHubClients.Object);

        return (mockHubContext, mockClientProxy, mockClients, mockHubContextCaller);
    }
}
