using CAM_WEB1.DTO;
using CAM_WEB1.Models;
using CAM_WEB1.Repositories.Interfaces;
using CAM_WEB1.Services.Interfaces;

namespace CAM_WEB1.Services.Implementations
{
    public class ApprovalService : IApprovalService
    {
        private readonly IApprovalRepository _repo;

        public ApprovalService(IApprovalRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Approval>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<Approval> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task SubmitDecision(int id, ApprovalDecisionDTO request, string reviewerId)
        {
            if (string.IsNullOrEmpty(request.Decision))
                throw new Exception("Decision is required");

            if (request.Decision.ToLower() != "approve" &&
                request.Decision.ToLower() != "reject")
                throw new Exception("Invalid decision type");

            await _repo.SubmitDecision(id, request, reviewerId);
        }
    }
}
