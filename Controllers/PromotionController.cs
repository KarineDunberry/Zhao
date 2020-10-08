using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP1_KarineDunberry.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TP1_KarineDunberry.Models;
using System.Security.Cryptography.X509Certificates;

namespace TP1_KarineDunberry.Controllers
{
    public class PromotionController : Controller
    {
        public IActionResult Index()
        {
            return View(ObtenirListePromotions());
        }

        public IActionResult Details(int id)
        {
            return View(RetrouverPromotionParID(id));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Promotion promotion)
        {
            CreerPromotion(promotion); 
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            return View(RetrouverPromotionParID(id));
        }

        [HttpPost]
        public IActionResult Edit(Promotion promotion)
        {
            ModifierPromotion(promotion);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            return View(RetrouverPromotionParID(id));
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            SupprimerPromotion(id);

            return RedirectToAction("Index");
        }



        public List<Promotion> ObtenirListePromotions()
        {
            List<Promotion> promotions = HttpContext.Session.Get<List<Promotion>>("Promotions") ?? new List<Promotion>();

            if (promotions.Count == 0)
            {
                Promotion promotion1 = new Promotion
                {
                    IDPromotion = 1,
                    TypePromotion = TypePromotion.Comptoir,
                    TauxApplicable = 50,
                    Description = "2 repas pour 1",
                    DateDébut = new DateTime(2020, 10, 02),
                    DateFin = new DateTime(2020, 10, 12)
                };
                Promotion promotion2 = new Promotion
                {
                    IDPromotion = 2,
                    TypePromotion = TypePromotion.Livraison,
                    TauxApplicable = 10,
                    Description = "10% sur un repas en livraison",
                    DateDébut = new DateTime(2020, 10, 10),
                    DateFin = new DateTime(2020, 10, 20)
                };
                Promotion promotion3 = new Promotion
                {
                    IDPromotion = 3,
                    TypePromotion = TypePromotion.SalleAManger,
                    TauxApplicable = 50,
                    Description = "2 repas pour 1",
                    DateDébut = new DateTime(2020, 10, 13),
                    DateFin = new DateTime(2020, 10, 22)
                };
                Promotion promotion4 = new Promotion
                {
                    IDPromotion = 4,
                    TypePromotion = TypePromotion.Tous,
                    TauxApplicable = 10,
                    Description = "10% de rabais sur votre commande",
                    DateDébut = new DateTime(2020, 10, 12),
                    DateFin = new DateTime(2020, 10, 29)
                };

                promotions.Add(promotion1);
                promotions.Add(promotion2);
                promotions.Add(promotion3);
                promotions.Add(promotion4);
                HttpContext.Session.Set<List<Promotion>>("Promotions", promotions);
            }

            return promotions.OrderBy(x => x.IDPromotion).ToList();
        }

        public Promotion RetrouverPromotionParID(int ID)
        {
            List<Promotion> promotions = ObtenirListePromotions();

            Promotion promotion = promotions.Find(prom => prom.IDPromotion == ID);

            return promotion;
        }

        public void CreerPromotion(Promotion promotion)
        {
            List<Promotion> promotions = ObtenirListePromotions();
            promotion.IDPromotion = promotions.Max(promotion => promotion.IDPromotion) + 1;
            promotions.Add(promotion);
            HttpContext.Session.Set<List<Promotion>>("Promotions", promotions);
        }

        public void ModifierPromotion(Promotion promotion)
        {
            List<Promotion> promotions = ObtenirListePromotions();
            var promo = promotions.Where(p => p.IDPromotion == promotion.IDPromotion).FirstOrDefault();
            promotions.Remove(promo);
            promotions.Add(promotion);

            HttpContext.Session.Set<List<Promotion>>("Promotions", promotions);
        }

        public void SupprimerPromotion(int id)
        {
            List<Promotion> promotions = ObtenirListePromotions();
            promotions.RemoveAll(prom => prom.IDPromotion == id); 
            HttpContext.Session.Set<List<Promotion>>("Promotions", promotions);
        }
    }
}
