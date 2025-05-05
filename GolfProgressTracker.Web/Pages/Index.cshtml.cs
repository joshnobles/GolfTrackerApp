using GolfProgressTracker.Core.ViewModels;
using GolfProgressTracker.Web.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GolfProgressTracker.Web.Pages;

public class IndexModel(Context context) : PageModel
{
    private readonly Context _context = context;


    public RoundListViewModel RoundListViewModel { get; set; } = new();

    public RoundAndHolesViewModel? RoundAndHolesViewModel { get; set; } = null;

    public void OnGet() 
    {
        RoundListViewModel.RoundViewModels = _context.Round
            .Select(r => new RoundViewModel
            {
                Id = r.Id,
                Title = r.Title,
                DatePlayed = DateOnly.Parse(r.DatePlayed)
            })
            .ToList()
            .OrderByDescending(r => r.DatePlayed)
            .ToList();
    }

    public void OnPostOpenRound([FromQuery] int roundId)
    {
        RoundAndHolesViewModel = _context.Round
            .Include(r => r.Holes)
            .Where(r => r.Id == roundId)
            .Select(r => new RoundAndHolesViewModel
            {
                Round = new RoundViewModel
                {
                    Id = r.Id,
                    Title = r.Title,
                    DatePlayed = DateOnly.Parse(r.DatePlayed),
                },
                Holes = r.Holes.Select(h => new HoleViewModel
                {
                    Number = h.Number,
                    Par = h.Par,
                    Shots = h.Shots
                }).ToList()
            })
            .FirstOrDefault();

        OnGet();
    }

    public void OnPostDeleteRound([FromQuery] int roundId)
    {
        var round = _context.Round
            .Include(r => r.Holes)
            .FirstOrDefault(r => r.Id == roundId);

        if (round is null)
        {
            TempData["Error"] = "Could not identify round to delete";
            OnGet();
            return;
        }

        _context.Round.Remove(round);
        _context.Hole.RemoveRange(round.Holes);        
        _context.SaveChanges();

        TempData["Success"] = "Successfully deleted round";

        OnGet();
    }
}
