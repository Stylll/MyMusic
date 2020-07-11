using Microsoft.EntityFrameworkCore.Migrations;

namespace MyMusic.Data.Migrations
{
    public partial class SeedMusicAndArtistTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .Sql("INSERT INTO Artist (Name) Values ('Mac Miller')");
            migrationBuilder
                .Sql("INSERT INTO Artist (Name) Values ('Alabama Shakes')");
            migrationBuilder
                .Sql("INSERT INTO Artist (Name) Values ('Bruno Mars')");
            migrationBuilder
                .Sql("INSERT INTO Artist (Name) Values ('Chance The Rapper')");

            migrationBuilder
                .Sql("INSERT INTO Music (Name, ArtistId) Values ('Circles', (SELECT Id FROM Artist WHERE Name = 'Mac Miller'))");
            migrationBuilder
                .Sql("INSERT INTO Music (Name, ArtistId) Values ('Complicated', (SELECT Id FROM Artist WHERE Name = 'Mac Miller'))");
            migrationBuilder
                .Sql("INSERT INTO Music (Name, ArtistId) Values ('Blue World', (SELECT Id FROM Artist WHERE Name = 'Mac Miller'))");
            migrationBuilder
                .Sql("INSERT INTO Music (Name, ArtistId) Values ('Sound and Color', (SELECT Id FROM Artist WHERE Name = 'Alabama Shakes'))");
            migrationBuilder
                .Sql("INSERT INTO Music (Name, ArtistId) Values ('Don''t wanna fight', (SELECT Id FROM Artist WHERE Name = 'Alabama Shakes'))");
            migrationBuilder
                .Sql("INSERT INTO Music (Name, ArtistId) Values ('Dunes', (SELECT Id FROM Artist WHERE Name = 'Alabama Shakes'))");
            migrationBuilder
                .Sql("INSERT INTO Music (Name, ArtistId) Values ('24K Magic', (SELECT Id FROM Artist WHERE Name = 'Bruno Mars'))");
            migrationBuilder
                .Sql("INSERT INTO Music (Name, ArtistId) Values ('Chunky', (SELECT Id FROM Artist WHERE Name = 'Bruno Mars'))");
            migrationBuilder
                .Sql("INSERT INTO Music (Name, ArtistId) Values ('Perm', (SELECT Id FROM Artist WHERE Name = 'Bruno Mars'))");
            migrationBuilder
                .Sql("INSERT INTO Music (Name, ArtistId) Values ('All We Got', (SELECT Id FROM Artist WHERE Name = 'Chance The Rapper'))");
            migrationBuilder
                .Sql("INSERT INTO Music (Name, ArtistId) Values ('No Problem', (SELECT Id FROM Artist WHERE Name = 'Chance The Rapper'))");
            migrationBuilder
                .Sql("INSERT INTO Music (Name, ArtistId) Values ('Summer Friends', (SELECT Id FROM Artist WHERE Name = 'Chance The Rapper'))");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .Sql("DELETE FROM Music");

            migrationBuilder
                .Sql("DELETE FROM Artist");
        }
    }
}
