﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASMSBusinessLayer.ViewModels;

namespace ASMSBusinessLayer.EmailService
{
    public interface IEmailSender
    {
        Task SendMessage(EmailMessage message);
    }
}