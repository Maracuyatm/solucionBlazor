namespace BlazorCrud.Server.Models
{
    public partial class Estado
    {
        public Guid Id { get; set; }  // Clave primaria GUID
        public string Nombre { get; set; } = null!;  // Nombre de la marca

        public DateTime CreatedAt { get; set; }  // Fecha de creación
        public DateTime? UpdatedAt { get; set; }  // Fecha de última actualización
        public DateTime? DeletedAt { get; set; }  // Fecha de eliminación (soft delete)
    }
}
