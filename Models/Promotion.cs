using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TP1_KarineDunberry.Models
{
    public class Promotion
    {
        [DisplayName("ID")]
        [Required(ErrorMessage = "L'id de la promotion est requis.")]
        public int PromotionID { get; set; }
        [DisplayName("Type de promotion")]
        [Required(ErrorMessage = "Le type de promotion est requis.")]
        public TypePromotion TypePromotion { get; set; }
        [DisplayName("Taux applicable")]
        [DisplayFormat(DataFormatString = "{0:0}%")]
        [Range(0, 100, ErrorMessage = "Veuillez choisir une valeur entre 0 et 100.")]
        public int TauxApplicable { get; set; }
        [Required(ErrorMessage = "La description est requise.")]
        public string Description { get; set; }
        [DisplayName("Date de début")]
        [Required(ErrorMessage = "La date de début est requise.")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime DateDébut { get; set; }
        [DisplayName("Date de fin")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DateFin { get; set; }  //? veut dire que DateTime optionnelle, sinon le DateTime par défaut le met required

        internal int FindIndex(Func<object, bool> p)
        {
            throw new NotImplementedException();
        }
    }

    public enum TypePromotion
    {
        Comptoir,
        [Display(Name = "Salle à manger")]
        SalleAManger,
        Livraison,
        Tous
    }
}
