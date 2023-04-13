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
    public interface IAuthGroupUserMappingService
    {
        void Add(Entities.AuthGroupUserMapping post);

        void Update(Entities.AuthGroupUserMapping post);

        void Delete(int id);

        IEnumerable<Entities.AuthGroupUserMapping> GetAll();

        IEnumerable<Entities.AuthGroupUserMapping> GetAllPaging(int page, int pageSize, out int totalRow);

        IEnumerable<Entities.AuthGroupUserMapping> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow);

        Entities.AuthGroupUserMapping GetById(int id);
        void SaveChanges();
    }
    public class AuthGroupUserMappingService : IAuthGroupUserMappingService
    {
        IAuthGroupUserMappingRepository _authGroupUserMappingRepository;
        IUnitOfWork _unitOfWork;
        public AuthGroupUserMappingService(IAuthGroupUserMappingRepository authGroupUserMappingRepository, IUnitOfWork unitOfWork)
        {
            _authGroupUserMappingRepository = authGroupUserMappingRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(Entities.AuthGroupUserMapping post)
        {
            _authGroupUserMappingRepository.Add(post);
        }

        public void Delete(int id)
        {
            _authGroupUserMappingRepository.Delete(id);
        }

        public IEnumerable<Entities.AuthGroupUserMapping> GetAll()
        {
            return _authGroupUserMappingRepository.GetAll();
        }

        public IEnumerable<Entities.AuthGroupUserMapping> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow)
        {
            return _authGroupUserMappingRepository.GetMultiPaging(null, out totalRow, page, pageSize);
        }

        public IEnumerable<Entities.AuthGroupUserMapping> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return _authGroupUserMappingRepository.GetMultiPaging(null, out totalRow, page, pageSize);
        }

        public Entities.AuthGroupUserMapping GetById(int id)
        {
            return _authGroupUserMappingRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Entities.AuthGroupUserMapping post)
        {
            _authGroupUserMappingRepository.Update(post);
        }
    }
}
