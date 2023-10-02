namespace LunarCoffee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updatedata12 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tb_Category", "Link");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_Category", "Link", c => c.String());
        }
    }
}
