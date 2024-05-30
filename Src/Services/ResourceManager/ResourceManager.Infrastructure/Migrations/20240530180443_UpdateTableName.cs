using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResourceManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shared_directory_DirectoryId",
                table: "Shared");

            migrationBuilder.DropPrimaryKey(
                name: "PK_recycle_Bin",
                table: "recycle_Bin");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shared",
                table: "Shared");

            migrationBuilder.RenameTable(
                name: "recycle_Bin",
                newName: "recycle_bin");

            migrationBuilder.RenameTable(
                name: "Shared",
                newName: "shared_directory");

            migrationBuilder.RenameIndex(
                name: "IX_Shared_DirectoryId",
                table: "shared_directory",
                newName: "IX_shared_directory_DirectoryId");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifiedBy",
                table: "resource_config",
                type: "char(255)",
                maxLength: 255,
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "char(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "resource_config",
                type: "char(255)",
                maxLength: 255,
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "char(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "resource_config",
                type: "char(255)",
                maxLength: 255,
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "char(255)",
                oldMaxLength: 255)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifiedBy",
                table: "file",
                type: "char(255)",
                maxLength: 255,
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "char(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "file",
                type: "char(255)",
                maxLength: 255,
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "char(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "file",
                type: "char(255)",
                maxLength: 255,
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "char(255)",
                oldMaxLength: 255)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "LastModifiedBy",
                table: "directory",
                type: "char(255)",
                maxLength: 255,
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "char(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "directory",
                type: "char(255)",
                maxLength: 255,
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "char(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "directory",
                type: "char(255)",
                maxLength: 255,
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(string),
                oldType: "char(255)",
                oldMaxLength: 255)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_recycle_bin",
                table: "recycle_bin",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_shared_directory",
                table: "shared_directory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_shared_directory_directory_DirectoryId",
                table: "shared_directory",
                column: "DirectoryId",
                principalTable: "directory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shared_directory_directory_DirectoryId",
                table: "shared_directory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_recycle_bin",
                table: "recycle_bin");

            migrationBuilder.DropPrimaryKey(
                name: "PK_shared_directory",
                table: "shared_directory");

            migrationBuilder.RenameTable(
                name: "recycle_bin",
                newName: "recycle_Bin");

            migrationBuilder.RenameTable(
                name: "shared_directory",
                newName: "Shared");

            migrationBuilder.RenameIndex(
                name: "IX_shared_directory_DirectoryId",
                table: "Shared",
                newName: "IX_Shared_DirectoryId");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedBy",
                table: "resource_config",
                type: "char(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "resource_config",
                type: "char(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "resource_config",
                type: "char(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(255)",
                oldMaxLength: 255)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedBy",
                table: "file",
                type: "char(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "file",
                type: "char(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "file",
                type: "char(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(255)",
                oldMaxLength: 255)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedBy",
                table: "directory",
                type: "char(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "directory",
                type: "char(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "directory",
                type: "char(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(255)",
                oldMaxLength: 255)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddPrimaryKey(
                name: "PK_recycle_Bin",
                table: "recycle_Bin",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shared",
                table: "Shared",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shared_directory_DirectoryId",
                table: "Shared",
                column: "DirectoryId",
                principalTable: "directory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
