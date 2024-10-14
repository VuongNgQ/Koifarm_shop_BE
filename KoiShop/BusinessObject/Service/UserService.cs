using AutoMapper;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using BusinessObject.Utils;
using DataAccess.Entity;
using DataAccess.IRepo;

namespace BusinessObject.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IRoleRepo _roleRepo;
        public UserService(IUserRepo repo, IMapper mapper, IRoleRepo roleRepo)
        {
            _userRepo = repo;
            _mapper = mapper;
            _roleRepo = roleRepo;
        }
        public async Task<ServiceResponseFormat<ResponseUserDTO>> CreateUser(CreateUserDTO userDTO)
        {
            var res = new ServiceResponseFormat<ResponseUserDTO>();
            try
            {
                var emailExist = await _userRepo.GetByEmail(userDTO.Email);
                var phoneExist = await _userRepo.GetByPhone(userDTO.Phone);
                if (emailExist != null)
                {
                    res.Success = false;
                    res.Message = "User with this email already exists.";
                    return res;
                }
                else if (phoneExist != null)
                {
                    res.Success = false;
                    res.Message = "User with this phone already exists.";
                    return res;
                }
                var mapp = _mapper.Map<User>(userDTO);
                mapp.Status = "Active";
                mapp.RoleId = 3;
                await _userRepo.CreateUser(mapp);

                var result = _mapper.Map<ResponseUserDTO>(mapp);
                res.Success = true;
                res.Message = "User created successfully";
                res.Data = result;
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to create User:{ex.Message}";
            }
            return res;
        }
        public async Task<ServiceResponseFormat<ResponseUserDTO>> CreateStaff(CreateUserDTO CreateStaffDTO)
        {
            var res = new ServiceResponseFormat<ResponseUserDTO>();
            try
            {
                var emailExist = await _userRepo.GetByEmail(CreateStaffDTO.Email);
                var phoneExist = await _userRepo.GetByPhone(CreateStaffDTO.Phone);
                if (emailExist != null)
                {
                    res.Success = false;
                    res.Message = "User with this email already exists.";
                    return res;
                }
                else if (phoneExist != null)
                {
                    res.Success = false;
                    res.Message = "User with this phone already exists.";
                    return res;
                }

                var newStaff = _mapper.Map<User>(CreateStaffDTO);
                newStaff.Status = "Active";
                newStaff.RoleId = 2;

                var createdUser = await _userRepo.CreateUser(newStaff);
                if (createdUser != null)
                {
                    res.Success = true;
                    res.Message = "Staff created successfully.";
                    res.Data = _mapper.Map<ResponseUserDTO>(createdUser);
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "Failed to create staff. Repository error.";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Error creating staff: {ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<UpdateProfileDTO>> UpdateProfile(int id, UpdateProfileDTO updateProfileDTO)
        {
            var res = new ServiceResponseFormat<UpdateProfileDTO>();
            try
            {
                var user = await _userRepo.GetById(id);
                if (user == null)
                {
                    res.Success = false;
                    res.Message = "User not found.";
                    return res;
                }

                var emailExist = await _userRepo.GetByEmail(updateProfileDTO.Email);
                var phoneExist = await _userRepo.GetByPhone(updateProfileDTO.Phone);
                if (emailExist != null && emailExist.UserId != user.UserId)
                {
                    res.Success = false;
                    res.Message = "User with this Email already exist";
                    return res;
                }
                if (phoneExist != null && phoneExist.UserId != user.UserId)
                {
                    res.Success = false;
                    res.Message = "User with this Phone already exist";
                    return res;
                }

                user.Name = updateProfileDTO.Name ?? user.Name;
                user.Email = updateProfileDTO.Email ?? user.Email;
                user.Phone = updateProfileDTO.Phone ?? user.Phone;
                user.DateOfBirth = updateProfileDTO.DateOfBirth;

                var updateUser = await _userRepo.UpdateUser(id, user);
                if (updateUser != null)
                {
                    res.Success = true;
                    res.Message = "Updated Successfully.";
                    res.Data = _mapper.Map<UpdateProfileDTO>(user);
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "Failed to update User. Repository error.";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Error updating profile: {ex.Message}";
            }
            return res;
        }

        public async Task<ServiceResponseFormat<bool>> DeleteUser(int id)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                var roleRestrict = await _userRepo.GetByIdAsync(id);
                if (roleRestrict != null)
                {
                    if (roleRestrict.RoleId == 1)
                    {
                        res.Success = false;
                        res.Message = "THIS IS MANAGER, OK?";
                        return res;
                    }
                }
                var result=await _userRepo.DeleteUser(id);
                if (result)
                {
                    res.Success = true;
                    res.Message = "User Deleted successfully";
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No User found";
                    return res;
                }
            }
            catch(Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to delete User:{ex.Message}";
                return res;
            }
        }

        public async Task<ServiceResponseFormat<PaginationModel<ResponseUserDTO>>> GetAllUser(int page, int pageSize,
            string? search, string sort)
        {
            var res = new ServiceResponseFormat<PaginationModel<ResponseUserDTO>>();
            try
            {
                var users = await _userRepo.GetAllUser();
                if (!string.IsNullOrEmpty(search))
                {
                    users = users.Where(e => e.Name.Contains(search, StringComparison.OrdinalIgnoreCase)||
                    e.Email.Contains(search, StringComparison.OrdinalIgnoreCase)||
                    e.Phone.Contains(search, StringComparison.OrdinalIgnoreCase));
                }
                users = sort.ToLower().Trim() switch
                {
                    "name" => users.OrderBy(e => e.Name),
                    "birthday"=>users.OrderBy(e => e.DateOfBirth),
                    _ => users.OrderBy(e => e.UserId)
                };
                var mapp = _mapper.Map<IEnumerable<ResponseUserDTO>>(users);
                if (mapp.Any())
                {
                    var paginationModel = await Pagination.GetPaginationEnum(mapp, page, pageSize);
                    res.Success = true;
                    res.Message = "Get Users successfully";
                    res.Data = paginationModel;
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No User";
                    return res;
                }
            }
            catch (Exception ex) 
            {
                res.Success = false;
                res.Message = $"Fail to get User:{ex.Message}";
            }
            return res;
        }

        public async Task<User> LoginUser(string email, string pass)
        {
            try
            {
                return await _userRepo.Login(email, pass);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to login: {ex.Message}");
            }
        }
        //public async Task<ServiceResponseFormat<bool>> LoginAdmin(string email, string pass)
        //{
        //    var res = new ServiceResponseFormat<bool>();
        //    try
        //    {
        //        var result = await _userRepo.LoginAdmin(email, pass);
        //        if (result)
        //        {
        //            res.Success = true;
        //            res.Message = "Login Successfully";
        //            res.Data = result;
        //            return res;
        //        }
        //        else
        //        {
        //            res.Success = false;
        //            res.Message = "Password does not match the Email";
        //            return res;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Success = false;
        //        res.Message = $"Login fail:{ex.Message}";
        //        return res;
        //    }
        //}
        //public async Task<ServiceResponseFormat<bool>> LoginCustomer(string email, string pass)
        //{
        //    var res = new ServiceResponseFormat<bool>();
        //    try
        //    {
        //        var result = await _userRepo.LoginCustomer(email, pass);
        //        if (result)
        //        {
        //            res.Success = true;
        //            res.Message = "Login Successfully";
        //            res.Data = result;
        //            return res;
        //        }
        //        else
        //        {
        //            res.Success = false;
        //            res.Message = "Password does not match the Email";
        //            return res;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Success = false;
        //        res.Message = $"Login fail:{ex.Message}";
        //        return res;
        //    }
        //}
        //public async Task<ServiceResponseFormat<bool>> LoginStaff(string email, string pass)
        //{
        //    var res = new ServiceResponseFormat<bool>();
        //    try
        //    {
        //        var result = await _userRepo.LoginStaff(email, pass);
        //        if (result)
        //        {
        //            res.Success = true;
        //            res.Message = "Login Successfully";
        //            res.Data = result;
        //            return res;
        //        }
        //        else
        //        {
        //            res.Success = false;
        //            res.Message = "Password does not match the Email";
        //            return res;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Success = false;
        //        res.Message = $"Login fail:{ex.Message}";
        //        return res;
        //    }
        //}
        public async Task<ServiceResponseFormat<ResponseUserDTO>> UpdateUser(int id, ResponseUserDTO updateUserDTO)
        {
            var res = new ServiceResponseFormat<ResponseUserDTO>();
            try
            {
                var user = await _userRepo.GetById(id);
                if (user == null)
                {
                    res.Success = false;
                    res.Message = "User not found.";
                    return res;
                }
                var emailExist = await _userRepo.GetByEmail(updateUserDTO.Email);
                var phoneExist = await _userRepo.GetByPhone(updateUserDTO.Phone);
                if (emailExist != null && emailExist.UserId != user.UserId)
                {
                    res.Success = false;
                    res.Message = "User with this Email already exist";
                    return res;
                }
                if (phoneExist != null && phoneExist.UserId != user.UserId)
                {
                    res.Success = false;
                    res.Message = "User with this Phone already exist";
                    return res;
                }
                if (user.RoleId == 1)
                {
                    res.Success = false;
                    res.Message = "YOU CAN'T UPDATE MANAGER, OK?";
                    return res;
                }

                user.Name = updateUserDTO.Name ?? user.Name;
                user.Email = updateUserDTO.Email ?? user.Email;
                user.Phone = updateUserDTO.Phone ?? user.Phone;
                user.DateOfBirth = updateUserDTO.DateOfBirth;

                //var mapp = _mapper.Map<User>(updateUserDTO);
                var updateUser = await _userRepo.UpdateUser(id, user);
                if (updateUser != null)
                {
                    res.Success = true;
                    res.Message = "User Updated Successfully";
                    res.Data = _mapper.Map<ResponseUserDTO>(user); ;
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "No User Found or got error at Repo";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to update User:{ex.Message}";
                return res;
            }
        }
    }
}
