using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maxsys.MediaManager.MusicContext.Infra.DataEFCore.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Composers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Composers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Catalogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicCatalogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MusicCatalogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Artists_MusicCatalogs_MusicCatalogId",
                        column: x => x.MusicCatalogId,
                        principalTable: "Catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlbumDirectory = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: true),
                    Genre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AlbumType = table.Column<byte>(type: "tinyint", nullable: false),
                    AlbumCover = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ArtistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Albums_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TrackNumber = table.Column<int>(type: "int", nullable: true),
                    Lyrics = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Classification_Rating = table.Column<int>(type: "int", nullable: true),
                    MusicDetails_IsBonusTrack = table.Column<bool>(type: "bit", nullable: true),
                    MusicDetails_VocalGender = table.Column<byte>(type: "tinyint", nullable: true),
                    MusicDetails_CoveredArtist = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MusicDetails_FeaturedArtist = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MusicProperties_Duration = table.Column<TimeSpan>(type: "time", nullable: true),
                    MusicProperties_BitRate = table.Column<int>(type: "int", nullable: true),
                    AlbumId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MediaFile_FullPath = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    MediaFile_OriginalFileName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MediaFile_FileSize = table.Column<long>(type: "bigint", nullable: false),
                    MediaFile_CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MediaFile_UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Musics_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComposerMusic",
                columns: table => new
                {
                    ComposersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MusicsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComposerMusic", x => new { x.ComposersId, x.MusicsId });
                    table.ForeignKey(
                        name: "FK_ComposerMusic_Composers_ComposersId",
                        column: x => x.ComposersId,
                        principalTable: "Composers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComposerMusic_Musics_MusicsId",
                        column: x => x.MusicsId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistItems",
                columns: table => new
                {
                    PlaylistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MusicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistItems", x => new { x.PlaylistId, x.MusicId });
                    table.ForeignKey(
                        name: "FK_PlaylistItems_Musics_MusicId",
                        column: x => x.MusicId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistItems_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "AK_Albums_ArtistAlbumName",
                table: "Albums",
                columns: new[] { "ArtistId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "AK_Artists_Name_Catalog",
                table: "Artists",
                columns: new[] { "Name", "CatalogId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Artists_MusicCatalogId",
                table: "Artists",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_ComposerMusic_MusicsId",
                table: "ComposerMusic",
                column: "MusicsId");

            migrationBuilder.CreateIndex(
                name: "AK_Composers_Name",
                table: "Composers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "AK_MusicCatalogs_Name",
                table: "Catalogs",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "AK_Musics_FullPath",
                table: "Songs",
                column: "MediaFile_FullPath",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Musics_AlbumId",
                table: "Songs",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistItems_MusicId",
                table: "PlaylistItems",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "AK_Playlists_Name",
                table: "Playlists",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComposerMusic");

            migrationBuilder.DropTable(
                name: "PlaylistItems");

            migrationBuilder.DropTable(
                name: "Composers");

            migrationBuilder.DropTable(
                name: "Songs");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Catalogs");
        }
    }
}
