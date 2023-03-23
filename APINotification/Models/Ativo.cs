using System;

namespace APINotification.Models
{
    public class Ativo
    {
        public int Id { get; set; }
        public string Nome_Ativo { get; set; }
        public int Dia_Ativo { get; set; }
        public DateTime Data_Ativo { get; set; }
        public Decimal Val_Abertura { get; set; }
        public Decimal Val_Fechamento { get; set; }
        public Decimal Variacao_Dia_Anterior { get; set; }
        public Decimal Variacao_Primeiro_Dia { get; set; }
        public Decimal Min_Dia { get; set; }
        public Decimal Max_Dia { get; set; }

    }
}
