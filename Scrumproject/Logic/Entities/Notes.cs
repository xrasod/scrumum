using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Scrumproject.Logic.Entities.Notes))]

namespace Scrumproject.Logic.Entities
{
    public class Notes
    {
        public string Note { get; set; }
    }
}
