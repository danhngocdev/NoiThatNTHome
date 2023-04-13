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
    public interface IAuthGroupCategoryMappingService
    {
        void Add(Entities.AuthGroupCategoryMapping post);

        void Update(Entities.AuthGroupCategoryMapping post);

        void Delete(int id);

        IEnumerable<Entities.AuthGroupCategoryMapping> GetAll();

        IEnumerable<Entities.AuthGroupCategoryMapping> GetAllPaging(int page, int pageSize, out int totalRow);

        IEnumerable<Entities.AuthGroupCategoryMapping> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow);

        Entities.AuthGroupCategoryMapping GetById(int id);
        void SaveChanges();
    }
    public class AuthGroupCategoryMappingService : IAuthGroupCategoryMappingService
    {
        IAuthGroupCategoryMappingRepository _authGroupCategoryMappingRepository;
        IUnitOfWork _unitOfWork;
        public AuthGroupCategoryMappingService(IAuthGroupCategoryMappingRepository authGroupCategoryMappingRepository, IUnitOfWork unitOfWork)
        {
            _authGroupCategoryMappingRepository = authGroupCategoryMappingRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(Entities.AuthGroupCategoryMapping post)
        {
            _authGroupCategoryMappingRepository.Add(post);
        }

        public void Delete(int id)
        {
            _authGroupCategoryMappingRepository.Delete(id);
        }

        public IEnumerable<Entities.AuthGroupCategoryMapping> GetAll()
        {
            return _authGroupCategoryMappingRepository.GetAll();
        }

        public IEnumerable<Entities.AuthGroupCategoryMapping> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow)
        {
            return _authGroupCategoryMappingRepository.GetMultiPaging(null, out totalRow, page, pageSize);
        }

        public IEnumerable<Entities.AuthGroupCategoryMapping> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return _authGroupCategoryMappingRepository.GetMultiPaging(null, out totalRow, page, pageSize);
        }

        public Entities.AuthGroupCategoryMapping GetById(int id)
        {
            return _authGroupCategoryMappingRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Entities.AuthGroupCategoryMapping post)
        {
            _authGroupCategoryMappingRepository.Update(post);
        }
    }
}
