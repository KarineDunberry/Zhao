using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TP1_KarineDunberry.Controllers
{
    public class ReservationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(TP1_KarineDunberry.Models.Reservation reservation)
        {
            SendEmail(reservation);
            return View("Details");
        }

        public void SendEmail(TP1_KarineDunberry.Models.Reservation reservation)
        {
            //Instanciation du client
            SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587);
            //On indique au client d'utiliser les informations qu'on va lui fournir
            smtpClient.UseDefaultCredentials = true;
            //Ajout des informations de connexion
            smtpClient.Credentials = new System.Net.NetworkCredential("courrielTI@cstjean.qc.ca", "");//mot de passe manquant pour confidentialité
            //On indique que l'on envoie le mail par le réseau
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //On active le protocole SSL
            smtpClient.EnableSsl = true;

            MailMessage mail = new MailMessage();
            //Expéditeur
            mail.From = new MailAddress("courrielTI@cstjean.qc.ca", "Restaurant Zhào");
            //Destinataire
            mail.To.Add(new MailAddress("dunberryk@gmail.com"));
            mail.Subject = "Confirmation de réservation";
            mail.Body = $"Nom: {reservation.Prénom} {reservation.Nom}\n" +
                        $"Type de réservation: {reservation.TypeReservation}\n" + //comment avoir le display name?
                        $"Courriel: {reservation.Courriel}\n" +
                        $"Date et heure: {reservation.DateHeure}\n" +
                        $"Téléphone: {reservation.Téléphone}\n" +
                        $"Nombre de personnes: {reservation.nbPersonnes}";
            smtpClient.Send(mail);
        }
    }
}
