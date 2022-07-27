using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace MeuLivroDeReceitas.Api.WebSockets;

public class Broadcaster
{
    private readonly static Lazy<Broadcaster> _instance = new(() => new Broadcaster());

    public static Broadcaster Instance { get { return _instance.Value; } }

    private ConcurrentDictionary<string, object> _dictionary { get; set; }

    public Broadcaster()
    {
        _dictionary = new ConcurrentDictionary<string, object>();
    }

    public void InicializarConexao(IHubContext<AdicionarConexao> hubContext, string idUsuarioQueGerouQrCode, string connectionId)
    {
        var conexao = new Conexao(hubContext, connectionId);

        _dictionary.TryAdd(connectionId, conexao);
        _dictionary.TryAdd(idUsuarioQueGerouQrCode, connectionId);

        conexao.IniciarContagemTempo(CallbackTempoExpirado);
    }

    private void CallbackTempoExpirado(string connectionId)
    {
        _dictionary.TryRemove(connectionId, out _);
    }

    public string GetConnectionIdDoUsuario(string usuarioId)
    {
        if (!_dictionary.TryGetValue(usuarioId, out var connectionId))
        {
            throw new MeuLivroDeReceitasException(ResourceMensagensDeErro.USUARIO_NAO_ENCONTRADO);
        }

        return connectionId.ToString();
    }

    public void ResetarTempoExpiracao(string connectionId)
    {
        _dictionary.TryGetValue(connectionId, out var objetoConexao);

        var conexao = objetoConexao as Conexao;

        conexao.ResetarContagemTempo();
    }

    public void SetConnectionIdUsuarioLeitorQRCode(string idUsuarioQueGerouQRCode, string connectionIdUsarioLeitorQRCode)
    {
        var connectionIdUsuarioQueLeuQRCode = GetConnectionIdDoUsuario(idUsuarioQueGerouQRCode);

        _dictionary.TryGetValue(connectionIdUsuarioQueLeuQRCode, out var objetoConexao);

        var conexao = objetoConexao as Conexao;

        conexao.SetConnectionIdUsuarioLeitorQRCode(connectionIdUsarioLeitorQRCode);
    }

    public string Remover(string connectionId, string usuarioId)
    {
        if (!_dictionary.TryGetValue(connectionId, out var objetoConexao))
        {
            throw new MeuLivroDeReceitasException(ResourceMensagensDeErro.USUARIO_NAO_ENCONTRADO);
        }

        var conexao = objetoConexao as Conexao;

        conexao.StopTimer();

        _dictionary.TryRemove(connectionId, out _);
        _dictionary.TryRemove(usuarioId, out _);

        return conexao.UsuarioQueLeuQRCode();
    }
}
