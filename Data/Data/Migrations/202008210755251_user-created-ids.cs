namespace ReadLater.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usercreatedids : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "UserCreatedId", c => c.Guid(nullable: false));
            AddColumn("dbo.Bookmarks", "UserCreatedId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookmarks", "UserCreatedId");
            DropColumn("dbo.Categories", "UserCreatedId");
        }
    }
}
