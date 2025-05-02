using GolfProgressTracker.Core.Models;
using GolfProgressTracker.Core.ViewModels;
using GolfProgressTracker.Web.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace GolfProgressTracker.Web.Pages
{
    public class AddRoundModel(Context _context) : PageModel
    {
        [BindProperty]
        public RoundAndHolesViewModel AddRoundAndHolesViewModel { get; set; } = new();

        private static int HoleNumber = 1;

        public void OnGet()
        {
            HoleNumber = 1;
        }

        public void OnPostAddHole()
        {
            ModelState.Clear();

            AddRoundAndHolesViewModel.Holes.Add(new HoleViewModel
            {
                Number = HoleNumber
            });

            HoleNumber++;
        }

        public void OnPostAddHalfRound()
        {
            ModelState.Clear();

            var holeNum = HoleNumber + 9;

            for (; HoleNumber < holeNum; HoleNumber++)
            {
                AddRoundAndHolesViewModel.Holes.Add(new HoleViewModel
                {
                    Number = HoleNumber
                });
            }
        }

        public void OnPostAddFullRound()
        {
            ModelState.Clear();

            var holeNum = HoleNumber + 18;

            for (; HoleNumber < holeNum; HoleNumber++)
            {
                AddRoundAndHolesViewModel.Holes.Add(new HoleViewModel
                {
                    Number = HoleNumber
                });
            }
        }

        public void OnPostDeleteHole()
        {
            ModelState.Clear();

            if (AddRoundAndHolesViewModel.Holes.Count > 0)
                AddRoundAndHolesViewModel.Holes.RemoveAt(AddRoundAndHolesViewModel.Holes.Count - 1);

            HoleNumber--;
        }

        public void OnPostClearHoles()
        {
            ModelState.Clear();
            AddRoundAndHolesViewModel.Holes.Clear();

            HoleNumber = 1;
        }

        public IActionResult OnPostSubmitRound()
        {
            if (!ModelState.IsValid)
                return Page();

            using var transcation = _context.Database.BeginTransaction();
            
            try
            {
                var newRound = new Round
                {
                    Title = AddRoundAndHolesViewModel.Round.Title,
                    DatePlayed = AddRoundAndHolesViewModel.Round.DatePlayed.ToString("o", CultureInfo.InvariantCulture)
                };

                _context.Round.Add(newRound);
                _context.SaveChanges();

                var roundHoles = AddRoundAndHolesViewModel.Holes
                    .Select(r => new Hole
                    {
                        RoundId = newRound.Id,
                        Number = r.Number,
                        Par = (int)r.Par!,
                        Shots = (int)r.Shots!
                    });

                _context.Hole.AddRange(roundHoles);
                _context.SaveChanges();
                transcation.Commit();

                return Redirect("/Index");
            }
            catch
            {
                TempData["Error"] = "An error occurred saving the new round";
                transcation.Rollback();
                return Page();
            }
        }
    }
}
