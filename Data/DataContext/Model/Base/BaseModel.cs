using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataContext.Model.Base
{
    public abstract class BaseModel
    {
        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        [Column("createDate")]
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Estado del registro
        /// </summary>
        [Column("status")]
        public bool Status { get; set; }

        /// <summary>
        /// Fecha de la última modificación
        /// </summary>
        [Column("updateDate")]
        public DateTime? ModificationDate { get; set; }

        /// <summary>
        /// Nombre del usuario que creó el registro
        /// </summary>
        [Column("username")]
        public string Username { get; set; }
    }
}
