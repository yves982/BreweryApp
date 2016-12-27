using BreweryApp.Models;
using BreweryApp.ViewModels.Entities;
using System;

namespace BreweryApp.ViewModels
{
    /// <summary>
    /// Extension methods for ViewModel entities
    /// </summary>
    /// <remarks> Used especially for ViewModel - Model conversions
    /// Libraries, like Automapper can be used for such boilerplate code aswell</remarks>
    public static class VMExtensions
    {
        public static ESortDir ToSortDir(this ESortOrder sortOrder)
        {
            if(sortOrder == ESortOrder.None)
            {
                throw new InvalidOperationException("Cannot convert ESortOrder to ESortDir, if the member is ESortOrder.None");
            }
            ESortDir sortDir = sortOrder == ESortOrder.Asc ? ESortDir.Asc : ESortDir.Desc;
            return sortDir;
        }
    }
}
