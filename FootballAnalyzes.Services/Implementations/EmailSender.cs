﻿namespace FootballAnalyzes.Services.Implementations
{
    using System.Threading.Tasks;
    
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }
    }
}
