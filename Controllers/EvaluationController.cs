using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TP1_KarineDunberry.Models;

namespace TP1_KarineDunberry.Controllers
{
    public class EvaluationController : Controller
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
        public IActionResult Create(TP1_KarineDunberry.Models.Evaluation evaluation)
        {
            SendEmail(evaluation);
            return View("Details");
        }


        public void SendEmail(TP1_KarineDunberry.Models.Evaluation evaluation)
        {
            //Instanciation du client
            SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587);
            //On indique au client d'utiliser les informations qu'on va lui fournir
            smtpClient.UseDefaultCredentials = true;
            //Ajout des informations de connexion
            smtpClient.Credentials = new System.Net.NetworkCredential("courrielTI@cstjean.qc.ca", "");//username //mot de passe manquant pour confidentialité
            //On indique que l'on envoie le mail par le réseau
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //On active le protocole SSL
            smtpClient.EnableSsl = true;

            MailMessage mail = new MailMessage();
            //Expéditeur
            mail.From = new MailAddress("courrielTI@cstjean.qc.ca", "Restaurant Zhào");
            //Destinataire
            mail.To.Add(new MailAddress("dunberryk@gmail.com"));
            mail.Subject = "Evaluation d'une visite";
            mail.Body = $"Nom: {evaluation.Prénom} {evaluation.Nom}\n" +
                        $"Type de réservation: {evaluation.TypeReservation}\n" + 
                        $"Courriel: {evaluation.Courriel}\n" +
                        $"Date de la visite: {evaluation.DateVisite}\n" +
                        $"Qualité du repas: {evaluation.QualitéRepas}/5\n" +
                        $"Qualité du service: {evaluation.QualitéService}/5\n" +
                        $"Commentaires: {evaluation.Commentaires}";
            smtpClient.Send(mail);
        }
    }
}
