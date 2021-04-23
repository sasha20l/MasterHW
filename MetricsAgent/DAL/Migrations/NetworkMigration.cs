
using FluentMigrator;

namespace MetricsAgent.DAL.Migrations
{

    [Migration(1)]
    public class NetworkMigration : Migration
    {
        public override void Up()
        {
            Create.Table("networkmetrics")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Value").AsInt32()
                .WithColumn("Time").AsInt64();
        }

        public override void Down()
        {
            Delete.Table("networkmetrics");
        }
    }
}
