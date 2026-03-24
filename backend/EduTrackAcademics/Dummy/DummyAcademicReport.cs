using System;
using System.Collections.Generic;
using EduTrackAcademics.Model;

namespace EduTrackAcademics.Dummy
{
    public class DummyAcademicReport
    {

        public static readonly List<AcademicReport> Reports = new List<AcademicReport>
        {
   new AcademicReport
   {
       ReportId = "R101",
       Scope = "Batch A",
       CompletionRate = 90.5m,
       AvgScore = 85.2m,
       DropOutRate = 5.3m,
       GeneratedDate = DateTime.Now
   },
   new AcademicReport
   {
       ReportId = "R102",
       Scope = "Batch B",
       CompletionRate = 88.1m,
       AvgScore = 80.4m,
       DropOutRate = 6.5m,
       GeneratedDate = DateTime.Now
   }
};
    }
}
