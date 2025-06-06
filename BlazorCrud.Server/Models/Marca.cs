﻿using System;
using System.Collections.Generic;

namespace BlazorCrud.Server.Models
{
    public partial class Marca
    {
        public Guid Id { get; set; }  // Clave primaria GUID
        public Guid TipoActivoId { get; set; }
        public TipoActivo TipoActivo { get; set; } // ← navegación para el join
        public string Nombre { get; set; } = null!;  // Nombre de la marca

        public DateTime CreatedAt { get; set; }  // Fecha de creación
        public DateTime? UpdatedAt { get; set; }  // Fecha de última actualización
        public DateTime? DeletedAt { get; set; }  // Fecha de eliminación (soft delete)
        public short Estado { get; set; } = 1;

    }
}
