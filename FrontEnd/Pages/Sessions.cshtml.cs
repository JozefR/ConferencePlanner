﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferenceDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class SessionModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public SessionModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public SessionResponse Session { get; set; }

        public int? Dayoffset { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Session = await _apiClient.GetSessionAsync(id);

            if (Session == null)
            {
                return RedirectToPage("/Index");
            }

            var allSessions = await _apiClient.GetSessionsAsync();

            var startDate = allSessions.Min(s => s.StartTime?.Date);

            Dayoffset = Session.StartTime?.Subtract(startDate ?? DateTimeOffset.MinValue).Days;

            if (!string.IsNullOrEmpty(Session.Abstract))
            {
                Session.Abstract = "<p>" + String.Join("</p><p>", Session.Abstract.Split("\r\n", StringSplitOptions.RemoveEmptyEntries)) + "</p>";
            }

            return Page();
        }
    }
}