using System;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LynxPMCore.Models;
using LynxPMCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LynxPMCore.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using LynxPMCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LynxPMCore.Controllers
{
    public class TaskTrackerVMController : Controller
    {
        private readonly LynxPMContext _context;

        public TaskTrackerVMController(LynxPMContext context)
        {
            _context = context;
            
        }

        public string eaToday = DateTime.Now.ToString("yyyy/MM/dd");

        public Task<List<LTask>> GetLTasks()
        {
            return _context
                .LTasks
                .Include(lt => lt.Equipment)
                .Include(lt => lt.Term)
                .Include(lt => lt.TaskType)
                .ToListAsync();
        }

        public Task<List<Area>> GetAreas()
        {
            return _context.Areas.ToListAsync();
        }

        public Task<List<EquipmentArea>> GetEquipmentAreas() => _context.EquipmentAreas.Include(e => e.Area).ToListAsync();

        public Task<List<Term>> GetTerms() => _context.Terms.ToListAsync();

        public Task<List<Equipment>> GetEquipment()
        {
            
            return _context
                .Equipments
                .ToListAsync();
        }

        public Task<List<Condition>> GetCondition()
        {
            return _context
                .Conditions
                .ToListAsync();
        }

        public Task<List<TaskType>> GetTaskType()
        {
            return _context.TaskTypes.ToListAsync();
        }

        public Task<List<EquipmentArea>> GetEquipArea()
        {
            return _context.EquipmentAreas.ToListAsync();
        }
        
        public async Task<Guid> BuildTaskTracker (Guid TTGUID)
        {
                List<LTask> tlist = new List<LTask>();
                List<Term> TrmList = new List<Term>();
                List<TaskTracker> tTList = new List<TaskTracker>();
                tlist = _context.LTasks.ToList();
                TrmList = _context.Terms.ToList();
                tTList = _context.TaskTrackers.ToList();
                int j = tlist.Count;
                string[,] eXtras = new string[j, 10];
                int i = 0;
                string ipString = HttpContext.Connection.RemoteIpAddress.ToString();
                
                if (tTList.Count != 0)   //Data exists for the task tracker
                {
                    var result = tTList.OrderByDescending(x => x.TaskTrackerCompletionDate).First();
                    int k = 0;
                    
                    for (i = 0; i < j; i++)
                    {
                        eXtras[i, 0] = TTGUID.ToString();
                        eXtras[i, 1] = tlist[i].LTaskID.ToString();
                        //Look for the last completion date in the task tracker table for this task.
                        var temp = from t in tTList
                                   where t.LTaskID.ToString() == tlist[i].LTaskID.ToString()
                                   group t by t.LTaskID into tgrp
                                   let topp = tgrp.Max(x => x.TaskTrackerCompletionDate)
                                   select new
                                   {
                                       MostRecentID = tgrp.First(y => y.TaskTrackerCompletionDate == topp).TaskTrackerID,
                                       MostRecentTID = tgrp.First(y => y.TaskTrackerCompletionDate == topp).LTaskID,
                                       MostRecentDate = topp
                                   };
                        
                        foreach (var a in temp)
                        {
                            if (k == 0)
                            {
                                eXtras[i, 2] = a.MostRecentTID.ToString();
                                eXtras[i, 3] = Convert.ToDateTime(a.MostRecentDate).ToString();
                                eXtras[i, 4] = Convert.ToDateTime(a.MostRecentDate).AddDays(tlist[i].Term.TermDays).ToString();
                                k = k + 1;
                            };
                        }
                        if (eXtras[i, 2] == null)
                        {
                            _context.TaskTrackers.AddRange(new TaskTracker
                            {
                                TaskTrackerID = Guid.NewGuid(),
                                TaskTrackerSessionID = TTGUID,
                                TaskTrackerStageID = new Guid("bf0cc0de-f9c4-4a55-b9e2-b6c661ac5b22"), //Code Grey - User may have clicked on the Tracker by accident will update to green later.
                                LTaskID = tlist[i].LTaskID,
                                TaskTrackerDaystoComplete = tlist[i].Term.TermDays,
                                TaskTrackerPreviousCompletionDate = null,
                                TaskTrackerExpectedCompletionDate = null,
                                TaskTrackerDateStamp = DateTime.Now.ToString(),
                                TaskRecordUserIP = ipString
                            });
                        };
                        //Pull out the first record and put into eXtras array.
                        if (tlist[i].LTaskID.ToString() == eXtras[i, 2])
                        {
                            _context.TaskTrackers.AddRange(new TaskTracker
                            {
                                TaskTrackerID = Guid.NewGuid(),
                                TaskTrackerSessionID = TTGUID,
                                TaskTrackerStageID = new Guid("bf0cc0de-f9c4-4a55-b9e2-b6c661ac5b22"), //Code Grey - User may have clicked on the Tracker by accident will update to green later.
                                LTaskID = tlist[i].LTaskID,
                                TaskTrackerDaystoComplete = tlist[i].Term.TermDays, //ToDo perform calculation to display remaining days on view
                                TaskTrackerPreviousCompletionDate = eXtras[i, 3],
                                TaskTrackerExpectedCompletionDate = eXtras[i, 4],
                                TaskTrackerDateStamp = DateTime.Now.ToString(),
                                TaskRecordUserIP = ipString
                            });
                        };
                    };
                }
                // For when there is no previous data.
                else
                {
                    for (i = 0; i < j; i++)
                    {
                        if (ModelState.IsValid != true)
                        {
                            _context.TaskTrackers.AddRange(new TaskTracker
                            {
                                TaskTrackerID = Guid.NewGuid(),
                                TaskTrackerSessionID = TTGUID,
                                TaskTrackerDateTime = DateTime.Now,
                                TaskTrackerStageID = new Guid("bf0cc0de-f9c4-4a55-b9e2-b6c661ac5b22"), //Code Grey - User may have clicked on the Tracker by accident will update to green later.
                                LTaskID = tlist[i].LTaskID,
                                TaskTrackerDaystoComplete = tlist[i].Term.TermDays, //ToDo perform calculation to display remaining days on view
                                TaskTrackerPreviousCompletionDate = null,
                                TaskTrackerExpectedCompletionDate = null,
                                TaskTrackerDateStamp = DateTime.Now.ToString(),
                                TaskRecordUserIP = ipString
                            });
                        }
                    }
                }
                await _context.SaveChangesAsync();            
            return TTGUID;
        }
        
        public Task<List<TaskTracker>> GetTaskTracker()
        {
            return _context
                 .TaskTrackers
                 .ToListAsync();
        }

        public JsonResult getequipareabyID(Guid id)
        {
            List<EquipmentArea> list = new List<EquipmentArea>();
            list = _context.EquipmentAreas.Where(ea => ea.Area.AreaID == id).ToList();
            list.Insert(0, new EquipmentArea { EquipmentAreaID = Guid.NewGuid(), EquipmentAreaName = "Select" });
            return Json(new SelectList(list, "EquipmentAreaID", "EquipmentAreaName"));
        }

        public JsonResult getequiparea()
        {
            List<EquipmentArea> list = new List<EquipmentArea>();
            list = _context.EquipmentAreas.ToList();
            return Json(list);

        }


        
        // Create Index to display full list of tasks
        public async Task<IActionResult> Index(String id)
        {
            Guid TTGUID = Guid.NewGuid();

            ViewData["GID"] = id;
            ViewData["AreaID"] = new SelectList(_context.Areas, "AreaID", "AreaName");
            ViewData["EquipmentAreaID"] = new SelectList(_context.EquipmentAreas, "EquipmentAreaID", "EquipmentAreaName");

            var tracker = await BuildTaskTracker(TTGUID);
            
                var viewModel = new TaskTrackerVM
                {
                    equipment = await GetEquipment(),
                    lTask = await GetLTasks(),
                    condition = await GetCondition(),
                    taskTracker = await GetTaskTracker(),
                    area = await GetAreas(),
                    equipmentArea = await GetEquipArea()

                };
            
            return View(viewModel);
        }

        public async Task<IActionResult> TaskStatus()
        {
            var viewModel = GetEAreaStatusList(await GetLTasks(), await GetTerms(), await GetTaskTracker(), await GetEquipmentAreas(), await GetEquipment(), await GetAreas());

            return View(viewModel);
        }

        public async Task<IActionResult> Status()
        {
            List<AreaStatusVM> AreaStatusList = await GenerateAreaStatusList();
            return View(AreaStatusList);
        }

        public async Task<IActionResult> EquipAreaStatus(string id)
        {
            List<EquipmentAreaStatusVM> EquipAreaStatusList = await GenerateEquipmentAreaStatusList();
            List<EquipmentAreaStatusVM> FilteredAreaStatusList = new List<EquipmentAreaStatusVM>();
            int i;
            for (i = 0; i < EquipAreaStatusList.Count(); i++)
            {
                if (EquipAreaStatusList[i].AreaID == id)
                {
                    FilteredAreaStatusList.Add(new EquipmentAreaStatusVM
                    {
                        AreaID = EquipAreaStatusList[i].AreaID,
                        AreaName = EquipAreaStatusList[i].AreaName,
                        EquipmentAreaID = EquipAreaStatusList[i].EquipmentAreaID,
                        EquipmentAreaName = EquipAreaStatusList[i].EquipmentAreaName,
                        TotalTasks = EquipAreaStatusList[i].TotalTasks,
                        TotalOverdue = EquipAreaStatusList[i].TotalOverdue,
                        TotalAction = EquipAreaStatusList[i].TotalAction,
                        TotalInspection = EquipAreaStatusList[i].TotalInspection
                    });
                }
            }
            return View(FilteredAreaStatusList);
        }

        public List<TaskStatusVM> tStatusList = new List<TaskStatusVM>();
        public List<EquipmentStatusVM> tEquipStatusList = new List<EquipmentStatusVM>();
        public List<EquipmentAreaStatusVM> tEquipmentAreaStatusList = new List<EquipmentAreaStatusVM>();
        public List<AreaStatusVM> tAreaStatus = new List<AreaStatusVM>();

        public async Task<List<TaskStatusVM>> GeneratetStatusList()
        {
            List<LTask> tList = await GetLTasks();
            List<Term> TrmList = await GetTerms();
            List<TaskType> tTypeList = await GetTaskType();
            List<TaskTracker> tTList = await GetTaskTracker();
            List<Area> tArea = await GetAreas();
            List<EquipmentArea> tEquipArea = await GetEquipmentAreas();
            List<Condition> tConditions = await GetCondition();
            int i, j, k, z;

            string tmpAreaID = "";
            string tmpConditionID = "";
            string tmpConditionLtr = "";
            bool tmpStatus = true;
            int tmpTrmDays = 0;
            DateTime tmpDate = Convert.ToDateTime(eaToday);
            int tmpDaysDue;

            for (i = 0; i < tList.Count(); i++)
            {
                k = 0;
                var temp = from t in tTList
                           where t.LTaskID.ToString() == tList[i].LTaskID.ToString()
                           group t by t.LTaskID into tgrp
                           let topp = tgrp.Max(x => x.TaskTrackerCompletionDate)
                           select new
                           {
                               MostRecentID = tgrp.First(y => y.TaskTrackerCompletionDate == topp).TaskTrackerID,
                               MostRecentTID = tgrp.First(y => y.TaskTrackerCompletionDate == topp).LTaskID,
                               MostRecentDate = topp,
                               MostRecentConditionID = tgrp.First(y => y.TaskTrackerCompletionDate == topp).ConditionID,
                           };
                foreach (var a in temp)
                {
                    if (k == 0)
                    {
                        tmpConditionID = a.MostRecentConditionID.ToString();
                        tmpDate = Convert.ToDateTime(a.MostRecentDate);
                    }
                }

                for (z = 0; z < tConditions.Count; z++)
                {
                    if (tmpConditionID == tConditions[z].ConditionID.ToString())
                    {
                        tmpConditionLtr = tConditions[z].ConditionDisplayLetter;
                    }
                }

                tmpTrmDays = tList[i].Term.TermDays;
                tmpDaysDue = (Convert.ToInt32(tmpDate.ToOADate()) + tmpTrmDays) - Convert.ToInt32(Convert.ToDateTime(eaToday).ToOADate());
                if (tmpDaysDue < 0)
                {
                    tmpStatus = false; // False = Overdue
                }
                else
                {
                    tmpStatus = true;
                }

                for (z = 0; z < tEquipArea.Count; z++)
                {
                    if (tEquipArea[z].EquipmentAreaID.ToString() == tList[i].Equipment.EquipmentAreaID.ToString())
                    {
                        for (j = 0; j < tArea.Count(); j++)
                        {
                            if (tEquipArea[z].AreaID.ToString() == tArea[j].AreaID.ToString())
                            {
                                tmpAreaID = tArea[j].AreaID.ToString();
                            }
                        }
                    }
                }

                tStatusList.Add(new TaskStatusVM()
                {
                    TaskID = tList[i].LTaskID.ToString(),
                    TaskName = tList[i].LTaskName.ToString(),
                    EquipmentID = tList[i].EquipmentID.ToString(),
                    EquipmentAreaID = tList[i].Equipment.EquipmentAreaID.ToString(),
                    AreaID = tmpAreaID,
                    TermID = tList[i].TermID.ToString(),
                    TaskTypeID = tList[i].TaskTypeID.ToString(),
                    TaskTypeLtr = tList[i].TaskType.TaskTypeDisplayLetter.ToString(),
                    ConditionID = tmpConditionID,
                    ConditionLtr = tmpConditionLtr,
                    Status = tmpStatus,
                    TermDays = tmpTrmDays,
                    RecentCompletionDate = tmpDate,
                    DaysDue = tmpDaysDue
                });

            }
            return tStatusList;
        }

        public async Task<JsonResult> GetTaskStatus()
        {
            List<TaskStatusVM> tStatusList = await GeneratetStatusList();

            return Json(tStatusList);
        }

        public async Task<List<EquipmentStatusVM>> GenerateEquipmentStatusList()
        {
            List<Equipment> tEquipList = await GetEquipment();
            List<EquipmentArea> tEAList = await GetEquipmentAreas();
            List<TaskStatusVM> tStatusList = await GeneratetStatusList();

            int i = 0, j = 0, x = 0, y = 0;
            int tmpTaskCounter = 0, tmpOverdueCounter = 0, tmpInspItemCounter = 0, tmpActItemCounter = 0;

            for (i = 0; i < tEquipList.Count(); i++)
            {
                for (j = 0; j < tStatusList.Count(); j++)
                {
                    if (tStatusList[j].TaskTypeLtr != "N")
                    {
                        if (tStatusList[j].EquipmentID.ToString() == tEquipList[i].EquipmentID.ToString())
                        {
                            tmpTaskCounter = tmpTaskCounter + 1;
                            if (tStatusList[j].Status == false)
                            {
                                tmpOverdueCounter = tmpOverdueCounter + 1;
                            }
                            if (tStatusList[j].TaskTypeLtr == "I")
                            {
                                tmpInspItemCounter = tmpInspItemCounter + 1;
                            }
                            else if (tStatusList[j].TaskTypeLtr == "A")
                            {
                                tmpActItemCounter = tmpActItemCounter + 1;
                            }
                        }
                    }
                }

                tEquipStatusList.Add(new EquipmentStatusVM()
                {
                    EquipmentID = tEquipList[i].EquipmentID.ToString(),
                    EquipmentName = tEquipList[i].EquipmentName,
                    EquipmentAreaID = tEquipList[i].EquipmentAreaID.ToString(),
                    EquipmentAreaName = tEquipList[i].EquipmentName,
                    TotalTasks = tmpTaskCounter,
                    TotalOverdue = tmpOverdueCounter,
                    TotalInspection = tmpInspItemCounter,
                    TotalAction = tmpActItemCounter
                });

                tmpTaskCounter = 0;
                tmpOverdueCounter = 0;
                tmpInspItemCounter = 0;
                tmpActItemCounter = 0;
            }
            return tEquipStatusList;
        }

        public async Task<JsonResult> GetEquipStatus(string id)
        {
            List<EquipmentStatusVM> tEquipStatusList = await GenerateEquipmentStatusList();
            int i;
            List<EquipmentStatusVM> FilteredEquipmentStatusList = new List<EquipmentStatusVM>();
            for (i=0; i< tEquipStatusList.Count(); i++)
            {
                if(tEquipStatusList[i].EquipmentAreaID == id)
                {
                    FilteredEquipmentStatusList.Add(new EquipmentStatusVM
                    {
                        EquipmentAreaID = tEquipStatusList[i].EquipmentAreaID,
                        EquipmentAreaName = tEquipStatusList[i].EquipmentAreaName,
                        EquipmentID = tEquipStatusList[i].EquipmentID,
                        EquipmentName = tEquipStatusList[i].EquipmentName,
                        TotalAction = tEquipStatusList[i].TotalAction,
                        TotalInspection = tEquipStatusList[i].TotalInspection,
                        TotalOverdue = tEquipStatusList[i].TotalOverdue,
                        TotalTasks = tEquipStatusList[i].TotalTasks
                    });
                }
            }
            return Json(FilteredEquipmentStatusList);
        }

        public async Task<List<EquipmentAreaStatusVM>> GenerateEquipmentAreaStatusList()
        {
            List<EquipmentArea> tEquipAreaList = await GetEquipmentAreas();
            List<EquipmentStatusVM> tEquipStatusList = await GenerateEquipmentStatusList();

            int i = 0, j = 0, x = 0;
            int tmpTaskCounter = 0, tmpOverdueCounter = 0, tmpInspItemCounter = 0, tmpActItemCounter = 0;

            for (i = 0; i < tEquipAreaList.Count(); i++)
            {
                for (j = 0; j < tEquipStatusList.Count(); j++)
                {
                    if (tEquipStatusList[j].EquipmentAreaID.ToString() == tEquipAreaList[i].EquipmentAreaID.ToString())
                    {
                        tmpTaskCounter = tmpTaskCounter + tEquipStatusList[j].TotalTasks;
                        tmpOverdueCounter = tmpOverdueCounter + tEquipStatusList[j].TotalOverdue;
                        tmpInspItemCounter = tmpInspItemCounter + tEquipStatusList[j].TotalInspection;
                        tmpActItemCounter = tmpActItemCounter + tEquipStatusList[j].TotalAction;
                    }
                }

                tEquipmentAreaStatusList.Add(new EquipmentAreaStatusVM()
                {
                    EquipmentAreaID = tEquipAreaList[i].EquipmentAreaID.ToString(),
                    EquipmentAreaName = tEquipAreaList[i].EquipmentAreaName,
                    AreaID = tEquipAreaList[i].AreaID.ToString(),
                    AreaName = tEquipAreaList[i].Area.AreaName,
                    TotalTasks = tmpTaskCounter,
                    TotalOverdue = tmpOverdueCounter,
                    TotalInspection = tmpInspItemCounter,
                    TotalAction = tmpActItemCounter
                });
                tmpTaskCounter = 0;
                tmpOverdueCounter = 0;
                tmpInspItemCounter = 0;
                tmpActItemCounter = 0;
            }
            return tEquipmentAreaStatusList;
        }

        public async Task<JsonResult> GetEquipAreaStatus(string id)
        {

            List<EquipmentAreaStatusVM> EquipAreaStatusList = await GenerateEquipmentAreaStatusList();
            int i;
            List<EquipmentAreaStatusVM> FilteredAreaStatusList = new List<EquipmentAreaStatusVM>();
            for (i = 0; i < EquipAreaStatusList.Count(); i++)
            {
                if (EquipAreaStatusList[i].AreaID == id)
                {
                    FilteredAreaStatusList.Add(new EquipmentAreaStatusVM
                    {
                        AreaID = EquipAreaStatusList[i].AreaID,
                        AreaName = EquipAreaStatusList[i].AreaName,
                        EquipmentAreaID = EquipAreaStatusList[i].EquipmentAreaID,
                        EquipmentAreaName = EquipAreaStatusList[i].EquipmentAreaName,
                        TotalTasks = EquipAreaStatusList[i].TotalTasks,
                        TotalOverdue = EquipAreaStatusList[i].TotalOverdue,
                        TotalAction = EquipAreaStatusList[i].TotalAction,
                        TotalInspection = EquipAreaStatusList[i].TotalInspection
                    });
                }
            }
            return Json(FilteredAreaStatusList);
        }

        public async Task<List<AreaStatusVM>> GenerateAreaStatusList()
        {
            List<Area> lAreaList = await GetAreas();
            List<EquipmentAreaStatusVM> tEquipmentAreaStatusList = await GenerateEquipmentAreaStatusList();
            int i = 0, j = 0, x = 0;
            int tmpTaskCounter = 0, tmpOverdueCounter = 0, tmpInspItemCounter = 0, tmpActItemCounter = 0;

            for (i = 0; i < lAreaList.Count(); i++)
            {
                for (j = 0; j < tEquipmentAreaStatusList.Count(); j++)
                {
                    if (tEquipmentAreaStatusList[j].AreaID.ToString() == lAreaList[i].AreaID.ToString())
                    {
                        tmpTaskCounter = tmpTaskCounter + tEquipmentAreaStatusList[j].TotalTasks;
                        tmpOverdueCounter = tmpOverdueCounter + tEquipmentAreaStatusList[j].TotalOverdue;
                        tmpInspItemCounter = tmpInspItemCounter + tEquipmentAreaStatusList[j].TotalInspection;
                        tmpActItemCounter = tmpActItemCounter + tEquipmentAreaStatusList[j].TotalAction;
                    }
                }
                tAreaStatus.Add(new AreaStatusVM()
                {
                    AreaID = lAreaList[i].AreaID.ToString(),
                    AreaName = lAreaList[i].AreaName,
                    TotalTasks = tmpTaskCounter,
                    TotalOverdue = tmpOverdueCounter,
                    TotalInspection = tmpInspItemCounter,
                    TotalAction = tmpActItemCounter
                });
                tmpTaskCounter = 0;
                tmpOverdueCounter = 0;
                tmpInspItemCounter = 0;
                tmpActItemCounter = 0;
            }
            return tAreaStatus;
        }

        public async Task<JsonResult> GetAreaStatus()
        {
            List<AreaStatusVM> tAreaStatus = await GenerateAreaStatusList();

            return Json(tAreaStatus);
        }

        public EquipmentAreaStatusListVM GetEAreaStatusList(List<LTask> tList, List<Term> TrmList, List<TaskTracker> tTList, List<EquipmentArea> tEAList, List<Equipment> tEquipList, List<Area> tAreaList)
        {
           
            int j = tList.Count;
            int i = 0;
            int k = 0;

            string temp1;
            string temp2;
            string temp3;
            string temp4;
            int temp5= new Int32();
            double temp6;
            DateTime temp7;
            
            string[,] TListStatuses = new string[j, 9];
            string[,] equipStatuses = new string[tEquipList.Count, 7];
            string[,] eAreaStatuses = new string[tEAList.Count, 7];
            string[,] areaStatuses = new string[tAreaList.Count, 7];
            
            if (tTList.Count != 0) //Determine there is existing tracker data to search through
            {
                var result = tTList.OrderByDescending(x => x.TaskTrackerCompletionDate).First();
                for (i = 0; i < j; i++) //Build a list of the tasks determining due status and days remaining/overdue
                {
                    TListStatuses[i, 0] = tList[i].LTaskID.ToString();
                    TListStatuses[i, 4] = tList[i].EquipmentID.ToString();
                    TListStatuses[i, 7] = tList[i].Term.TermDays.ToString();

                    temp6 = Convert.ToDateTime(eaToday).ToOADate();
                    temp6 = temp6 - Convert.ToDouble(tList[i].Term.TermDays);
                    
                    temp7 = DateTime.FromOADate(temp6);
                    temp3 = temp7.ToShortDateString();

                    TListStatuses[i, 8] = (DateTime.FromOADate(Convert.ToDouble(Convert.ToDateTime(eaToday).ToOADate())-Convert.ToDouble(tList[i].Term.TermDays))).ToShortDateString();

                    if(tList[i].TaskTypeID.ToString() == "bf232fd4-a0c2-4e6d-b9b4-08d57b1433ff")
                    {
                        TListStatuses[i, 3] = "TaskNotUsed";
                    }
                   
                    if (tList[i].TaskTypeID.ToString() != "bf232fd4-a0c2-4e6d-b9b4-08d57b1433ff") //Ensure we don't count tasks not being used.
                    {
                        //Look for the last completion date in the task tracker table for this task.
                        var temp = from t in tTList
                                   where t.LTaskID.ToString() == tList[i].LTaskID.ToString()
                                   group t by t.LTaskID into tgrp
                                   let topp = tgrp.Max(x => x.TaskTrackerCompletionDate)
                                   select new
                                   {
                                       MostRecentID = tgrp.First(y => y.TaskTrackerCompletionDate == topp).TaskTrackerID,
                                       MostRecentTID = tgrp.First(y => y.TaskTrackerCompletionDate == topp).LTaskID,
                                       MostRecentDate = topp
                                   };

                        foreach (var a in temp)
                        {
                            if (k == 0)
                            {
                                //Calculate remaining / overdue days (+ = remaining; - = overdue)
                                temp1 = Convert.ToDateTime(a.MostRecentDate).ToShortDateString();
                                temp2 = tList[i].Term.TermDays.ToString();


                                temp3 = Convert.ToDateTime(eaToday).ToShortDateString();
                                temp7 = Convert.ToDateTime(temp3);
                                temp6 = Convert.ToDateTime(eaToday).ToOADate();
                                temp6 = Convert.ToDateTime(a.MostRecentDate).ToOADate();

                                temp6 = temp7.ToOADate();

                                TListStatuses[i, 3] = (Convert.ToDateTime(a.MostRecentDate).ToOADate()).ToString();
                                
                                //temp5 = Convert.ToInt32(temp3);
                                temp5 = Convert.ToInt32(Convert.ToDateTime(a.MostRecentDate).ToOADate() + tList[i].Term.TermDays) - Convert.ToInt32(Convert.ToDateTime(eaToday).ToOADate());
                                TListStatuses[i, 1] = temp5.ToString();


                                if (Convert.ToInt64(TListStatuses[i, 1]) < 0)
                                {
                                    TListStatuses[i, 2] = "X"; //Overdue    
                                }
                                else
                                {
                                    TListStatuses[i, 2] = "S";//Not due
                                }

                                if (tList[i].TaskType.TaskTypeDisplayLetter == "I")
                                {
                                    TListStatuses[i, 5] = Convert.ToInt32(TListStatuses[i, 5]) + 1.ToString(); //Add to inspection items
                                }
                                else
                                {
                                    TListStatuses[i, 6] = Convert.ToInt32(TListStatuses[i, 6]) + 1.ToString();//Add to Action Items
                                }
                            }
                        }
                    }
                }

                k = i;
                
                //-----------------------------------------------------------------------------------------//
                //////~~~~~~~~~~~~~ TODO : Seperate Inspection Items and Action Items ~~~~~~~~~~~~~~~~//////
                //---------------------------------------------------------------------------------------//

                for (i = 0; i < tEquipList.Count; i++) // Buid a list of equipment counting total and overdue tasks for each.
                {
                    equipStatuses[i, 0] = tEquipList[i].EquipmentID.ToString();
                    equipStatuses[i, 1] = tEquipList[i].EquipmentName.ToString();
                    equipStatuses[i, 2] = tEquipList[i].EquipmentAreaID.ToString();
                    
                    for (j = 0; j < k; j++)
                    {
                        if (TListStatuses[j, 4] == tEquipList[i].EquipmentID.ToString())
                        {
                            equipStatuses[i, 3] = (Convert.ToInt64(equipStatuses[i, 3]) + 1).ToString(); //This will count how many tasks per equipment
                            if(TListStatuses[j, 2] == "X")
                            {
                                equipStatuses[i, 4] = (Convert.ToInt64(equipStatuses[i, 4]) + 1).ToString(); //This will count total overdue tasks per equipment
                            }
                            if (TListStatuses[j, 5] != null)
                            {
                                equipStatuses[i, 5] = (Convert.ToInt32(equipStatuses[i, 5]) + 1).ToString();//I
                            }
                            else if (TListStatuses[j, 6] != null)
                            {
                                equipStatuses[i, 6] = (Convert.ToInt32(equipStatuses[i, 6]) + 1).ToString();//A
                            }
                        }
                    }
                }
                k = i;
                for (i = 0; i < tEAList.Count; i++ )
                {
                    eAreaStatuses[i, 0] = tEAList[i].EquipmentAreaID.ToString(); //Equipment Area ID
                    eAreaStatuses[i, 1] = tEAList[i].EquipmentAreaName.ToString(); // Equipment Area Name
                    eAreaStatuses[i, 2] = tEAList[i].AreaID.ToString(); //Parent Area Name
                    for (j = 0; j < k; j++)
                    {
                        if (equipStatuses[j, 2] == tEAList[i].EquipmentAreaID.ToString())
                        {
                            eAreaStatuses[i, 3] = (Convert.ToInt16(eAreaStatuses[i,3]) + Convert.ToInt16(equipStatuses[j, 3])).ToString(); //Calculate total tasks per equipment area
                            eAreaStatuses[i, 4] = (Convert.ToInt16(eAreaStatuses[i, 4]) + Convert.ToInt16(equipStatuses[j, 4])).ToString(); //Calculate total overdue tasks per equipment area
                            if (equipStatuses[j,5] != null)
                            {
                                eAreaStatuses[i, 5] = (Convert.ToInt32(eAreaStatuses[i, 5]) + Convert.ToInt16(equipStatuses[j,5])).ToString();
                            }
                            if (equipStatuses[j,6] != null)
                            {
                                eAreaStatuses[i, 6] = (Convert.ToInt32(eAreaStatuses[i, 6]) + Convert.ToInt16(equipStatuses[j,6])).ToString();
                            }
                        }
                    }
                }
                k = i;
                for (i=0; i < tAreaList.Count; i++)
                {
                    areaStatuses[i, 0] = tAreaList[i].AreaID.ToString(); //Area ID
                    areaStatuses[i, 1] = tAreaList[i].AreaName.ToString(); // Area Name
                    for (j = 0; j < k; j++)
                    {
                        if (eAreaStatuses[j, 2] == areaStatuses[i, 0])
                        {
                            areaStatuses[i, 3] = (Convert.ToInt16(areaStatuses[i, 3]) + Convert.ToInt16(eAreaStatuses[j, 3])).ToString(); //Total tasks per Area
                            areaStatuses[i, 4] = (Convert.ToInt16(areaStatuses[i, 4]) + Convert.ToInt16(eAreaStatuses[j, 4])).ToString(); //Total overdue tasks per Area
                            if(eAreaStatuses[j,5] != null)
                            {
                                areaStatuses[i, 5] = (Convert.ToInt32(areaStatuses[i, 5]) + Convert.ToInt32(eAreaStatuses[j,5])).ToString();

                            }
                            if (eAreaStatuses[j, 6] != null)
                            {
                                areaStatuses[i, 6] = (Convert.ToInt32(areaStatuses[i, 6]) + Convert.ToInt32(eAreaStatuses[j,6])).ToString();
                            }
                        }
                    }
                }

            }
            
            eaStatusListVM = new EquipmentAreaStatusListVM
            {
                equipSArray = equipStatuses,
                equipAreaSArray = eAreaStatuses,
                areaSArray = areaStatuses,
                taskSArray = TListStatuses
            };

            return (eaStatusListVM);
        }

        public EquipmentAreaStatusListVM eaStatusListVM;

        public async Task<JsonResult> getStatusVM()
        {
            var viewModel = GetEAreaStatusList(await GetLTasks(), await GetTerms(), await GetTaskTracker(), await GetEquipmentAreas(), await GetEquipment(), await GetAreas());

            string output = JsonConvert.SerializeObject(viewModel);
            return Json(output);

        }
    }
}