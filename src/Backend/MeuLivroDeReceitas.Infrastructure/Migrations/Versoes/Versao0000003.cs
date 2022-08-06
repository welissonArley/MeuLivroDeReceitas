using FluentMigrator;

namespace MeuLivroDeReceitas.Infrastructure.Migrations.Versoes;

[Migration((long)NumeroVersoes.AlterarTabelaReceitas, "Adicionando coluna Tempo para o preparo")]
public class Versao0000003 : Migration
{
    public override void Down()
    {
    }

    public override void Up()
    {
        Alter.Table("Receitas").AddColumn("TempoPreparo").AsInt32().NotNullable().WithDefaultValue(0);
    }
}
