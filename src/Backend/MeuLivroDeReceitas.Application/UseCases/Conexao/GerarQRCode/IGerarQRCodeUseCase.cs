namespace MeuLivroDeReceitas.Application.UseCases.Conexao.GerarQRCode;
public interface IGerarQRCodeUseCase
{
    Task<(string qrCode, string idUsuario)> Executar();
}
