﻿using Entities;
using MentorInterface.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Models
{
    public class UserIdentity
    {
        public int ApplicationUserId { get; set; }
        public long SteamId { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public int DailyMatchesLimit { get; set; }
    }
}
