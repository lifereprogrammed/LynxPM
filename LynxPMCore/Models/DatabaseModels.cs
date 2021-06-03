using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using LynxPMCore.ViewModels;

namespace LynxPMCore.Models
{
    public class DatabaseModels
    {

    }

    public class LynxPMContext : DbContext
    { 
        public LynxPMContext(DbContextOptions<LynxPMContext> options)
           : base(options)
        { }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<DueStatus> DueStatuses { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentArea> EquipmentAreas { get; set; }
        public DbSet<LTask> LTasks { get; set; }
        public DbSet<TaskTracker> TaskTrackers { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }
        public DbSet<Term> Terms { get; set; }
        public DbSet<TaskTrackerStage> TaskTrackerStages { get; set; }
        public DbSet<WorkGroup> WorkGroups { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<WorkOrderAssetList> WorkOrderAssetsLists { get; set; }

    }

    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        
    }

    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid CompanyID { get; set; }
        public virtual Company Company { get; set; }
        
    }

    public class Area
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AreaID { get; set; }
        [Required]
        public string AreaName { get; set; }
        public string AreaDescription { get; set; }
        public string AreaKMLFileURL { get; set; }
        [Required]
        public Boolean AreaActive { get; set; }
        public int AreaAppearanceOrder { get; set; }
        public Guid CompanyID { get; set; }
        public virtual Company Company { get; set; }
        public static IQueryable<Area> GetAreas()
        {
            return new List<Area>
            { }.AsQueryable();
        }

    }

    public class Condition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ConditionID { get; set; }
        public string ConditionName { get; set; }
        public string ConditionDescription { get; set; }
        public string ConditionDisplayLetter { get; set; }
    }

    public class DueStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid DueStatusID { get; set; }
        [Required]
        public string DueStatusName { get; set; }

        public string DustStatusDescription { get; set; }

    }

    public class Equipment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid EquipmentID { get; set; }
        [DisplayName("Equipment Name")]
        public string EquipmentName { get; set; }
        [DisplayName("Equipment Description")]
        public string EquipmentDescription { get; set; }

        public Guid? AreaID { get; set;  }
        [DisplayName("Area")]
        public virtual Area Area { get; set; }

        public Guid EquipmentAreaID { get; set; }
        [DisplayName("Equipment Area")]
        public virtual EquipmentArea EquipmentArea { get; set; }

        [DisplayName("Appearance Order")]
        public int EquipmentAppearance { get; set; }
        [DisplayName("Active Equipment")]
        public Boolean EquipmentActive { get; set; }
        public string EquipmentPictureID { get; set; }
        [DisplayName("Upload Picture")]
        public string EquipmentPictureURL { get; set; }
        //[NotMapped]
        //public HttpPostedFileBase EquipmentPicture { get; set; }

    }

    public class EquipmentArea
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid EquipmentAreaID { get; set; }
        [Required]
        public string EquipmentAreaName { get; set; }

        public string EquipmentAreaDescription { get; set; }

        public Guid AreaID { get; set; }
        public virtual Area Area { get; set; }

        public string EquipmentAreaAppearanceOrder { get; set; }

        public Boolean EquipmentAreaActive { get; set; }


    }
    
    public class LTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid LTaskID { get; set; }

        [Required]
        [DisplayName("Name")]
        public string LTaskName { get; set; }

        [DisplayName("Description")]
        public string LTaskDescription { get; set; }

        [DisplayName("Term")]
        public Guid TermID { get; set; }
        public virtual Term Term { get; set; }
        [DisplayName("Equipment")]
        public Guid EquipmentID { get; set; }
        public virtual Equipment Equipment { get; set; }
        [DisplayName("Task Type")]
        public Guid TaskTypeID { get; set; }
        public virtual TaskType TaskType { get; set; }
        
        
    }

    public class TaskTrackerStage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TaskTrackerStageID { get; set; }
        public string TaskTrackerStageName { get; set; }
        public string TaskTrackerStageDescription { get; set; }
    }

    public class TaskTracker
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TaskTrackerID { get; set; }
        public Guid TaskTrackerSessionID { get; set; }
        public Guid TaskTrackerStageID { get; set; }
        public DateTime TaskTrackerDateTime { get; set; }
        public virtual TaskTrackerStage TaskTrackerStage { get; set; }
        public Guid LTaskID { get; set; }
        public virtual LTask LTask { get; set; }
        public string TaskTrackerComments { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public String TaskTrackerCompletionDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public String TaskTrackerPreviousCompletionDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public String TaskTrackerExpectedCompletionDate { get; set; }

        public int TaskTrackerDaystoComplete { get; set; }

        public Guid? ConditionID { get; set; }
        public virtual Condition Condition { get; set; }

        public string TaskTrackerRecordUser { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public String TaskTrackerDateStamp { get; set; }

        public string TaskRecordUserIP { get; set; }
    }

    public class TaskType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TaskTypeID { get; set; }

        [Required]
        [DisplayName("Name")]
        public string TaskTypeName { get; set; }

        [DisplayName("Description")]
        public string TaskTypeDescription { get; set; }

        [Required]
        [StringLength(1, ErrorMessage = "The short name may not be more than {0} character(s)")]
        [DisplayName("Short Name / Display Letter")]
        public string TaskTypeDisplayLetter { get; set; }

    }

    public class Term
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TermID { get; set; }
        [Required]
        [DisplayName("Name")]
        public string TermName { get; set; }
        [DisplayName("Description")]
        public string TermDescription { get; set; }
        [DisplayName("Short Name")]
        [StringLength(2, ErrorMessage = "The Short Name may not be more than 2 characters")]
        public string TermShortName { get; set; }
        public int TermDays { get; set; }
        [DisplayName("Lead time (days)")]
        public int TermLeadTime { get; set; }

    }

    public class WorkGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid WorkGroupID { get; set; }
        public string WorkGroupName { get; set; }
        public string WorkGroupDrescription { get; set; }

    }

    public class TaskHazards
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TaskHazardID { get; set; }
        public string TaskHazardName { get; set; }
        public string TaskHazardDescription { get; set; }
        public Guid TaskHazardTypeID { get; set; }
        public virtual TaskHazardTypes TaskHazardTypes { get; set; }
    }

    public class TaskHazardTypes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TaskHazardTypeID { get; set; }
        public string TaskHazardTypeName { get; set; }
        public string TaskHazardDescription { get; set; }

    }

    public class WorkOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid WorkOrderID { get; set; }
        public string WorkOderName { get; set; }
        public string WorkOrderDescription { get; set; }
        public Guid CompanyID { get; set; }
        public virtual Company Company { get; set; }
        public Guid WorkOrderTypeID { get; set; }
        public virtual WorkOrderType WorkOrderType {get; set;}
    }

    public class WorkOrderType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid WorkOrderTypeID { get; set; }
        public string WorkOderTypeName { get; set; }
        public string WorkOrderTypeDescription { get; set; }
    }

    public class WorkOrderAssetList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid WorkOrderAssetListID { get; set;  }
        public Guid WorkOderID { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }
        public Guid EquipmentID { get; set; }
        public virtual Equipment Equipment { get; set; }
        public Guid LTaskID { get; set; }
        public virtual LTask LTask { get; set; }
    }

}
