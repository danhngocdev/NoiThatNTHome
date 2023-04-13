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
    public interface IUserService
    {
        void Add(User post);

        void Update(User post);

        void Delete(int id);

        IEnumerable<User> GetAll();

        IEnumerable<User> GetAllPaging(int page, int pageSize, out int totalRow);

        IEnumerable<User> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow);

        User GetById(int id);
        void SaveChanges();
    }
    public class UserService : IUserService
    {
        IUserRepository _userRepository;
        IUnitOfWork _unitOfWork;
        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(User post)
        {
            _userRepository.Add(post);
        }

        public void Delete(int id)
        {
            _userRepository.Delete(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public IEnumerable<User> GetAllByCategoryPaging(int categoryId, int page, int pageSize, out int totalRow)
        {
            return _userRepository.GetMultiPaging(x => x.Status == 1, out totalRow, page, pageSize);
        }

        public IEnumerable<User> GetAllPaging(int page, int pageSize, out int totalRow)
        {
            return _userRepository.GetMultiPaging(x => x.Status == 1, out totalRow, page, pageSize);
        }

        public User GetById(int id)
        {
            return _userRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(User post)
        {
            _userRepository.Update(post);
        }
    }
}
