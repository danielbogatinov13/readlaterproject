namespace ReadLater.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bookmarkclickedcount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookmarks", "ClickCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookmarks", "ClickCount");
        }
    }
}
