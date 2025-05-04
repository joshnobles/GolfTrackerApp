using GolfProgressTracker.Core.Models;
using GolfProgressTracker.Core.ViewModels;
using GolfProgressTracker.Web.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GolfProgressTracker.Web.Pages
{
    public class StatisticsModel(Context context) : PageModel
    {
        private readonly Context _context = context;


        public StatisticsViewModel StatisticsViewModel { get; set; } = null!;

        public void OnGet()
        {
            var rounds = _context.Round
                .Include(r => r.Holes)
                .ToList();

            StatisticsViewModel = ConvertToStatisticsViewModel(rounds);
        }

        public IActionResult OnGetStatisticsGraphData()
        {
            OnGet();
            return new JsonResult(StatisticsViewModel.RoundGraphData);
        }

        private StatisticsViewModel ConvertToStatisticsViewModel(List<Round> rounds)
        {
            var roundAndHolesViewModel = rounds
                .Select(r => new RoundAndHolesViewModel
                {
                    Round = new RoundViewModel
                    {
                        Id = r.Id,
                        Title = r.Title,
                        DatePlayed = DateOnly.Parse(r.DatePlayed)
                    },
                    Holes = r.Holes.Select(h => new HoleViewModel
                    {
                        Number = h.Number,
                        Par = h.Par,
                        Shots = h.Shots
                    }).ToList()
                })
                .ToList();

            return new StatisticsViewModel(roundAndHolesViewModel);
        }
    }
}
