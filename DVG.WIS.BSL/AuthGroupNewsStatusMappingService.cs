using DVG.WIS.DAL.Infrastructure;
using DVG.WIS.DAL.Repositories;
using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Business
{
    public interface IAuthGroupNewsStatusMappingService
    {
        void Add(Entities.AuthGroupNewsStatusMapping post);

        void Update(Entities.AuthGroupNewsStatusMapping post);

        void Delete(int id);

        IEnumerable<Entities.AuthGroupNewsStatusMapping> GetAll();

        IEnumerable<Entities.AuthGroupNewsStatusMapping> GetAllPaging(int page, int pageSize, out int totalRow);

        IEnumerable<Entities.AuthGroupNewsStatusMapping> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow);

        Entities.AuthGroupNewsStatusMapping GetById(int id);
        void SaveChanges();
    }
    public class AuthGroupNewsStatusMappingService : IAuthGroupNewsStatusMappingService
    {
        IAuthGroupNewsStatusMappingRepository _authGroupNewsStatusMappingRepository;
        IUnitOfWork _unitOfWork;
        public AuthGroupNewsStatusMappingService(IAuthGroupNewsStatusMappingRepository authGroupNewsStatusMappingRepository, IUnitOfWork unitOfWork)
        {
            _authGroupNewsStatusMappingRepository = authGroupNewsStatusMappingRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(Entities.AuthGroupNewsStatusMapping post)
        {
            _authGroupNewsStatusMappingRepository.Add(post);
        }

        public void Delete(int id)
        {
            _authGroupNewsStatusMappingRepository.Delete(id);
        }

        public IEnumerable<Entities.AuthGroupNewsStatusMapping> GetAll()
        {
            return _authGroupNewsStatusMappingRepository.GetAll();
        }

        public IEnumerable<Entities.AuthGroupNewsStatusMapping> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow)
        {
            return _authGroupNewsStatusMappingRepository.GetMultiPaging(null, out totalRow, page, pageSize);
        }

        public IEnumerable<Entities.AuthGroupNewsStatusMapping> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return _authGroupNewsStatusMappingRepository.GetMultiPaging(null, out totalRow, page, pageSize);
        }

        public Entities.AuthGroupNewsStatusMapping GetById(int id)
        {
            return _authGroupNewsStatusMappingRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Entities.AuthGroupNewsStatusMapping post)
        {
            _authGroupNewsStatusMappingRepository.Update(post);
        }
    }
}
