﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using TOT.Dto;
using TOT.Interfaces.Services;

namespace TOT.Business.Services {
    public class VacationEmailSender : IVacationEmailSender {
        private readonly IHttpContextAccessor _httpContext;
        private const string apiKey = "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";
        public VacationEmailSender(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        public async void ExecuteToManager(EmailModel model)
        {
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("apriorit-time-off-tracker@example.com", "TimeOffTracker System");
            var subject = "New request from the TimeOffTracker System";
            var to = new EmailAddress(model.To, "User");
            var plainTextContent = "Hello, this is plainTextContent!";
            var htmlContent = $"<strong>Hello, you have got new request to review</strong><p>Time Off Tracker: " +
                            $"{model.FullName} want to take a vacation</p><hr/>";
            htmlContent += String.IsNullOrEmpty(model.Body) ? "<p>This message doesn't have notes</p>"
                    : $"<p>{model.Body}</p>";
            var host = _httpContext.HttpContext.Request.Host;
            htmlContent += $"<a href='{host}/Manager/Index'>Follow the link</a>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            await client.SendEmailAsync(msg);
        }
        public async void ExecuteToEmployeeAccept(EmailModel model)
        {
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("apriorit-time-off-tracker@example.com", "TimeOffTracker System");
            var subject = "New response from the TimeOffTracker System";
            var to = new EmailAddress(model.To, "User");
            var plainTextContent = "Hello, this is plainTextContent!";
            var htmlContent = $"<strong>Dear {model.FullName} your request is accepted! </strong><hr/>";
            
            var host = _httpContext.HttpContext.Request.Host;
            htmlContent += $"<a href='{host}/Manager/Index'>Follow the link</a>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            await client.SendEmailAsync(msg);
        }
        public async void ExecuteToEmployeeDecline(EmailModel model)
        {
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("apriorit-time-off-tracker@example.com", "TimeOffTracker System");
            var subject = "New response from the TimeOffTracker System";
            var to = new EmailAddress(model.To, "User");
            var plainTextContent = "Hello, this is plainTextContent!";
            var htmlContent = $"<strong>Dear {model.FullName}, unfortunally your request is declined</strong><hr/>";
           
            var host = _httpContext.HttpContext.Request.Host;
            htmlContent += $"<a href='{host}/Manager/Index'>Follow the link</a>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            await client.SendEmailAsync(msg);
        }
    }
}