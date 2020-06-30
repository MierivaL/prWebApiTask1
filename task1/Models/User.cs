using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace task1
{
    [TableName("Users")]
    [PrimaryKey("Id")]
    public class User
    {
        public Guid Id { get; }
        public string Firstname { get; set; } // Имя
        public string Lastname { get; set; } // Фамилия
        public string Patronymic { get; set; } // Отчество
        public ulong PhoneNumber { get; set; } // Номер телефона
    }
}
