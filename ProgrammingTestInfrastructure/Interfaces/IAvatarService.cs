﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingTestInfrastructure.Interfaces
{
    public interface IAvatarService
    {
        string GetDicebearAvatarUrl(string identifier);

        string GetAvatarUrl(string identifier);
    }
}