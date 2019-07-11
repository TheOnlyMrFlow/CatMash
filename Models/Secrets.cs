using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CatMash.Models
{
    public class Secrets : ISecrets
    {
        public string AdminPassword { get; set; }
    }

    public interface ISecrets
    {
        string AdminPassword { get; set; }
    }
}


