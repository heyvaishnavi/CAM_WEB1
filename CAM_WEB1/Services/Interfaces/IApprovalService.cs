using CAM_WEB1.DTO;
using CAM_WEB1.Models;

namespace CAM_WEB1.Services.Interfaces
{
    public interface IApprovalService
    {
        Task<List<Approval>> GetAll();

        Task<Approval> GetById(int id);

        Task SubmitDecision(int id, ApprovalDecisionDTO request, string reviewerId);
    }
}
