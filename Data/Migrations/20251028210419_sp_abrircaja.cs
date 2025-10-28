using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class sp_abrircaja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE PROCEDURE sp_abrircaja
    @usuario_id UNIQUEIDENTIFIER,
    @caja_id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE 
        @now DATETIME2(7) = SYSUTCDATETIME(),
        @vitacora_id UNIQUEIDENTIFIER = NEWID(),
        @estadoCerrado SMALLINT = 1,
        @estadoAbierto SMALLINT = 2;

    IF @usuario_id IS NULL OR @caja_id IS NULL
        THROW 50000, N'usuario_id y caja_id son requeridos.', 1;

    IF NOT EXISTS (SELECT 1 FROM usuarios WHERE id=@usuario_id AND activo=1 AND eliminado=0)
        THROW 50002, N'Usuario no encontrado o inactivo/eliminado.', 1;

    DECLARE @estado_id SMALLINT;
    SELECT @estado_id = estado_id FROM dbo.cajas WHERE id=@caja_id AND activo=1 AND eliminado=0;

    IF @estado_id IS NULL
        THROW 50003, N'Caja no encontrada o inactiva/eliminada.', 1;

    IF @estado_id <> @estadoCerrado
        THROW 50004, N'La caja no está en estado ""Cerrado"".', 1;

    IF EXISTS (SELECT 1 FROM dbo.caja_vitacora WHERE caja_id=@caja_id AND fecha_cierre IS NULL)
        THROW 50005, N'Existe una vitácora abierta para esta caja.', 1;

    BEGIN TRANSACTION;
        INSERT INTO dbo.caja_vitacora (id, usuario_id, caja_id, fecha_apertura, creado_en)
        VALUES (@vitacora_id, @usuario_id, @caja_id, @now, @now);

        UPDATE dbo.cajas 
        SET estado_id = @estadoAbierto, actualizado_en = @now
        WHERE id = @caja_id;
    COMMIT TRANSACTION;
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_abrircaja;");
        }
    }
}
