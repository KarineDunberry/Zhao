using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TP1_KarineDunberry.Models
{
    public class Reservation
    {
        [DisplayName("ID")]
        public int ReservationID { get; set; }
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
        [DisplayName("Date et heure")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy HH:mm}")]
        [Required(ErrorMessage = "La date et l'heure sont requis.")]
        public DateTime DateHeure { get; set; }
        [Phone(ErrorMessage = "Veuillez saisir un numéro de téléphone valide.")]
        [RegularExpression(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$", ErrorMessage = "Le format du numéro de téléphone est invalide.")]
        [Required(ErrorMessage = "Le numéro de téléphone est requis.")]
        public string Téléphone { get; set; }
        [DisplayName("Nombre de personnes")]
        [Range(1, 50, ErrorMessage = "Veuillez choisir une valeur entre 1 et 50.")]
        [Required(ErrorMessage = "Le nombre de personnes est requis.")]
        public int nbPersonnes { get; set; }
        [DisplayName("Statut de la réservation")]
        public StatutReservation StatutReservation { get; set; }

    }

    public enum TypeReservation
    {
        [Display(Name="Salle à manger")]
        SalleAManger,
        [Display(Name = "Salon privé (évènements)")]
        SalonPrive
    }

    public enum StatutReservation
    {
        [Display(Name = "Non confirmée")]
        NonConfirmee,
        [Display(Name = "Confirmée")]
        Confirmee


    }
}
