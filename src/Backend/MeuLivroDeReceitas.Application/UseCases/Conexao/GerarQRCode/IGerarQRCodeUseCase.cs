using System.Drawing;

namespace MeuLivroDeReceitas.Application.UseCases.Conexao.GerarQRCode;
public interface IGerarQRCodeUseCase
{
    Task<(byte[] qrCode, string idUsuario)> Executar();
}
