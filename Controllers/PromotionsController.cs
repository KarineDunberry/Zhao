using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TP1_KarineDunberry.Models;

namespace TP1_KarineDunberry.Controllers
{
    public class PromotionsController : Controller
    {
        private readonly ZhaoContext _context;

        public PromotionsController(ZhaoContext context)
        {
            _context = context;
        }

        // GET: Promotions
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> IndexAdministrateur()
        {
            return View("Index", await _context.Promotion.ToListAsync()); 
        }

        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> IndexAdministrateurComptoir()
        {
            var promotions = _context.Promotion.Where(prom => prom.TypePromotion == TypePromotion.Comptoir);
            return View("Index", await promotions.ToListAsync());
        }

        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> IndexAdministrateurSalleAManger()
        {
            var promotions = _context.Promotion.Where(prom => prom.TypePromotion == TypePromotion.SalleAManger);
            return View("Index", await promotions.ToListAsync());
        }

        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> IndexAdministrateurLivraison()
        {
            var promotions = _context.Promotion.Where(prom => prom.TypePromotion == TypePromotion.Livraison);
            return View("Index", await promotions.ToListAsync());
        }

     
        public async Task<IActionResult> IndexUtilisateur()
        {
            var promotions = _context.Promotion.Where(prom => prom.DateFin >= DateTime.Now || prom.DateFin == null);
            //or null??
            return View("Index", await promotions.ToListAsync());
        }


        public async Task<IActionResult> IndexUtilisateurComptoir()
        {
            var promotions = _context.Promotion.Where(prom => prom.DateFin >= DateTime.Now || prom.DateFin == null).Where(prom => prom.TypePromotion == TypePromotion.Comptoir);
            //or null??

            return View("Index", await promotions.ToListAsync());
        }


        public async Task<IActionResult> IndexUtilisateurSalleAManger()
        {
            var promotions = _context.Promotion.Where(prom => prom.DateFin >= DateTime.Now || prom.DateFin == null).Where(prom => prom.TypePromotion == TypePromotion.SalleAManger);
            //or null??

            return View("Index", await promotions.ToListAsync());
        }


        public async Task<IActionResult> IndexUtilisateurLivraison()
        {
            var promotions = _context.Promotion.Where(prom => prom.DateFin >= DateTime.Now || prom.DateFin == null).Where(prom => prom.TypePromotion == TypePromotion.Livraison);
            //or null??

            return View("Index", await promotions.ToListAsync());
        }

        // GET: Promotions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotion
                .FirstOrDefaultAsync(m => m.PromotionID == id);
            if (promotion == null)
            {
                return NotFound();
            }

            return View(promotion);
        }

        // GET: Promotions/Create
        [Authorize(Roles = "Administrateur")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Promotions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrateur")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PromotionID,TypePromotion,TauxApplicable,Description,DateDébut,DateFin")] Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(promotion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexAdministrateur));
            }
            return View(promotion);
        }

        // GET: Promotions/Edit/5
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotion.FindAsync(id);
            if (promotion == null)
            {
                return NotFound();
            }
            return View(promotion);
        }

        // POST: Promotions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrateur")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PromotionID,TypePromotion,TauxApplicable,Description,DateDébut,DateFin")] Promotion promotion)
        {
            if (id != promotion.PromotionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(promotion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PromotionExists(promotion.PromotionID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexAdministrateur));
            }
            return View(promotion);
        }

        // GET: Promotions/Delete/5
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotion
                .FirstOrDefaultAsync(m => m.PromotionID == id);
            if (promotion == null)
            {
                return NotFound();
            }

            return View(promotion);
        }

        // POST: Promotions/Delete/5
        [Authorize(Roles = "Administrateur")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var promotion = await _context.Promotion.FindAsync(id);
            _context.Promotion.Remove(promotion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexAdministrateur));
        }

        private bool PromotionExists(int id)
        {
            return _context.Promotion.Any(e => e.PromotionID == id);
        }
    }
}
