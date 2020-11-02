using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP1_KarineDunberry.Models;

namespace TP1_KarineDunberry.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly ZhaoContext _context;
        private readonly IAuthorizationService _authServ;

        public ReservationsController(ZhaoContext context, IAuthorizationService authService)
        {
            _context = context;
            _authServ = authService;
        }

        // GET: Reservations
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> IndexAdministrateur()
        {
            return View("Index", await _context.Reservation.ToListAsync());
        }

        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> IndexAdministrateurConfirmee()
        {
            var reservations = _context.Reservation.Where(res => res.StatutReservation == StatutReservation.Confirmee);

            return View("Index", await reservations.ToListAsync());
        }

        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> IndexAdministrateurNonConfirmee()
        {
            var reservations = _context.Reservation.Where(res => res.StatutReservation == StatutReservation.NonConfirmee);

            return View("Index", await reservations.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> IndexUtilisateurConnecte()
        {
            string email = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            return View("Index", await _context.Reservation.Where(res => res.Courriel == email).ToListAsync());
        }

        // GET: Reservations/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .FirstOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReservationID,Prénom,Nom,TypeReservation,Courriel,DateHeure,Téléphone,nbPersonnes")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                SendEmail(reservation);
                return RedirectToAction(nameof(IndexUtilisateurConnecte));
            }
            return View("Index", reservation);
        }

        // GET: Reservations/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservationID,Prénom,Nom,TypeReservation,Courriel,DateHeure,Téléphone,nbPersonnes,StatutReservation")] Reservation reservation)
        {
            if (id != reservation.ReservationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ReservationID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if ((await _authServ.AuthorizeAsync(User, "AdministrateurSeulement")).Succeeded)
                {
                    return RedirectToAction(nameof(IndexAdministrateur));
                }
                else
                {
                    return RedirectToAction(nameof(IndexUtilisateurConnecte));
                }
            }
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .FirstOrDefaultAsync(m => m.ReservationID == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservation.FindAsync(id);
            _context.Reservation.Remove(reservation);
            await _context.SaveChangesAsync();
            if ((await _authServ.AuthorizeAsync(User, "AdministrateurSeulement")).Succeeded)
            {
                return RedirectToAction(nameof(IndexAdministrateur));
            }
            else
            {
                return RedirectToAction(nameof(IndexUtilisateurConnecte));
            }
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.ReservationID == id);
        }

        public void SendEmail(TP1_KarineDunberry.Models.Reservation reservation)
        {
            //Instanciation du client
            SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587);
            //On indique au client d'utiliser les informations qu'on va lui fournir
            smtpClient.UseDefaultCredentials = true;
            //Ajout des informations de connexion
            smtpClient.Credentials = new System.Net.NetworkCredential("courrielTI@cstjean.qc.ca", "");
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
