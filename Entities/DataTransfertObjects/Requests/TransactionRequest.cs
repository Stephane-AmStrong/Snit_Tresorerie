using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public class TransactionRequest
    {
        public Guid? Id { get; set; }
        [Required, Display(Name = "Libellé")]
        public string Name { get; set; }

        [Required, BindProperty, DataType(DataType.DateTime)]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required, Display(Name = "Réference")]
        public string Reference { get; set; }

        [Required, Display(Name = "Type : Achat ou vente")]
        public string Type { get; set; }

        [Required, Display(Name = "Montant hors taxes")]
        public int AmountBeforeTax { get; set; }

        [Required, Display(Name = "TVA")]
        public int VAT { get; set; }

        [Required, Display(Name = "TTC")]
        public int ATI { get; set; }

        [Required, Display(Name = "Choisissez le mode de paiement")]
        public Guid PaymentTypeId { get; set; }

        [Required, Display(Name = "Choisissez l'acteur")]
        public Guid ActorId { get; set; }
        public string AppUserId { get; set; }
        [Required, Display(Name = "Choisissez le site")]
        public Guid SiteId { get; set; }

    }
}
