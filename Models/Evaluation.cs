using Microsoft.AspNetCore.Components.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TP1_KarineDunberry.Models
{
    public class Evaluation
    {
        [DisplayName("ID")]
        public int EvaluationID { get; set; } //ajout de l'ID
        [MaxLength(100, ErrorMessage = "Le champ ne doit pas contenir plus de 100 caractères.")]
        [Required(ErrorMessage = "Le prénom est requis.")]
        public string Prénom { get; set; }
        [MaxLength(100, ErrorMessage = "Le champ ne doit pas contenir plus de 100 caractères.")]
        [Required(ErrorMessage = "Le nom est requis.")]
        public string Nom { get; set; }
        [DisplayName("Type de réservation")]
        [Required(ErrorMessage = "Le type de réservation est requis.")]
        public TypeReservation TypeReservation { get; set; }
        [EmailAddress(ErrorMessage = "Veuillez saisir une adresse courriel valide.")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Le format de l'adresse courriel est invalide.")]
        public string Courriel { get; set; }
        [DisplayName("Date de la visite")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La date de la visite est requise.")]
        public DateTime DateVisite { get; set; } 
        [Required(ErrorMessage = "L'évaluation est requise.")]
        [DisplayName("Qualité du repas")]
        [Range(1, 5, ErrorMessage = "Veuillez choisir une valeur entre 1 et 5.")]
        public int QualitéRepas { get; set; }
        [Required(ErrorMessage = "L'évaluation est requise.")]
        [DisplayName("Qualité du service")]
        [Range(1, 5, ErrorMessage = "Veuillez choisir une valeur entre 1 et 5.")]
        public int QualitéService { get; set; }
        public string Commentaires { get; set; } 
    }

 
}
