using DungeonOT.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonOT.Data.Entities
{
    public class PlayerEntity
    {
        public int Id { get; set; }
        public int AccountId { get;set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
    }
}
