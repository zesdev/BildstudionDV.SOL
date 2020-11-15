using BildstudionDV.BI.ViewModels;
using MongoDB.Bson;
using System.Collections.Generic;

namespace BildstudionDV.BI.ViewModelLogic
{
    public interface IJobbVMLogic
    {
        void AddJobb(JobbViewModel kundJobb);
        List<JobbViewModel> GetJobbsForKund(ObjectId kundId);
        JobbViewModel GetJobbViewModel(ObjectId jobbId);
        void RemoveAllJobsInKund(ObjectId kundId);
        void RemoveJobb(ObjectId jobbId);
        void UpdateJobb(JobbViewModel viewModel);
    }
}