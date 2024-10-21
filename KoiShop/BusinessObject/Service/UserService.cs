using AutoMapper;
using BusinessObject.IService;
using BusinessObject.Model.RequestDTO;
using BusinessObject.Model.ResponseDTO;
using BusinessObject.Utils;
using DataAccess.Entity;
using DataAccess.IRepo;
using Microsoft.EntityFrameworkCore;
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
                if (phoneExist != null)
                {
                    res.Success = false;
                    res.Message = "User with this phone already exists.";
                    return res;
                }
                var passwordHash = Util.HashPassword(userDTO.Password,_configuration["PasswordSalt"]);
                var newUser = _mapper.Map<User>(userDTO);
                newUser.PasswordHash = passwordHash;
                newUser.Status = UserStatusEnum.Active;
                newUser.RoleId = 4;
                await _userRepo.CreateUser(newUser);

                CreateCartDTO userCart = new CreateCartDTO()
                {
                    UserId=newUser.UserId,
                };
                var cartmapp = _mapper.Map<UserCart>(userCart);
                await _cartRepo.AddAsync(cartmapp);

                var result = _mapper.Map<ResponseUserDTO>(newUser);
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
                var passwordHash = Util.HashPassword(CreateStaffDTO.Password, _configuration["PasswordSalt"]);
                var newStaff = _mapper.Map<User>(CreateStaffDTO);
                newStaff.PasswordHash = passwordHash;
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
                var passwordHash = Util.HashPassword(CreateManagerDTO.Password, _configuration["PasswordSalt"]);
                var newStaff = _mapper.Map<User>(CreateManagerDTO);
                newStaff.PasswordHash = passwordHash;
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

        public async Task<ServiceResponseFormat<ResponseUserDTO>> UpdateProfile(int id, UpdateProfileDTO updateProfileDTO)
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
                if (updateProfileDTO.ChangePassword)
                {
                    if (string.IsNullOrEmpty(updateProfileDTO.CurrentPassword) ||
                        string.IsNullOrEmpty(updateProfileDTO.NewPassword) ||
                        string.IsNullOrEmpty(updateProfileDTO.ConfirmPassword))
                    {
                        res.Success = false;
                        res.Message = "Please provide current password, new password, and confirm password.";
                        return res;
                    }

                    var isCurrentPasswordValid = Util.VerifyPassword(updateProfileDTO.CurrentPassword, user.PasswordHash, _configuration["PasswordSalt"]);
                    if (!isCurrentPasswordValid)
                    {
                        res.Success = false;
                        res.Message = "Current password is incorrect.";
                        return res;
                    }

                    if (updateProfileDTO.NewPassword != updateProfileDTO.ConfirmPassword)
                    {
                        res.Success = false;
                        res.Message = "New password and confirm password do not match.";
                        return res;
                    }

                    user.PasswordHash = Util.HashPassword(updateProfileDTO.NewPassword, _configuration["PasswordSalt"]);
                }

                //_mapper.Map(updateProfileDTO, user);
                var updateUser = await _userRepo.UpdateProfile(user);
                if (updateUser)
                {
                    var userResponse = _mapper.Map<ResponseUserDTO>(user);
                    res.Success = true;
                    res.Message = "Updated Successfully.";
                    res.Data = userResponse;
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

                if (user.RoleId == 1)
                {
                    res.Success = false;
                    res.Message = "THIS IS MANAGER, OK?";
                    return res;
                }

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
                    res.Message = "No User Found.";
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
        public async Task<ResponseUserDTO> GetUserProfile(int userId)
        {
            var user = await _userRepo.GetById(userId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var userDTO = _mapper.Map<ResponseUserDTO>(user);
            return userDTO;
        }

        public async Task<ServiceResponseFormat<ResponseUserDTO>> LoginUser(string email, string pass)
        {
            var res = new ServiceResponseFormat<ResponseUserDTO>();
            try
            {
                var user = await _userRepo.GetByEmail(email);
                if (user==null)
                {
                    res.Success = false;
                    res.Message = "Email Not Found";
                    return res;
                }
                bool veri = Utils.Util.VerifyPassword(pass, user.PasswordHash, _configuration["PasswordSalt"]);
                if (!veri || user == null)
                {
                    res.Success = false;
                    res.Message = "Invalid password.";
                    return res;
                }
                if (user.Status != UserStatusEnum.Active)
                {
                    res.Success = false;
                    res.Message = "Your account is being blocked.";
                    return res;
                }

                var responseUser = _mapper.Map<ResponseUserDTO>(user);
                responseUser.RoleName = user.Role?.RoleName;
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
        public async Task<ServiceResponseFormat<ResponseUserDTO>> UpdateUser(int id, UpdateUserDTO updateUserDTO)
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
        
        public async Task<ServiceResponseFormat<string>> ForgotPassword(RequestPasswordResetDTO request)
        {
            var res = new ServiceResponseFormat<string>();
            try
            {
                var user = await _userRepo.GetByEmail(request.Email);
                if (user == null)
                {
                    res.Success = false;
                    res.Message = "If the email is registered, a reset link will be sent.";
                    return res;
                }

                var token = Guid.NewGuid().ToString();
                var passwordResetToken = new PasswordResetToken
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    Token = token,
                    ExpirationTime = DateTime.UtcNow.AddMinutes(10)
                };

                await _userRepo.AddToken(passwordResetToken);
                var resetLink = $"{_configuration["FrontendUrl"]}/reset-password?token={token}";
                await _emailService.SendResetPasswordEmail(user.Email, token);

                res.Success = true;
                res.Message = "If the email is registered, a reset link will be sent.";
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Error processing request: {ex.Message}";
            }
            return res;
        }
        public async Task<ServiceResponseFormat<string>> ResetPassword(ResetPasswordDTO request)
        {
            var res = new ServiceResponseFormat<string>();
            try
            {
                if (request.NewPassword != request.ConfirmPassword)
                {
                    res.Success = false;
                    res.Message = "New password and confirm password do not match.";
                    return res;
                }

                var token = await _userRepo.GetToken(request.Token);
                if (token == null || token.ExpirationTime < DateTime.UtcNow)
                {
                    res.Success = false;
                    res.Message = "Invalid or expired token.";
                    return res;
                }

                var user = await _userRepo.GetById(token.UserId);
                if (user == null)
                {
                    res.Success = false;
                    res.Message = "User not found.";
                    return res;
                }

                user.PasswordHash = Util.HashPassword(request.NewPassword, _configuration["PasswordSalt"]);
                var updateResult = await _userRepo.UpdateProfile(user);
                if (!updateResult)
                {
                    res.Success = false;
                    res.Message = "Failed to update the password.";
                    return res;
                }

                await _userRepo.RemoveToken(token.Id);

                res.Success = true;
                res.Message = "Password has been reset successfully.";
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = $"Error resetting password: {ex.Message}";
            }
            return res;
        }

    }
}
