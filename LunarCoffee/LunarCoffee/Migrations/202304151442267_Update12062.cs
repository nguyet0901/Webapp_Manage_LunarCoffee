namespace LunarCoffee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update12062 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Category", "Position", c => c.Int(nullable: false));
            AddColumn("dbo.tb_OrderDetail", "Quantity", c => c.Int(nullable: false));
            DropColumn("dbo.tb_Category", "Posiotion");
            DropColumn("dbo.tb_OrderDetail", "Quanlity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_OrderDetail", "Quanlity", c => c.Int(nullable: false));
            AddColumn("dbo.tb_Category", "Posiotion", c => c.Int(nullable: false));
            DropColumn("dbo.tb_OrderDetail", "Quantity");
            DropColumn("dbo.tb_Category", "Position");
        }
    }
}
