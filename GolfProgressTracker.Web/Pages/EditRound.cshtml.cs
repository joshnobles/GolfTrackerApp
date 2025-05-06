using GolfProgressTracker.Core.Models;
using GolfProgressTracker.Core.ViewModels;
using GolfProgressTracker.Web.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace GolfProgressTracker.Web.Pages
{
    public class EditRoundModel(Context context) : PageModel
    {
        private readonly Context _context = context;

        [FromRoute]
        public int RoundId { get; set; }

        [BindProperty]
        public RoundAndHolesViewModel EditRoundAndHolesViewModel { get; set; } = null!;

        private static int HoleNumber;

        public IActionResult OnGet()
        {
            var roundAndHoles = _context.Round
                .Include(r => r.Holes)
                .FirstOrDefault(r => r.Id == RoundId);

            if (roundAndHoles is null)
                return Redirect("/");

            EditRoundAndHolesViewModel = ConvertToViewModel(roundAndHoles);

            HoleNumber = EditRoundAndHolesViewModel.Holes
                .Max(h => h.Number) + 1;

            return Page();
        }

        public void OnPostAddHole()
        {
            ModelState.Clear();

            EditRoundAndHolesViewModel.Holes
                .Add(new HoleViewModel
                {
                    Number = HoleNumber
                });

            HoleNumber++;
        }

        public void OnPostDeleteHole()
        {
            ModelState.Clear();

            if (EditRoundAndHolesViewModel.Holes.Count > 0)
                EditRoundAndHolesViewModel.Holes.RemoveAt(EditRoundAndHolesViewModel.Holes.Count - 1);

            HoleNumber--;
        }

        public IActionResult OnPostSaveChanges()
        {
            if (!ModelState.IsValid)
                return Page();

            var roundToUpdate = _context.Round
                .FirstOrDefault(r => r.Id == EditRoundAndHolesViewModel.Round.Id);

            if (roundToUpdate is null)
            {
                TempData["Error"] = "Could not identify round to update";
                return Page();
            }

            _context.Hole
                .RemoveRange(_context.Hole.Where(h => h.RoundId == roundToUpdate.Id));

            using var transcation = _context.Database.BeginTransaction();

            try
            {
                roundToUpdate.Title = EditRoundAndHolesViewModel.Round.Title;
                roundToUpdate.DatePlayed = EditRoundAndHolesViewModel.Round.DatePlayed.ToString("o", CultureInfo.InvariantCulture);

                var newHoles = EditRoundAndHolesViewModel.Holes
                    .Select(h => new Hole
                    {
                        RoundId = roundToUpdate.Id,
                        Number = h.Number,
                        Par = (int)h.Par!,
                        Shots = (int)h.Shots!
                    });

                _context.Hole.AddRange(newHoles);
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

        private static RoundAndHolesViewModel ConvertToViewModel(Round round)
        {
            return new RoundAndHolesViewModel
            {
                Round = new RoundViewModel
                {
                    Id = round.Id,
                    Title = round.Title,
                    DatePlayed = DateOnly.Parse(round.DatePlayed)
                },
                Holes = round.Holes.Select(h => new HoleViewModel
                {
                    Number = h.Number,
                    Par = h.Par,
                    Shots = h.Shots
                }).ToList()
            };
        }
    }
}
