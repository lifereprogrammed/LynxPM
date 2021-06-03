using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LynxPMCore.Models;

namespace LynxPMCore.ViewModels
{
    public class TaskTrackerVM
    {
        public List<TaskTracker> taskTracker { get; set; }
        public List<Area> area { get; set; }
        public List<EquipmentArea> equipmentArea { get; set; }
        public List<LTask> lTask { get; set; }
        public List<Equipment> equipment { get; set; }
        public List<Term> term { get; set; }
        public List<TaskType> taskType {get; set;}
        public List<Condition> condition { get; set; }

        public Guid TTguid { get; set; }
        public Guid LTguid { get; set; }
        public string LTName { get; set; }
        public string TTComments { get; set; }
        public DateTime TTCompDate { get; set; }
        public DateTime TTPrevDate { get; set; }
        public DateTime TTExpDate { get; set; }
        public int TTDays { get; set; }
        public Guid TTCondID { get; set; }
        public string TTRUser { get; set; }
        public DateTime TTDateStamp { get; set; }
        public string TTUserIP { get; set; }
        
    }

    public class TasksWithIDs
    {
        public Guid LTaskID { get; set; }
        public string LTaskName { get; set; }
        public Guid EquipmentID { get; set;  }
        public Guid EquipmentAreaID { get; set; }
        public Guid AreaID { get; set; }
    }


    public class EquipmentAreaStatusListVM
    {
        public string[,] equipSArray { get; set; }
        public string[,] equipAreaSArray { get; set; }
        public string[,] areaSArray { get; set; }
        public string[,] taskSArray { get; set; }
    }

    public class AreaStatusVM
    {
        public string AreaID { get; set; }
        public string AreaName { get; set; }
        public int TotalTasks { get; set; }
        public int TotalOverdue { get; set; }
        public int TotalInspection { get; set; }
        public int TotalAction { get; set; }
    }

    public class EquipmentAreaStatusVM
    {
        public string EquipmentAreaID { get; set; }
        public string EquipmentAreaName { get; set; }
        public string AreaID { get; set; }
        public string AreaName { get; set; }
        public int TotalTasks { get; set; }
        public int TotalOverdue { get; set; }
        public int TotalInspection { get; set; }
        public int TotalAction { get; set; }
    }

    public class EquipmentStatusVM
    {
        public string EquipmentID { get; set; }
        public string EquipmentName { get; set; }
        public string EquipmentAreaID { get; set; }
        public string EquipmentAreaName { get; set; }
        public int TotalTasks { get; set; }
        public int TotalOverdue { get; set; }
        public int TotalInspection { get; set; }
        public int TotalAction { get; set; }
    }

    public class TaskStatusVM
    {
        public string TaskID { get; set; }
        public string TaskName { get; set; }
        public string EquipmentID { get; set; }
        public string EquipmentAreaID { get; set; }
        public string AreaID { get; set; }
        public string TermID { get; set; }
        public string TaskTypeID { get; set; }
        public string TaskTypeLtr { get; set; }
        public string ConditionID { get; set; }
        public string ConditionLtr { get; set; }
        public bool Status { get; set; }
        public int TermDays { get; set; }
        public DateTime RecentCompletionDate { get; set; }
        public int DaysDue { get; set; }
    }


}
