using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ecobairroServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bairros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Pontuacao = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bairros", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Username = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fiscais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Rgf = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fiscais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fiscais_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Municipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Cpf = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Municipes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Pontuacao = table.Column<int>(type: "INTEGER", nullable: true),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true),
                    Latitude = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Longitude = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Categoria = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Endereco = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    FiscalId = table.Column<int>(type: "INTEGER", nullable: true),
                    MunicipeCriadorId = table.Column<int>(type: "INTEGER", nullable: false),
                    BairroId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pins_Bairros_BairroId",
                        column: x => x.BairroId,
                        principalTable: "Bairros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pins_Fiscais_FiscalId",
                        column: x => x.FiscalId,
                        principalTable: "Fiscais",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pins_Municipes_MunicipeCriadorId",
                        column: x => x.MunicipeCriadorId,
                        principalTable: "Municipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChamadasPin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MunicipeId = table.Column<int>(type: "INTEGER", nullable: false),
                    PinId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChamadasPin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChamadasPin_Municipes_MunicipeId",
                        column: x => x.MunicipeId,
                        principalTable: "Municipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChamadasPin_Pins_PinId",
                        column: x => x.PinId,
                        principalTable: "Pins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bairros_Nome",
                table: "Bairros",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChamadasPin_MunicipeId",
                table: "ChamadasPin",
                column: "MunicipeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChamadasPin_PinId",
                table: "ChamadasPin",
                column: "PinId");

            migrationBuilder.CreateIndex(
                name: "IX_Fiscais_Rgf",
                table: "Fiscais",
                column: "Rgf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fiscais_UserId",
                table: "Fiscais",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipes_Cpf",
                table: "Municipes",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Municipes_UserId",
                table: "Municipes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Pins_BairroId",
                table: "Pins",
                column: "BairroId");

            migrationBuilder.CreateIndex(
                name: "IX_Pins_FiscalId",
                table: "Pins",
                column: "FiscalId");

            migrationBuilder.CreateIndex(
                name: "IX_Pins_MunicipeCriadorId",
                table: "Pins",
                column: "MunicipeCriadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            // Populando banco
            migrationBuilder.InsertData(
                table: "Bairros",
                columns: new[] { "Id", "Nome", "Pontuacao" },
                values: new object[,]
                {
                    { 1, "Jundiapeba", 0 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Nome", "Username", "Password", "Email", "Role" },
                values: new object[,]
                {
                    { 1, "Municipe", "municipe", "123", "municipe@email.com", "Municipe" },
                    { 2, "Fiscal", "fiscal", "123", "fiscal@email.com", "Fiscal" },
                    { 3, "Ronan Benitis", "ronanbenitis", "123", "ronanbenitis@email.com", "Municipe" },
                    { 4, "Henrique Moura", "henriquemoura", "123", "henriquemoura@email.com", "Fiscal" }
                });

            migrationBuilder.InsertData(
                table: "Municipes",
                columns: new[] { "Id", "Cpf", "UserId" },
                values: new object[,]
                {
                    { 1, "CPF123", 1 },
                    { 2, "CPF234", 3 }
                });

            migrationBuilder.InsertData(
                table: "Fiscais",
                columns: new[] { "Id", "Rgf", "UserId" },
                values: new object[,]
                {
                    { 1, "RGF123", 2 },
                    { 2, "RGF234", 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChamadasPin");

            migrationBuilder.DropTable(
                name: "Pins");

            migrationBuilder.DropTable(
                name: "Bairros");

            migrationBuilder.DropTable(
                name: "Fiscais");

            migrationBuilder.DropTable(
                name: "Municipes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
