using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace task1
{
    [TableName("Appointment")]
    [PrimaryKey("Id")]
    public class Appointment
    {
        public Guid Id { get; }
        public DateTime Date { get; set; } // Дата обращения
        public Guid UserId { get; set; } // ID пользователя
        public string Comment { get; set; } // Комментарий
    }
}