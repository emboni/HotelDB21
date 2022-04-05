using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageHotelApp.Exceptions;
using RazorPageHotelApp.Interfaces;
using RazorPageHotelApp.Models;

namespace RazorPageHotelApp.Pages.Hotels
{
    public class CreateModel : PageModel
    {
        private IHotelService hotelService;
        [BindProperty]
        public Hotel Hotel { get; set; }
        public string ErrorMsg { get; set; }
        public CreateModel(IHotelService hotelService)
        {
            this.hotelService = hotelService;
        }
        public void OnGet() {
        }
        public async Task<IActionResult> OnPost()
        {
            try
            {
                await hotelService.CreateHotelAsync(Hotel);
            }
            catch (DatabaseException ex)
            {
                ErrorMsg = ex.Message;
                return Page();
            }
            return RedirectToPage("/Hotels/GetAllHotels");
        }
    }
}
