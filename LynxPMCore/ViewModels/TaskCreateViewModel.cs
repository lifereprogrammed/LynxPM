using LynxPMCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LynxPMCore.ViewModels
{
    public class TaskCreateViewModel
    {
        public Area Area { get; set; }
        public EquipmentArea EquipmentArea { get; set; }
        public Equipment Equipment { get; set; }
        public Term Term { get; set; }
        public TaskType TaskType { get; set; }
        public LTask LTask { get; set; }
    }
}
