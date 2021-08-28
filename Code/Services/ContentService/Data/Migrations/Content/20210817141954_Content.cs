using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContentService.Data.Migrations.Content
{
    public partial class Content : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Comments",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CommentText = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        LikesCount = table.Column<int>(type: "int", nullable: false),
            //        Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Comments", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Likes",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Likes", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PostImages",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        IsSoftDelete = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PostImages", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Posts",
            //    columns: table => new
            //    {
            //        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            //        UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        LikeCount = table.Column<int>(type: "int", nullable: false),
            //        CommentCount = table.Column<int>(type: "int", nullable: false),
            //        Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        IsSoftDelete = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Posts", x => x.Id);
            //    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "PostImages");

            migrationBuilder.DropTable(
                name: "Posts");
        }
    }
}
