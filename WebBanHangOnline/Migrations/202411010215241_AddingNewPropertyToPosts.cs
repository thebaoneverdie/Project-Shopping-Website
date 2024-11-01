namespace WebBanHangOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingNewPropertyToPosts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Posts", "SeoTitle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Posts", "SeoTitle");
        }
    }
}
