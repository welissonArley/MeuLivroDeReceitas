using FluentMigrator;

namespace MeuLivroDeReceitas.Infrastructure.Migrations.Versoes;

[Migration((long)NumeroVersoes.CriarTabelasAssociacaoUsuario, "Adicionando tabelas para associação de usuários")]
public class Versao0000004 : Migration
{
    public override void Down()
    {
    }

    public override void Up()
    {
        CriarTabelaCodigo();
        CriarTabelaConexao();
    }

    private void CriarTabelaCodigo()
    {
        var tabela = VersaoBase.InserirColunasPadrao(Create.Table("Codigos"));

        tabela
            .WithColumn("Codigo").AsString(2000).NotNullable()
            .WithColumn("UsuarioId").AsInt64().NotNullable().ForeignKey("FK_Codigo_Usuario_Id", "Usuarios", "Id");
    }

    private void CriarTabelaConexao()
    {
        var tabela = VersaoBase.InserirColunasPadrao(Create.Table("Conexoes"));

        tabela
            .WithColumn("UsuarioId").AsInt64().NotNullable().ForeignKey("FK_Conexao_Usuario_Id", "Usuarios", "Id")
            .WithColumn("ConectadoComUsuarioId").AsInt64().NotNullable().ForeignKey("FK_Conexao_ConectadoComUsuario_Id", "Usuarios", "Id");
    }
}
