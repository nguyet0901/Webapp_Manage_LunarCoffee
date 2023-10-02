namespace LunarCoffee.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _456 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.tb_Product", new[] { "ProductCategoryID" });
            CreateIndex("dbo.tb_Product", "ProductCategoryId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.tb_Product", new[] { "ProductCategoryId" });
            CreateIndex("dbo.tb_Product", "ProductCategoryID");
        }
    }
}
