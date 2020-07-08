using System;

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


