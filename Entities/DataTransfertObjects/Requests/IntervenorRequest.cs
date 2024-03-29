﻿using Entities.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.DataTransfertObjects
{
    public record IntervenorRequest
    {
        public Guid? Id { get; set; }
        [Display(Name = "Choisissez une image")]
        public IFormFile ImgFile { get; set; }
        [Display(Name = "Image")]
        public string ImgLink { get; set; }

        [Required, Display(Name = "Prénoms")]
        public string FirstName { get; set; }

        [Required, Display(Name = "Nom")]
        public string LastName { get; set; }

        [Required, Display(Name = "Compte bancaire")]
        public string BankAccount { get; set; }

        public string AppUserId { get; set; }
    }
}
