﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L072NS_HSZF_2024251.Console.UI
{
    public interface IExitable
    {
        event EventHandler? OnExiting;
    }
}
