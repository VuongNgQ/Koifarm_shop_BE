using AutoMapper;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using DataAccess.Entity;
using DataAccess.IRepo;
using Microsoft.Extensions.Configuration;

namespace BusinessObject.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IRoleRepo _roleRepo;
        private readonly ICartRepo _cartRepo;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        public UserService(IUserRepo repo, IMapper mapper, IRoleRepo roleRepo, ICartRepo cartRepo, IEmailService emailService, IConfiguration configuration)
        {
            _userRepo = repo;
            _mapper = mapper;
            _roleRepo = roleRepo;
            _cartRepo = cartRepo;
            _emailService = emailService;
            _configuration = configuration;
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
                mapp.Status = UserStatusEnum.Active;
                mapp.RoleId = 4;
                await _userRepo.CreateUser(mapp);

                if (mapp.RoleId ==4)
                {
                    CreateCartDTO userCart = new CreateCartDTO()
                    {
                        UserId=mapp.UserId,
                    };
                    var cartmapp = _mapper.Map<UserCart>(userCart);
                    await _cartRepo.AddAsync(cartmapp);
                }

                var result = _mapper.Map<ResponseUserDTO>(mapp);
                res.Success = true;
                res.Message = "Created successfully";
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
                newStaff.Status = UserStatusEnum.Active;
                newStaff.RoleId = 3;

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
        public async Task<ServiceResponseFormat<ResponseUserDTO>> CreateManager(CreateUserDTO CreateManagerDTO)
        {
            var res = new ServiceResponseFormat<ResponseUserDTO>();
            try
            {
                var emailExist = await _userRepo.GetByEmail(CreateManagerDTO.Email);
                var phoneExist = await _userRepo.GetByPhone(CreateManagerDTO.Phone);
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

                var newStaff = _mapper.Map<User>(CreateManagerDTO);
                newStaff.Status = UserStatusEnum.Active;
                newStaff.RoleId = 2;

                var createdUser = await _userRepo.CreateUser(newStaff);
                if (createdUser != null)
                {
                    res.Success = true;
                    res.Message = "Manager created successfully.";
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

        public async Task<ServiceResponseFormat<UpdateUserDTO>> UpdateProfile(int id, UpdateProfileDTO updateProfileDTO)
        {
            var res = new ServiceResponseFormat<UpdateUserDTO>();
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

                user.Name = !string.IsNullOrWhiteSpace(updateProfileDTO.Name) ? updateProfileDTO.Name : user.Name;
                user.Email = !string.IsNullOrWhiteSpace(updateProfileDTO.Email) ? updateProfileDTO.Email : user.Email;
                user.Phone = !string.IsNullOrWhiteSpace(updateProfileDTO.Phone) ? updateProfileDTO.Phone : user.Phone;
                user.DateOfBirth = updateProfileDTO.DateOfBirth;
                if (!string.IsNullOrEmpty(updateProfileDTO.NewPassword))
                {
                    user.PasswordHash = updateProfileDTO.NewPassword;
                }
                var updateUser = await _userRepo.UpdateProfile(id, user);
                if (updateUser != null)
                {
                    res.Success = true;
                    res.Message = "Updated Successfully.";
                    res.Data = _mapper.Map<UpdateUserDTO>(user);
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
                var user = await _userRepo.GetByIdAsync(id);
                if (user == null)
                {
                    res.Success = false;
                    res.Message = "User not found.";
                    return res;
                }

                // Kiểm tra nếu người dùng là Manager thì không được phép xóa
                if (user.RoleId == 1)
                {
                    res.Success = false;
                    res.Message = "THIS IS MANAGER, OK?";
                    return res;
                }

                // Thay đổi trạng thái của người dùng thành "Unactive"
                user.Status = UserStatusEnum.Disable;
                var result = await _userRepo.UpdateUser(id, user);

                if (result != null)
                {
                    res.Success = true;
                    res.Message = "User status changed to Unactive successfully.";
                    res.Data = true;
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "Failed to update User status.";
                    res.Data = false;
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to change User status: {ex.Message}";
                res.Data = false;
                return res;
            }
        }
        public async Task<ServiceResponseFormat<bool>> RestoreUser(int id)
        {
            var res = new ServiceResponseFormat<bool>();
            try
            {
                var user = await _userRepo.GetByIdAsync(id);
                if (user == null)
                {
                    res.Success = false;
                    res.Message = "User not found.";
                    return res;
                }

                if (user.RoleId == 1)
                {
                    res.Success = false;
                    res.Message = "THIS IS MANAGER, OK?";
                    return res;
                }

                user.Status = UserStatusEnum.Active;
                var result = await _userRepo.UpdateUser(id, user);

                if (result != null)
                {
                    res.Success = true;
                    res.Message = "User status changed to Active successfully.";
                    res.Data = true;
                    return res;
                }
                else
                {
                    res.Success = false;
                    res.Message = "Failed to update User status.";
                    res.Data = false;
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Fail to change User status: {ex.Message}";
                res.Data = false;
                return res;
            }
        }

        public async Task<ServiceResponseFormat<bool>> RemoveUser(int id)
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
                    res.Message = "User remove successfully";
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
                res.Message = $"Fail to remove User:{ex.Message}";
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
                    var paginationModel = await Utils.Pagination.GetPaginationEnum(mapp, page, pageSize);
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
        public async Task<ServiceResponseFormat<CreateUserDTO>> GetUserById(int userId)
        {
            var res = new ServiceResponseFormat<CreateUserDTO>();
            var user = await _userRepo.GetById(userId);
            if (user == null)
            {
                res.Success = false;
                res.Message = "User not found.";
                return res;
            }

            res.Success = true;
            res.Data = _mapper.Map<CreateUserDTO>(user);
            return res;
        }

        public async Task<ServiceResponseFormat<ResponseUserDTO>> LoginUser(string email, string pass)
        {
            var res = new ServiceResponseFormat<ResponseUserDTO>();
            try
            {
                var user = await _userRepo.GetByEmail(email);
                bool veri = Utils.Util.VerifyPassword(pass, user.PasswordHash, _configuration["PasswordSalt"]);
                if (veri == null || user==null)
                {
                    res.Success = false;
                    res.Message = "Invalid email or password.";
                    return res;
                }

                var responseUser = _mapper.Map<ResponseUserDTO>(user);
                res.Success = true;
                res.Message = "Login successful.";
                res.Data = responseUser;
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Failed to login: {ex.Message}";
                return res;
            }
        }
        public async Task<ServiceResponseFormat<UpdateUserDTO>> UpdateUser(int id, UpdateUserDTO updateUserDTO)
        {
            var res = new ServiceResponseFormat<UpdateUserDTO>();
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

                user.Name = !string.IsNullOrWhiteSpace(updateUserDTO.Name) ? updateUserDTO.Name : user.Name;
                user.Email = !string.IsNullOrWhiteSpace(updateUserDTO.Email) ? updateUserDTO.Email : user.Email;
                user.PasswordHash = !string.IsNullOrWhiteSpace(updateUserDTO.Password) ? updateUserDTO.Password : user.PasswordHash;
                user.Phone = !string.IsNullOrWhiteSpace(updateUserDTO.Phone) ? updateUserDTO.Phone : user.Phone;
                user.DateOfBirth = updateUserDTO.DateOfBirth;

                var updateUser = await _userRepo.UpdateUser(id, user);
                if (updateUser != null)
                {
                    res.Success = true;
                    res.Message = "User Updated Successfully";
                    res.Data = _mapper.Map<UpdateUserDTO>(user); ;
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
        public async Task<bool> ValidateOldPassword(int userId, string oldPassword)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            return user.PasswordHash == oldPassword;
        }
        public async Task<bool> GeneratePasswordResetToken(string email)
        {
            var user = await _userRepo.GetByEmail(email);
            if (user == null) return false;

            var token = Guid.NewGuid().ToString();
            var expirationTime = DateTime.UtcNow.AddHours(1); // Token có hiệu lực 1 giờ

            var resetToken = new PasswordResetToken
            {
                Token = token,
                Email = email,
                ExpirationTime = expirationTime
            };

            if (await _userRepo.SavePasswordResetToken(resetToken))
            {
                // Gửi email cho người dùng với token
                //await _emailService.SendPasswordResetEmail(email, token);
                return true;
            }

            return false;
        }

        public async Task<bool> ResetPassword(string token, string newPassword)
        {
            var resetToken = await _userRepo.GetPasswordResetToken(token);
            if (resetToken == null || resetToken.ExpirationTime < DateTime.UtcNow)
            {
                return false; // Token không hợp lệ hoặc đã hết hạn
            }

            var user = await _userRepo.GetByEmail(resetToken.Email);
            if (user == null) return false;

            user.PasswordHash = newPassword; // Bảo mật: Bạn cần mã hóa password
            //await _userRepo.UpdateUser(user); // Cập nhật mật khẩu cho người dùng

            await _userRepo.DeletePasswordResetToken(token); // Xóa token sau khi sử dụng
            return true;
        }

        public async Task<ServiceResponseFormat<ResponseUserDTO>> GetUserByEmail(string email)
        {
            try
            {
                var res = new ServiceResponseFormat<ResponseUserDTO>();
                var user = await _userRepo.GetByEmail(email);
                if (user == null)
                {
                    res.Success = false;
                    res.Message = "User not found.";
                    return res;
                }

                res.Success = true;
                res.Data = _mapper.Map<ResponseUserDTO>(user);
                return res;
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
