﻿using System;
using System.Collections.Generic;
using System.Linq;

using Umbraco.Core.Models;

namespace UmbracoVault
{
    internal class UmbracoContentModel
    {
        public IPublishedContent Content { get; internal set; }
    }
}