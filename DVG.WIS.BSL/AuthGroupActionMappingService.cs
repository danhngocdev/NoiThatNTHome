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
    public interface IAuthGroupActionMappingService
    {
        void Add(Entities.AuthGroupActionMapping post);

        void Update(Entities.AuthGroupActionMapping post);

        void Delete(int id);

        IEnumerable<Entities.AuthGroupActionMapping> GetAll();

        IEnumerable<Entities.AuthGroupActionMapping> GetAllPaging(int page, int pageSize, out int totalRow);

        IEnumerable<Entities.AuthGroupActionMapping> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow);

        Entities.AuthGroupActionMapping GetById(int id);
        void SaveChanges();
    }
    public class AuthGroupActionMappingService : IAuthGroupActionMappingService
    {
        IAuthGroupActionMappingRepository _authGroupActionMappingRepository;
        IUnitOfWork _unitOfWork;
        public AuthGroupActionMappingService(IAuthGroupActionMappingRepository authGroupActionMappingRepository, IUnitOfWork unitOfWork)
        {
            _authGroupActionMappingRepository = authGroupActionMappingRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(Entities.AuthGroupActionMapping post)
        {
            _authGroupActionMappingRepository.Add(post);
        }

        public void Delete(int id)
        {
            _authGroupActionMappingRepository.Delete(id);
        }

        public IEnumerable<Entities.AuthGroupActionMapping> GetAll()
        {
            return _authGroupActionMappingRepository.GetAll();
        }

        public IEnumerable<Entities.AuthGroupActionMapping> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow)
        {
            return _authGroupActionMappingRepository.GetMultiPaging(null, out totalRow, page, pageSize);
        }

        public IEnumerable<Entities.AuthGroupActionMapping> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return _authGroupActionMappingRepository.GetMultiPaging(null, out totalRow, page, pageSize);
        }

        public Entities.AuthGroupActionMapping GetById(int id)
        {
            return _authGroupActionMappingRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Entities.AuthGroupActionMapping post)
        {
            _authGroupActionMappingRepository.Update(post);
        }
    }
}
