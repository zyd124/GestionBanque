using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionBanque
{
    public class Helpers
    {
        public Bank CreateBank(string bankName)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var newBank = new Bank(bankName);
                var existingBank = ctx.Banks.SingleOrDefault(b => b.Name == bankName);
                if (existingBank == null)
                {
                    ctx.Banks.Add(newBank);
                    ctx.SaveChanges();
                }

                return newBank;
            }
        }

        public int ReadInt(string message)
        {
            Console.Write(message);
            while (true)
            {
                string? input = Console.ReadLine();
                var isInt = int.TryParse(input, out var answer);
                if (isInt == false)
                {
                    Console.WriteLine("Saisie erronnée ,veuillez saisir un nombre !");
                }
                else
                {
                    return answer;
                }
            }
        }
        public int ReadIntBetween(string message, int min, int max)
        {
            Console.Write(message);
            while (true)
            {
                string? input = Console.ReadLine();
                var isInt = int.TryParse(input, out var answer);
                if (isInt == false)
                {
                    Console.WriteLine("Saisie erronnée, veuillez saisir un chiffre !");
                }
                else if (isInt == true && answer < min || answer > max)
                {
                    Console.WriteLine("Le chiffre entré n'est pas compris entre " + min + " et " + max);
                }
                else
                {
                    return answer;
                }
            }

        }
    }
}
