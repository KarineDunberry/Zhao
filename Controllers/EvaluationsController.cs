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
    public class EvaluationsController : Controller
    {
        private readonly ZhaoContext _context;
        private readonly IAuthorizationService _authServ;

        public EvaluationsController(ZhaoContext context, IAuthorizationService authService)
        {
            _context = context;
            _authServ = authService;
        }

        // GET: Evaluations
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> IndexAdministrateur()
        {
            return View("Index", await _context.Evaluation.ToListAsync());
        }

        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> IndexAdministrateurFiltreRepas()
        {
            var evaluations = _context.Evaluation.OrderBy(eval => eval.QualitéRepas);
            return View("Index", await evaluations.ToListAsync());
        }

        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> IndexAdministrateurFiltreService()
        {
            var evaluations = _context.Evaluation.OrderBy(eval => eval.QualitéService);
            return View("Index", await evaluations.ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> IndexUtilisateurConnecte()
        {
            string email = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            return View("Index", await _context.Evaluation.Where(eval => eval.Courriel == email).ToListAsync());
        }

        // GET: Evaluations/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluation = await _context.Evaluation
                .FirstOrDefaultAsync(m => m.EvaluationID == id);
            if (evaluation == null)
            {
                return NotFound();
            }

            return View(evaluation);
        }

        // GET: Evaluations/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Evaluations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EvaluationID,Prénom,Nom,TypeReservation,Courriel,DateVisite,QualitéRepas,QualitéService,Commentaires")] Evaluation evaluation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(evaluation);
                await _context.SaveChangesAsync();
                SendEmail(evaluation);
                return RedirectToAction(nameof(IndexUtilisateurConnecte));
            }
            return View("Index", evaluation);
        }

        // GET: Evaluations/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluation = await _context.Evaluation.FindAsync(id);
            if (evaluation == null)
            {
                return NotFound();
            }
            return View(evaluation);
        }

        // POST: Evaluations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EvaluationID,Prénom,Nom,TypeReservation,Courriel,DateVisite,QualitéRepas,QualitéService,Commentaires")] Evaluation evaluation)
        {
            if (id != evaluation.EvaluationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evaluation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EvaluationExists(evaluation.EvaluationID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexUtilisateurConnecte));
            }
            return View("Index", evaluation);
        }

        // GET: Evaluations/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluation = await _context.Evaluation
                .FirstOrDefaultAsync(m => m.EvaluationID == id);
            if (evaluation == null)
            {
                return NotFound();
            }

            return View(evaluation);
        }

        // POST: Evaluations/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evaluation = await _context.Evaluation.FindAsync(id);
            _context.Evaluation.Remove(evaluation);
            await _context.SaveChangesAsync();
            if((await _authServ.AuthorizeAsync(User, "AdministrateurSeulement")).Succeeded)
            {
                return RedirectToAction(nameof(IndexAdministrateur));
            }
            else
            {
                return RedirectToAction(nameof(IndexUtilisateurConnecte));
            }
        }

        private bool EvaluationExists(int id)
        {
            return _context.Evaluation.Any(e => e.EvaluationID == id);
        }

        public void SendEmail(TP1_KarineDunberry.Models.Evaluation evaluation)
        {
            //Instanciation du client
            SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587);
            //On indique au client d'utiliser les informations qu'on va lui fournir
            smtpClient.UseDefaultCredentials = true;
            //Ajout des informations de connexion
            smtpClient.Credentials = new System.Net.NetworkCredential("courrielTI@cstjean.qc.ca", "");//username
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
