using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GestionBanque
{
    public class Menu
    {
        public Helpers Helpers = new();
        public Bank Bank;
        private void ChoixBanque()
        {
            Helpers.CreateBank("Société Générale");
            Helpers.CreateBank("CIH");
            Helpers.CreateBank("Banque Populaire");
            Console.WriteLine("Veuillez choisir votre banque :");
            using (var ctx = new ApplicationDbContext())
            {
                var banks = ctx.Banks;
                var lastId = banks.OrderByDescending(o => o.Id).FirstOrDefault()!.Id;
                var firstId = banks.OrderBy(o => o.Id).FirstOrDefault()!.Id;
                foreach (var bank in banks)
                {
                    Console.WriteLine(bank.Id + " - " + bank.Name);
                }
                var choix = Helpers.ReadIntBetween("Votre choix : ", firstId, lastId);
                Bank = banks.SingleOrDefault(o => o.Id == choix)!;
            }
        }

        private int MenuUtilisateur()
        {
            Console.WriteLine();
            Console.WriteLine("Bienvenue chez " + Bank.Name);
            Console.WriteLine("MENU PRINCIPAL");
            Console.WriteLine("Qu'est-ce que tu aimerais faire?");
            Console.WriteLine("Tapez 1 pour ajouter des enregistrements.");
            Console.WriteLine("Tapez 2 pour afficher tous les enregistrements.");
            Console.WriteLine("Tapez 3 pour supprimer les enregistrements.");
            Console.WriteLine("Tapez 4 pour mettre à jour les enregistrements.");
            Console.WriteLine("Tapez 5 pour rechoisir la banque.");
            Console.WriteLine("Tapez 5 pour quitter l'application.");
            var choix = Helpers.ReadIntBetween("Quel est votre choix? : ", 1, 5);
            return choix;
        }
        public void Initialize()
        {
            bool isInMenu = true;
            ChoixBanque();
            while (isInMenu)
            {
                var choix = MenuUtilisateur();
                switch (choix)
                {
                    case 1:
                        AjouterEnregistrement();
                        break;
                    case 2:
                        AfficherEnregistrements();
                        break;
                    case 3:
                        SupprimerEnregistrement();
                        break;
                    case 4:
                        UpdateEnregistrement();
                        break;
                    case 5:
                        Initialize();
                        break;
                    case 6:
                        isInMenu = false;
                        break;
                }
            }
        }

        private void AjouterEnregistrement()
        {
            Enregistrement newEnregistrement = new Enregistrement();
            Console.WriteLine("Veuillez entrer un libellé :");
            newEnregistrement.BankId = Bank.Id;
            newEnregistrement.Libelle = Console.ReadLine();
            newEnregistrement.Montant = Helpers.ReadInt("Veuillez entrer le montant : ");
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Enregistrements.Add(newEnregistrement);
                ctx.SaveChanges();
            }
            Console.WriteLine("Enregistrement effectué !");
        }
        private void AfficherEnregistrements()
        {
            using (var ctx = new ApplicationDbContext())
            {
                Console.WriteLine("Numéro - Libellé - Montant");
                var enregistrements = ctx.Enregistrements.Where(o=> o.BankId == Bank.Id);
                foreach (var enregistrement in enregistrements)
                {
                    Console.WriteLine(enregistrement.Id + " - " + enregistrement.Libelle + " - " + enregistrement.Montant);
                }
            }
        }
        private void SupprimerEnregistrement()
        {
            AfficherEnregistrements();
            using (var ctx = new ApplicationDbContext())
            {
                
                var last = ctx.Enregistrements.Where(o => o.BankId == Bank.Id).OrderByDescending(o => o.Id).FirstOrDefault();
                var first = ctx.Enregistrements.Where(o=>o.BankId == Bank.Id).OrderBy(o => o.Id).FirstOrDefault();
                if (last != null && first != null)
                {
                    var enregistrementId = Helpers.ReadIntBetween("Entrez le numéro de l'enregistrement que vous voulez supprimer : ", first.Id, last.Id);
                    var enregistrementToDelete = ctx.Enregistrements.SingleOrDefault(o => o.Id == enregistrementId);
                    if (enregistrementToDelete != null && enregistrementToDelete.BankId == Bank.Id)
                    {
                        ctx.Enregistrements.Where(o => o.Id == enregistrementId).ExecuteDelete();
                        ctx.SaveChanges();
                        Console.WriteLine("L'enregistrement numéro " + enregistrementId + " a été supprimé !");
                    }
                    else
                    {
                        Console.WriteLine("Vous ne pouvez pas supprimer un enregistrement non affiché dans le menu !");
                    }

                }
                else
                {
                    Console.WriteLine("Il n'y a pas d'enregistrements à afficher pour l'instant");
                }
            }


        }
        private void UpdateEnregistrement()
        {
            AfficherEnregistrements();
            using (var ctx = new ApplicationDbContext())
            {
                var last = ctx.Enregistrements.Where(o => o.BankId == Bank.Id).OrderByDescending(o => o.Id).FirstOrDefault();
                var first = ctx.Enregistrements.Where(o => o.BankId == Bank.Id).OrderBy(o => o.Id).FirstOrDefault();
                if (last != null && first != null)
                {
                    var enregistrementId =
                        Helpers.ReadIntBetween("Entrez le numéro de l'enregistrement que vous voulez modifier : ",
                            first.Id, last.Id);
                    var enregistrement = ctx.Enregistrements.SingleOrDefault(o => o.Id == enregistrementId);
                    if (enregistrement != null && enregistrement.BankId == Bank.Id)
                    {
                        Console.WriteLine("Veuillez entrer un libellé :");
                        enregistrement.Libelle = Console.ReadLine();
                        enregistrement.Montant = Helpers.ReadInt("Veuillez entrer le montant : ");
                        ctx.SaveChanges();
                        Console.WriteLine("L'enregistrement numéro " + enregistrementId + " a été modifié !");
                    }
                    else
                    {
                        Console.WriteLine("Vous ne pouvez pas modifier un enregistrement non affiché dans le menu !");
                    }
                }
            }
        }
    }
}
