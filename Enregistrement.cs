
using Azure;
using System.Linq.Expressions;

namespace GestionBanque
{
    public class Enregistrement
    {

        public int Id { get; set; }
        public string Libelle { get; set; }
        public int Montant { get; set; }
        public int BankId { get; set; }
        
    }
}
